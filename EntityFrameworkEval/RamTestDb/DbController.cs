using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RamTestDb
{
    public class DbController
    {
        DbContextOptions<MyDbContext> _options;
        private MyDbContext _context;

        public DbController()
        {
            _options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _context = new MyDbContext(_options);

        }

        public IEnumerable<MyModel> GetAll()
        {
            using (var context = new MyDbContext(_options))
            {
                IEnumerable<MyModel> list = context.Entities;
                return list.ToList();
            }
        }

        public MyModel InsertItem(MyModel model)
        {
            using (var context = new MyDbContext(_options))
            {
                context.Entities.Add(model);
                context.SaveChanges();
            }
            return model;
        }

        public void RemoveItem(MyModel model)
        {
            using (var context = new MyDbContext(_options))
            {
                context.Entities.Remove(model);
                context.SaveChanges();
            }
        }
    }
}
