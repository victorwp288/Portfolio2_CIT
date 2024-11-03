using DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entities.Functions;
using DataAcessLayer.Repositories.Interfaces;
using Npgsql;
using DataAcessLayer.Context;
using Microsoft.EntityFrameworkCore;
using DataAcessLayer.Entities.Users;
using DataAcessLayer.Repositories.Implementations;

namespace DataAcessLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new ImdbContext();
            // Your program logic goes here 
            Console.WriteLine("Hello, world!");
            var ds = new DataService(context);
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









