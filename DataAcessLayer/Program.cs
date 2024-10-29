using DataAcessLayer;
using System;

namespace DataAcessLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Your program logic goes here 
            Console.WriteLine("Hello, world!");
            var ds = new DataService();
            /*var item = ds.GetUserById(1);
            if (item == null)
            {
                Console.WriteLine("User Not Found");
            }
            else
            {
                Console.WriteLine(item.Username);

            }*/
            if (ds.LoginUser("testuser1", "password123"))
            {
                Console.WriteLine("Login successful!");
            }
            else
            {
                Console.WriteLine("Login failed.");
            }


        }
        
    }
}









