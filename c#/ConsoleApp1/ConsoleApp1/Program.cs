using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var accessToken = FacebookSettings.AccessToken;
            var facebookClient = new FacebookClient(accessToken);
            var facebookService = new FacebookService(facebookClient);
            var getAccountTask = facebookService.GetAccountAsync(accessToken);
            var getTask = facebookService.GetAccountAsync(accessToken);


            Task.WaitAll(getAccountTask);
            var account = getAccountTask.Result;
            Console.WriteLine( $"{account.Id} {account.Name}");
            
            var postOnWallTask = facebookService.PostOnWallAsync(accessToken, "Ciao");
            Task.WaitAll(postOnWallTask);
            Console.ReadLine();

           











        }



    }
}
