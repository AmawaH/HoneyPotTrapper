using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;
using HoneyPotTrapper.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using HoneyPotTrapper.Validations;
using HoneyPotTrapper.Models.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using HoneyPotTrapper.Infrastructure;
using System.Management.Automation;

namespace HoneyPotTrapper.Controllers
{
    public class ListeningPortsController : Controller
    {
        private IAppModel appModel;
        private IValidators validators;
        private IMessageViewModel messageViewModel;
        private readonly IHubContext<MessageHub> hubContext;
        public ListeningPortsController(IAppModel _appModel, IValidators _validators, IMessageViewModel _messageViewModel, IHubContext<MessageHub> _hubContext)
        {
            appModel = _appModel;
            validators = _validators;
            messageViewModel = _messageViewModel;
            hubContext = _hubContext;
        }
        public IActionResult Index()
        {
            bool inProgress = appModel.isInProgress();
            if (inProgress)
            {
                ViewBag.ButtonText = "STOP";
                ViewBag.ButtonStyle = "btn-danger";
            }
            else
            {
                ViewBag.ButtonText = "Start Listening";
                ViewBag.ButtonStyle = "btn-primary";
            }

            ListeningPortsViewModel lpvm = new ListeningPortsViewModel();
            lpvm.PortsForListening = appModel.GetPortsForListening();
            lpvm.InProgress = appModel.isInProgress();
            List<int> busyPorts = validators.DetectSystemBusyPorts();
            string busyPortsString = String.Join(", ", busyPorts);
            ViewBag.busyPortsString = busyPortsString;
            return View(lpvm);
        }
        [HttpPost]
        public async Task<IActionResult> StartListening(List<int> ports)
        {
            var tasks = new List<Task>();
            bool listeningNotStarted = !appModel.isInProgress();
            if (listeningNotStarted) //Якщо прослуховування ще не запущено, готуємось до запуску
            {
                List<int> busyPorts = validators.DetectSystemBusyPorts(); //визначаємо порти які вже зайняті системою
                IEnumerable<int> commonPorts = ports.Intersect(busyPorts);
                if (!commonPorts.Any()) //Якщо серед введених немає "зайнятих" працюєм далі
                {
                    appModel.SetPortsForListening(ports); //зберігаєм обрані порти
                    try
                    {
                        foreach (var port in ports) //на кожен порт по лістенеру
                        {
                            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
                            tcpListener.Start();
                            Console.WriteLine($"{port} opened for listening ");

                            var task = Task.Run(async () => //на кожен лістенер окрему "задачу"
                            {
                                try
                                {
                                    while (true)
                                    {
                                        using (TcpClient client = tcpListener.AcceptTcpClient()) //"ловимо" вхідні запити
                                        {
                                            IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                                            string msg = $"Somebody connected from {clientEndPoint.Address}:{clientEndPoint.Port} on port {port}";
                                            Console.WriteLine(msg);
                                            messageViewModel.AddMessage(msg);
                                            string totalMessage = messageViewModel.GetMessage();
                                            await hubContext.Clients.All.SendAsync("ReceiveMessage", totalMessage);
                                        }
                                        // Logging and reactions here
                                    }
                                }
                                catch (SocketException ex) when (ex.ErrorCode == 10004) //лістенер не спрацював на закритому порті
                                {
                                    Console.WriteLine($"Port {port} was closed.");
                                }
                                catch (Exception ex) //щось пішло не так
                                {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            });
                            tasks.Add(task);
                            appModel.AddListener(tcpListener);
                        }
                    }
                    catch (Exception ex) //щось не так
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    appModel.turnOn(); //запуск прослуховування пройшов без проблем. Фіксуєм статус "в роботі"
                }
                else //серед введених портів є "зайнятий". Сповіщаєм користувача
                {
                    string busyPortsStringMsg = String.Join(", ", commonPorts);
                    TempData["ErrorMessage"] = $"Серед вибраних портів, є такі, які зайняті системою:{busyPortsStringMsg}";
                }
            }
            else //Якщо процес запущено, зупиняєм його
            {
                List<TcpListener> tcpListeners = appModel.GetTcpListeners();
                foreach (var tcpListener in tcpListeners)
                {
                    tcpListener.Stop();
                }
                await Task.WhenAll(tasks);
                appModel.turnOff();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult BlockMaliciousIP()
        {
            BlockIP("192.168.1.100");
            return RedirectToAction("Index");
        }
        public async void BlockIP(string ipAddress)
        {
            // Створення PowerShell команди для додавання правила брандмауера
            string blockCommand = $"New-NetFirewallRule -DisplayName 'BlockInboundTrafficFromIP' -Direction Inbound -Action Block -RemoteAddress {ipAddress}";

            // Запуск PowerShell команди
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript(blockCommand);
                ps.Invoke();
                string msg = $"{ipAddress} was blocked";
                msg = messageViewModel.AddMessage(msg);
                await hubContext.Clients.All.SendAsync("ReceiveMessage", msg);
            }
        }
    }

    
}
