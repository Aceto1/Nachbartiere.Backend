using Microsoft.Extensions.Configuration;
using Nachbartiere.Backend.Database;
using Nachbartiere.Backend.Database.Entities;
using System.Text;

namespace Nachbartiere.Backend.TokenGenerator
{
    internal class Program
    {
        private static Random random = new Random();

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void Main(string[] args)
        {
            Console.WriteLine(@" _____       _                      ___                             _             ");
            Console.WriteLine(@"|_   _| ___ | |__ ___  _ _         / __| ___  _ _   ___  _ _  __ _ | |_  ___  _ _ ");
            Console.WriteLine(@"  | |  / _ \| / // -_)| ' \\      | (_ |/ -_)| ' \ / -_)| '_|/ _` ||  _|/ _ \| '_|");
            Console.WriteLine(@"  |_|  \___/|_\_\\___||_||_|       \___|\___||_||_|\___||_|  \__/_| \__|\___/|_|  ");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            if (args.Length != 1 || !Int32.TryParse(args[0], out var count))
            {
                Console.WriteLine("Falsches Argument.");
                Console.WriteLine("Benutzung: Nachbartiere.Backend.TokenGenerator.exe {Anzahl}");
                Console.WriteLine("Beispiel: Nachbartiere.Backend.TokenGenerator.exe 12");
                Console.WriteLine();
                Console.WriteLine("Beliebige Taste zum Beenden drücken...");
                Console.ReadKey();
                Environment.Exit(1);
                return;
            }

            // Init random number generator
            for (int i = 0; i < 100; i++)
            {
                random.Next();
            }

            // Init DB context
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");

            var config = configuration.Build();
            var connectionString = config.GetConnectionString("Default");

            DatabaseContext.ConnectionString = connectionString;
            var ctx = new DatabaseContext();

            // Token generieren
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                string newToken;
                do
                {
                    newToken = GetRandomString(6);
                }
                while (ctx.InviteTokens.Any(m => m.Token == newToken));

                var tokenEntity = new InviteToken()
                {
                    Token = newToken
                };

                ctx.InviteTokens.Add(tokenEntity);
                sb.AppendLine(newToken);
            }

            ctx.SaveChanges();
            Console.WriteLine("Deine generierten Token:");
            Console.WriteLine(sb.ToString());
            Console.WriteLine();

            File.WriteAllText("token.txt", sb.ToString());
            Console.WriteLine("Token sind gespeichert.");

            Console.WriteLine("Beliebige Taste zum Beenden drücken...");
            Console.ReadKey();
        }
    }
}