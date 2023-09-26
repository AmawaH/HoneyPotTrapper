using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace HoneyPotTrapper.Validations
{
    public interface ISystemBusyPortsDetector
    {
        List<int> Detect(); 
    }

    public class SystemBusyPortsDetector : ISystemBusyPortsDetector
    {
        public List<int> Detect()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] endPoints = properties.GetActiveTcpListeners();
            List<int> busyPortsList = new List<int>();
            foreach (var endPoint in endPoints)
            {
                busyPortsList.Add(endPoint.Port);
            }
            busyPortsList = busyPortsList.Distinct().OrderBy(port => port).ToList<int>();
            return busyPortsList;
        }
    }
}
