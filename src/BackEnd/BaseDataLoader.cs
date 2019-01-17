using BackEnd.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BackEnd
{
    public abstract class BaseDataLoader
    {
        private readonly IServiceProvider _services;

        protected BaseDataLoader(IServiceProvider services)
        {
            _services = services;
        }

        protected string Filename { get; private set; }

        protected Conference Conference { get; private set; }

        protected bool SaveData { get; set; } = true;

        public void LoadData(string filename, string conferenceName)
        {
            Filename = filename;

            using (var scope = _services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                // Conference
                Conference = new Conference { Name = conferenceName };
                db.Conferences.Add(Conference);

                LoadFormattedData(db);

                if (SaveData)
                {
                    db.SaveChanges();
                }
            }
        }

        protected abstract void LoadFormattedData(ApplicationDbContext db);
    }
}
