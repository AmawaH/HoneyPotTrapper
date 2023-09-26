using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;
using HoneyPotTrapper.Models;
using System.Collections.Generic;
using System;
using System.Threading;
using HoneyPotTrapper.ViewModels;

namespace HoneyPotTrapper.Controllers
{
    public class ListeningPortsController : Controller
    {
        public IPortsForListeningCollection portsForListeningCollection;
        public ListeningPortsController( IPortsForListeningCollection _portsForListeningCollection)
        {
            portsForListeningCollection = _portsForListeningCollection;
        }
        public ActionResult Index()
        {
            ListeningPortsViewModel lpvm = new ListeningPortsViewModel();
            lpvm.InProgress = portsForListeningCollection.isInProgress();
            lpvm.PortsForListening = portsForListeningCollection.GetPortsForListening();

            return View(lpvm);
        }
        [HttpPost]
        public ActionResult StartListening(List<int> ports)
        {
            bool b = !portsForListeningCollection.isInProgress();
            if (b)
            {
                try
                {
                    foreach (var port in ports)
                    {
                        TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
                        tcpListener.Start();
                        Console.WriteLine($"{port} opened for listening ");


                        Thread thread = new Thread(() =>
                        {
                            while (true)
                            {
                                using (TcpClient client = tcpListener.AcceptTcpClient())
                                {
                                    IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                                    Console.WriteLine($"Somebody connected from {clientEndPoint.Address}:{clientEndPoint.Port} on port {port}");
                                }
                                // Logging and reactions here
                            }
                        });

                        thread.Start();
                        portsForListeningCollection.AddThread(thread);
                        portsForListeningCollection.AddListener(tcpListener);
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error: " + ex.Message;
                }
                portsForListeningCollection.turnOn();
            }
            else
            {
                List<Thread> threads = portsForListeningCollection.GetThreads();
                List<TcpListener> tcpListeners = portsForListeningCollection.GetTcpListeners();
                foreach (var thread in threads)
                {
                    thread.Abort();
                }
                foreach (var tcpListener in tcpListeners)
                {
                    tcpListener.Stop();
                }
                portsForListeningCollection.turnOff();
            }
            return RedirectToAction("Index");
        }
    }
    
}
