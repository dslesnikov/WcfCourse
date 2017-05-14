using System;
using System.ServiceModel;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(LibraryService));
            host.Open();

            Console.WriteLine($"Server is running at {host.Description.Endpoints[0].Address}");
            Console.WriteLine("Press enter to shutdown server");
            Console.ReadLine();
            host.Close();
        }
    }
}