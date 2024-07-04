using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer.Core.Services;

public interface IBackendServerCommunicator
{
    string Host { get; }
    int Port { get; }
    bool IsHealthy { get; set; }
    Task<bool> CheckHealthAsync();
}