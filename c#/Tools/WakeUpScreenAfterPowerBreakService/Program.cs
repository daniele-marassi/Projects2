
using System.ServiceProcess;

namespace Tools.WakeUpScreenAfterPowerBreak
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
                new WakeUpScreenAfterPowerBreakService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
