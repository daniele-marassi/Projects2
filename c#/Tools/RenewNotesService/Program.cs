
using System.ServiceProcess;

namespace Tools.RenewNotes
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
                new RenewNotesService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
