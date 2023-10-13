using System.Collections.Generic;

namespace HoneyPotTrapper.Models.ViewModels
{
    public class ListeningPortsViewModel
    {
        public bool InProgress { get; set; }
        public List<int> PortsForListening { get; set; }
    }
}
