using System.ServiceProcess;

namespace Tools.CheckSupp
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
                new CheckSuppService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
