using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace HoneyPotTrapper.Models
{
    public interface IPortsForListeningCollection
    {
        List<int> GetPortsForListening();
        bool isInProgress();
        void turnOn();
        void turnOff();
        void AddListener(TcpListener listener);
        List<TcpListener> GetTcpListeners();
        void AddThread(Thread thread);
        List<Thread> GetThreads();
    }
    public class PortsForListeningCollection : IPortsForListeningCollection
    {
        private List<TcpListener> TcpListeners { get; set; }
        private List<Thread> Threads { get; set; }
        private bool InProgress { get; set; }
        private List<int> PortsForListening { get; set; }
        public PortsForListeningCollection()
        {
            PortsForListening = new List<int> { 28, 248, 418, 481, 708 };
            InProgress = false;
            TcpListeners = new List<TcpListener>();
            Threads = new List<Thread>();

    }
        public PortsForListeningCollection(List<int> _PortsForListening, bool _inPogress)
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

        public void AddThread(Thread thread)
        {
            Threads.Add(thread);
        }
        public List<Thread> GetThreads()
        {
            return Threads;
        }

        public List<int> GetPortsForListening()
        {
            return PortsForListening;
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
