using System;
using System.Collections.Generic;
using System.Management;

namespace Service.Info.Windows
{
    internal class ServiceInfo : ServiceInfoBase, IServiceInfo
    {
        public static T GetPropertyValue<T>(object obj) where T : struct
        {
            return (obj == null) ? default(T) : (T)obj;
        }

        public static T[] GetPropertyArray<T>(object obj)
        {
            return (obj is T[] array) ? array : Array.Empty<T>();
        }

        public static string GetPropertyString(object obj)
        {
            return (obj is string str) ? str : string.Empty;
        }
        
        public List<Service> GetServiceList()
        {
            var services = new List<Service>();
            uint numberOfLogicalProcessors = 1;

            using (var win32ComputerSystem = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
            {
                foreach (var queryObject in win32ComputerSystem.Get())
                {
                    numberOfLogicalProcessors = GetPropertyValue<uint>(queryObject["NumberOfLogicalProcessors"]);
                }
            }

            using (var win32Service = new ManagementObjectSearcher("SELECT * FROM Win32_Service"))
            {
                foreach (var queryObject in win32Service.Get())
                {
                    var service = new Service
                    {
                        Name = GetPropertyString(queryObject["Name"]),
                        ProcessId = GetPropertyValue<uint>(queryObject["ProcessId"])
                    };
                    var state = GetPropertyString(queryObject["State"]);
                    switch (state)
                    {
                        case "Stopped":
                            service.State = ServiceState.Stopped;
                            break;
                        case "Start Pending":
                            service.State = ServiceState.StartPending;
                            break;
                        case "Stop Pending":
                            service.State = ServiceState.StopPending;
                            break;
                        case "Running":
                            service.State = ServiceState.Running;
                            break;
                        case "Continue Pending":
                            service.State = ServiceState.ContinuePending;
                            break;
                        case "Pause Pending":
                            service.State = ServiceState.PausePending;
                            break;
                        case "Paused":
                            service.State = ServiceState.Paused;
                            break;
                        case "Unknown":
                            service.State = ServiceState.Unknown;
                            break;
                        default:
                            service.State = service.State;
                            break;
                    }

                    if (service.State == ServiceState.Running)
                    {
                        using (var win32PerfFormattedDataPerfProcProcess = new ManagementObjectSearcher(
                            $"SELECT * FROM Win32_PerfFormattedData_PerfProc_Process WHERE IDProcess = {service.ProcessId}")
                        )
                        {
                            foreach (var queryObj in win32PerfFormattedDataPerfProcProcess.Get())
                            {
                                service.CpuUsage = GetPropertyValue<ulong>(queryObj["PercentProcessorTime"]) / numberOfLogicalProcessors;
                                service.MemoryPrivateBytes = GetPropertyValue<ulong>(queryObj["PrivateBytes"]);
                                service.MemoryWorkingSet = GetPropertyValue<ulong>(queryObj["WorkingSet"]);
                            }
                        }
                    }

                    services.Add(service);
                }
            }

            return services;
        }

        public Service GetService(string serviceName)
        {
            uint numberOfLogicalProcessors = 1;

            using (var win32ComputerSystem = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem"))
            {
                foreach (var queryObject in win32ComputerSystem.Get())
                {
                    numberOfLogicalProcessors = GetPropertyValue<uint>(queryObject["NumberOfLogicalProcessors"]);
                }
            }

            using (var win32Service = new ManagementObjectSearcher($"SELECT * FROM Win32_Service WHERE Name = '{serviceName}'"))
            {
                foreach (var queryObject in win32Service.Get())
                {
                    var service = new Service
                    {
                        Name = GetPropertyString(queryObject["Name"]),
                        ProcessId = GetPropertyValue<uint>(queryObject["ProcessId"])
                    };
                    var state = GetPropertyString(queryObject["State"]);
                    switch (state)
                    {
                        case "Stopped":
                            service.State = ServiceState.Stopped;
                            break;
                        case "Start Pending":
                            service.State = ServiceState.StartPending;
                            break;
                        case "Stop Pending":
                            service.State = ServiceState.StopPending;
                            break;
                        case "Running":
                            service.State = ServiceState.Running;
                            break;
                        case "Continue Pending":
                            service.State = ServiceState.ContinuePending;
                            break;
                        case "Pause Pending":
                            service.State = ServiceState.PausePending;
                            break;
                        case "Paused":
                            service.State = ServiceState.Paused;
                            break;
                        case "Unknown":
                            service.State = ServiceState.Unknown;
                            break;
                        default:
                            service.State = service.State;
                            break;
                    }

                    if (service.State == ServiceState.Running)
                    {
                        using (var win32PerfFormattedDataPerfProcProcess = new ManagementObjectSearcher(
                            $"SELECT * FROM Win32_PerfFormattedData_PerfProc_Process WHERE IDProcess = {service.ProcessId}")
                        )
                        {
                            foreach (var queryObj in win32PerfFormattedDataPerfProcProcess.Get())
                            {
                                service.CpuUsage = GetPropertyValue<ulong>(queryObj["PercentProcessorTime"]) / numberOfLogicalProcessors;
                                service.MemoryPrivateBytes = GetPropertyValue<ulong>(queryObj["PrivateBytes"]);
                                service.MemoryWorkingSet = GetPropertyValue<ulong>(queryObj["WorkingSet"]);
                            }
                        }
                    }

                    return service;
                }
            }

            return null;
        }

        public void SetServiceAction(string serviceName, ServiceAction action)
        {
            using (var classInstance = new ManagementObject($"Win32_Service.Name = '{serviceName}'", null))
            {
                switch (action)
                {
                    case ServiceAction.Stop:
                        classInstance.InvokeMethod("StopService", null, null);
                        break;
                    case ServiceAction.Start:
                        classInstance.InvokeMethod("StartService", null, null);
                        break;
                    case ServiceAction.Restart:
                        classInstance.InvokeMethod("StopService", null, null);
                        classInstance.InvokeMethod("StartService", null, null);
                        break;
                }
            }
        }
    }
}
