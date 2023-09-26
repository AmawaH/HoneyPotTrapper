using System.Collections.Generic;

namespace HoneyPotTrapper.Validations
{
    public interface IValidators
    {
        List<int> DetectSystemBusyPorts();
    }
    public class Validators : IValidators
    {
        private ISystemBusyPortsDetector systemBusyPortsDetector;
        public Validators(ISystemBusyPortsDetector _systemBusyPortsDetector)
        {
            systemBusyPortsDetector = _systemBusyPortsDetector;
        }
        public List<int> DetectSystemBusyPorts()
        {
            List<int> systemBusyPorts = new List<int>();
            systemBusyPorts = systemBusyPortsDetector.Detect();
            return systemBusyPorts;
        }

    }
    
}
