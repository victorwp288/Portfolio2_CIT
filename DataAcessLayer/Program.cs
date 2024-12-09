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
using DataAcessLayer.Entities.Movies;
using System.Threading.Tasks; 


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
            RunNameBasicRetrievalAsync().Wait();
            Console.WriteLine("Program finished.");
            /*var item = ds.GetUserById(1);
            if (item == null)
            {
                Console.WriteLine("User Not Found");
            }
            else
            {
                Console.WriteLine(item.Username);

            }
            if (ds.FunctionLoginUser("testuser1", "password123"))
            {
                Console.WriteLine("Login successful!");
            }
            else
            {
                Console.WriteLine("Login failed.");
            }
            Console.WriteLine(ds.DeleteUser(59));*/
            //Console.WriteLine(ds.UpdateUserEmail(id, "user6@example.com"));
            //Console.WriteLine(ds.LoginUser("testuser20", "password123"));
            //Console.WriteLine(ds.DeleteUser(59));

            //Console.WriteLine(ds.LoginUser("testuser20", "password123"));
            /*string[] keywords = { "aaa", "bbb", "ccc" };
            int resultLimit = 10;
            string sql = $"SELECT * from word_to_words_query({resultLimit}";
            foreach (string keyword in keywords)
            {
                sql = sql + ",'" + keyword + "'";
            }
            sql = sql + ")";

            Console.WriteLine(sql);*/

            var outPut = ds.GetNameBasicByNconst("nm0000129");

            /*var pkt = ds.GetPersonKnownTitlesByNconst("nm0000129");
            //var pp = ds.GetPersonProfessionsByNconst("nm0000129");
            //outPut.PersonProfessions=pp;
            //outPut.PersonKnownTitles=pkt;
            Console.WriteLine(pkt.Count());
            Console.WriteLine(pkt.First().Tconst);
            Console.WriteLine(pp.Count());
            Console.WriteLine(pp.First().Profession);

            Console.WriteLine(outPut.PrimaryName);
            Console.WriteLine(outPut.PersonProfessions.Count());
            Console.WriteLine(outPut.PersonKnownTitles.Count());
            Console.WriteLine(outPut.TitlePrincipals.Count());
            Console.WriteLine(outPut.BirthYear);
            Console.WriteLine(outPut.NameRatings.WeightedRating);
            Console.WriteLine(outPut.TitlePrincipals.First().Tconst);
            Console.WriteLine(outPut.PersonProfessions.First().Profession);
            Console.WriteLine(outPut.PersonKnownTitles.First().Tconst);


            /*
            var outPut2 = ds.GetTitleBasic("tt21880152");
            Console.WriteLine(outPut2.PrimaryTitle);
            Console.WriteLine(outPut2.TitleRating.AverageRating);
            Console.WriteLine(outPut2.PersonKnownTitles.First().Nconst);
            Console.WriteLine(outPut2.MovieGenres.First().Genre);
            Console.WriteLine(ds.GetNumberOfTitleBasics());*/


        }
        static async Task RunNameBasicRetrievalAsync()
        {
            try
            {
                var context = new ImdbContext();
                // Your program logic goes here 
                Console.WriteLine("Hello, world!");
                var ds = new DataService(context);
                // Call your async data access method
                var outPut = await ds.GetNameBasicByNconst("nm0000129");
                //Process Results
                if (outPut != null)
                {
                    Console.WriteLine($"Primary Name: {outPut.PrimaryName}");
                    // ... process other properties ...
                    //Console.WriteLine(outPut.PrimaryName);
                    Console.WriteLine(outPut.PersonProfessions.Count());
                    Console.WriteLine(outPut.PersonKnownTitles.Count());
                    Console.WriteLine(outPut.TitlePrincipals.Count());
                    Console.WriteLine(outPut.BirthYear);
                    Console.WriteLine(outPut.NameRatings);
                    Console.WriteLine(outPut.NameRatings.WeightedRating);
                    Console.WriteLine(outPut.TitlePrincipals.First().Tconst);
                    Console.WriteLine(outPut.TitlePrincipals.First().Tconst.Count());
                    Console.WriteLine(outPut.PersonProfessions.First().Profession);
                    Console.WriteLine(outPut.PersonKnownTitles.First().Tconst);
                }
                else
                {
                    Console.WriteLine("NameBasic not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                //Handle exception appropriately
            }
        }

    }
}









