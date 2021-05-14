using Mair.DS.Data.Context;
using Mair.DS.Data.Repositories;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mair.DS.Adapters.DataObjects
{
    public class Connection
        //where T: AutomationContext, EasyUAClient
    {
        public SimulatedConnectorRepository DBSimulatedConnection { get; set; }

        public Session OPCUASTDConnection { get; set; }

        //public T conn { get; set; }

        //QUI SARANNO INSERITE TUTTI GLI ALTRI TIPI DI CONNESSIONI, COME QUELLA DELL'OPC-UA,
        //OPC-DA, S7, ...

        //public OPCUAConnection
        //public S7Connection

        public void GetActiveConnection()
        {

        }

    }
}
