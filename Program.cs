using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OracleCRUDApp
{
    public class OracleDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle("User Id=system;Password=aayush07102004;Data Source=localhost:1521/XE");
        }

        public DbSet<Sample> Samples { get; set; }
    }

    public class Sample
    {
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add record");
                Console.WriteLine("2. Remove record");
                Console.WriteLine("3. Show records");
                Console.WriteLine("4. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddRecord();
                        break;
                    case "2":
                        RemoveRecord();
                        break;
                    case "3":
                        ShowRecords();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void AddRecord()
        {
            using (var context = new OracleDbContext())
            {
                Console.WriteLine("Enter message:");
                var message = Console.ReadLine();

                context.Samples.Add(new Sample { Message = message });
                context.SaveChanges();

                Console.WriteLine("Record added successfully.");
            }
        }

        static void RemoveRecord()
        {
            using (var context = new OracleDbContext())
            {
                Console.WriteLine("Enter ID of the record to remove:");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    var record = context.Samples.FirstOrDefault(s => s.Id == id);
                    if (record != null)
                    {
                        context.Samples.Remove(record);
                        context.SaveChanges();
                        Console.WriteLine("Record removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Record with the specified ID not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid ID.");
                }
            }
        }

        static void ShowRecords()
        {
            using (var context = new OracleDbContext())
            {
                var records = context.Samples.ToList();
                if (records.Any())
                {
                    Console.WriteLine("ID\tMessage");
                    foreach (var record in records)
                    {
                        Console.WriteLine($"{record.Id}\t{record.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                }
            }
        }
    }
}
