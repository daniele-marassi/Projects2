
using System.ServiceProcess;

namespace Tools.SyncIp
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SyncIpService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
