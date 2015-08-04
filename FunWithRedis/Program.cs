using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Redis;

namespace FunWithRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (IRedisNativeClient client = new RedisClient())
            //{

            //    client.Set("urn:messages:1", Encoding.UTF8.GetBytes("Hello C# World"));
            //}

            //using (IRedisNativeClient client = new RedisClient())
            //{

            //   var result = Encoding.UTF8.GetString(client.Get("urn:messages:1"));
            //   Console.WriteLine("Message: {0}", result);
            //}

            //using (IRedisClient client = new RedisClient())
            //{
            //    var customerNames = client.Lists["urncustomerNames"];
            //    customerNames.Clear();
            //    customerNames.Add("Joe");
            //    customerNames.Add("Mary");
            //    customerNames.Add("Bob");
            //}

            //using (IRedisClient client = new RedisClient())
            //{
            //    var customerNames = client.Lists["urn:customernames"];
            //    foreach (var customerName in customerNames)
            //    {
            //        Console.WriteLine("Customer: {0}", customerNames);
            //    }
            //}

            long lastId = 0;
            using (IRedisClient client = new RedisClient())
            {
                var customerClient = client.As<Customer>();
                var customer = new Customer()
                {
                    Id = customerClient.GetNextSequence(),
                    Address = "123 Main Street",
                    Name = "Bob Tabor",
                    Orders = new List<Order>
                    {
                        new Order {OrderNumber = "ABC"},
                        new Order {OrderNumber = "AB13243"}
                    }

                };
                var storedCustomer = customerClient.Store(customer);
                lastId = storedCustomer.Id;
            }

            using (IRedisClient client = new RedisClient())
            {
                var customerClient = client.As<Customer>();
                var customer = customerClient.GetById(lastId);
                Console.WriteLine("Got Customer {0}, with name {1}", customer.Id, customer.Name);
            }

            using (IRedisClient client = new RedisClient())
            {

                client.PublishMessage("debug", "Hello C#");
            }


            Console.ReadLine();
        }
    }

    public class Customer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Order> Orders { get; set; } 
    }

    public class Order
    {
        public string OrderNumber { get; set; }
    }
}
