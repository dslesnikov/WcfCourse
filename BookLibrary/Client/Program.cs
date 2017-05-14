using System;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using Client.LibraryServiceReference;

namespace Client
{
    public class OnSaveChangesCallback : ILibraryServiceCallback
    {
        public void OnSaveChanges() { Console.WriteLine("Ok"); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var instanceContext = new InstanceContext(new OnSaveChangesCallback());
            using (var client = new LibraryServiceClient(instanceContext))
            {
                client.EnterLibrary(1, "My name");
                var book = client.GetBook(1);
                Console.WriteLine($"First book's author: {book.Author}");
                client.TakeBook(1);
                client.SaveChanges();
                Thread.Sleep(200);
                client.Leave();
            }

            using (var client = new LibraryServiceClient(instanceContext))
            {
                client.EnterLibrary(1, "My name");
                var book = client.GetBook(1);
                Console.WriteLine($"Book 1 is taken? - {book.Taken}");
                client.ReturnBook(1);
                try
                {
                    client.ReturnBook(2);
                }
                catch (FaultException e)
                {
                    Console.WriteLine(e.Message);
                }
                client.SaveChanges();
                Thread.Sleep(200);
                client.Leave();
            }

            using (var client = new LibraryServiceClient(instanceContext))
            {
                client.EnterLibrary(1, "My name");
                var boks = client.GetBooks("The Univalent Foundations Program");
                boks.ToList().ForEach(b => Console.WriteLine(b.Name));
                client.Leave();
            }
        }
    }
}
