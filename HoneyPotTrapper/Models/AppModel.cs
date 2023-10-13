using System.Collections.Generic;
using System.Net.Sockets;

namespace HoneyPotTrapper.Models
{
    public interface IAppModel
    {
        List<int> GetPortsForListening();
        void SetPortsForListening(List<int> ports);
        bool isInProgress();
        void turnOn();
        void turnOff();
        void AddListener(TcpListener listener);
        List<TcpListener> GetTcpListeners();
    }
    public class AppModel : IAppModel
    {
        private List<TcpListener> TcpListeners { get; set; }
        private bool InProgress { get; set; }
        private List<int> PortsForListening { get; set; } = new List<int> { 28, 248, 418, 481, 708 };
        public AppModel()
        {
            InProgress = false;
            TcpListeners = new List<TcpListener>();

    }
        public AppModel(List<int> _PortsForListening, bool _inPogress)
        {
            PortsForListening = _PortsForListening;
            InProgress = _inPogress;
        }
        public void AddListener(TcpListener listener)
        {
            TcpListeners.Add(listener);
        }
        public List<TcpListener> GetTcpListeners()
        {
            return TcpListeners;
        }
        public List<int> GetPortsForListening()
        {
            return PortsForListening;
        }
        public void SetPortsForListening(List<int> ports)
        {
            PortsForListening = ports;
        }

        public bool isInProgress()
        {
            return InProgress;
        }

        public void turnOn()
        {
            InProgress = true;
        }

        public void turnOff()
        {
            InProgress = false;
        }
    }
}
