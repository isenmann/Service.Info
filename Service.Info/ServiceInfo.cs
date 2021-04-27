using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Service.Info
{
    public class ServiceInfo
    {
        public List<Service> Services { get; private set; } = new List<Service>();

        private readonly IServiceInfo _serviceInfo = null;

        public ServiceInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) // Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                _serviceInfo = new Windows.ServiceInfo();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) // Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                _serviceInfo = new Mac.ServiceInfo();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) // Environment.OSVersion.Platform == PlatformID.Unix)
            {
                _serviceInfo = new Linux.ServiceInfo();
            }
        }

        public void RefreshAll()
        {
            RefreshServiceList();
        }

        public void RefreshServiceList() => Services = _serviceInfo.GetServiceList();

        public void SetServiceAction(string serviceName, ServiceAction action) =>
            _serviceInfo.SetServiceAction(serviceName, action);
    }
}
