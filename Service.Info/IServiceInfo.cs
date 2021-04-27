using System.Collections.Generic;

namespace Service.Info
{
    internal interface IServiceInfo
    {
        List<Service> GetServiceList();
        Service GetService(string serviceName);
        void SetServiceAction(string serviceName, ServiceAction action);
    }
}
