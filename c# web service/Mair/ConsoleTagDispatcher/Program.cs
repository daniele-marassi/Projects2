using Mair.DigitalSuite.TagDispatcher.Servicies;
using System;
using System.Threading.Tasks;

namespace ConsoleTagDispatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var plcConnector = new PlcConnector(@"Data Source=localhost;Initial Catalog=Mair.DigitalSuite.Database; user id=admin;password=Password.00"))
            {
                var result = plcConnector.GetPlcData("Driver1", "Connection1");

                var data = result.Data;

                foreach (var item in data)
                {
                    Console.WriteLine($"Driver: {item.Driver}, ConnectionString: {item.ConnectionString}, TagNumber: {item.TagNumber}, TagValue: {item.TagValue}");
                }

                Console.WriteLine("############################################");
                Console.WriteLine("############################################");


                plcConnector.UpdatePlcData("Driver1", "Connection1", 1, "xxxx");


                result = plcConnector.GetPlcData("Driver1", "Connection1");

                data = result.Data;

                foreach (var item in data)
                {
                    Console.WriteLine($"Driver: {item.Driver}, ConnectionString: {item.ConnectionString}, TagNumber: {item.TagNumber}, TagValue: {item.TagValue}");
                }
            }

            Console.ReadLine();
        }
    }
}
