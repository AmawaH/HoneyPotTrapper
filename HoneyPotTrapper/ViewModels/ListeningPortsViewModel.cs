using System.Collections.Generic;

namespace HoneyPotTrapper.ViewModels
{
    public class ListeningPortsViewModel
    {
        public bool InProgress { get; set; }
        public List<int> PortsForListening { get; set; }
    }
}
