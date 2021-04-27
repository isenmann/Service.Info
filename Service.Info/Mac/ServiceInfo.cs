using System.Collections.Generic;

namespace Service.Info.Mac
{
    internal class ServiceInfo : ServiceInfoBase, IServiceInfo
    {
        public List<Service> GetServiceList()
        {
            return new List<Service>();
        }

        public Service GetService(string serviceName)
        {
            return null;
        }

        public void SetServiceAction(string serviceName, ServiceAction action)
        {
            
        }
    }
}
