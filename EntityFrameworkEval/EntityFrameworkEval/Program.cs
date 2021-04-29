using RamTestDb;
using System;
using System.Linq;

namespace EntityFrameworkEval
{
    class Program
    {
        private static DbController _db;

        static void Main(string[] args)
        {
            _db = new DbController(); // Note: EntityFrameworkCore must somehow be referenced in this Project to use RamTestDb correctly 
            ListEntries();

            Console.WriteLine("## Add some entries");

            AddEntry(new MyModel() { SomeEntry = "item1", OtherEntry = "someStuff 1" });
            AddEntry(new MyModel() { SomeEntry = "item2", OtherEntry = "someStuff 2" });
            AddEntry(new MyModel() { SomeEntry = "item3", OtherEntry = "someStuff 3" });

            ListEntries();

            Console.WriteLine("## Remove entry");
            var list = _db.GetAll().ToList();
            RemoveEntry(list[1]);

            ListEntries();

            Console.ReadLine();
        }

        private static void RemoveEntry(MyModel entry)
        {
            Console.WriteLine($"    - {entry.Id} {entry.SomeEntry}/{entry.OtherEntry}");
            _db.RemoveItem(entry);
        }

        private static void AddEntry(MyModel entry)
        {
            Console.WriteLine($"    + {entry.SomeEntry}/{entry.OtherEntry}");
            _db.InsertItem(entry);
        }

        private static void ListEntries()
        {
            Console.WriteLine("## List entries");
            var entries = _db.GetAll();
            if (!entries.Any())
                Console.WriteLine(" -- Nothing to List");

            foreach (var entriy in entries)
            {
                Console.WriteLine(entriy.ListEntry());
            }
        }


    }
}
