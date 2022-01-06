using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;

namespace ChoixResto.Models
{
	public class BddContext : DbContext
	{
        public BddContext(): base()
        {

        }

        //public IConfiguration Configuration { get; }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
		public DbSet<Resto> Restos { get; set; }
		public DbSet<Vote> Votes { get; set; }
		public DbSet<Sondage> Sondages { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            if (System.Diagnostics.Debugger.IsAttached)
            {
                optionsBuilder.UseMySql("server=localhost;user id=root;password=rrrrr;database=ChoixSejourDebug");
            }
            else
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
            }
                
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // specification on configuration

            //Declare non nullable columns
            modelBuilder.Entity<Utilisateur>().Property(u => u.Prenom).IsRequired();
            //Add uniqueness constraint
            modelBuilder.Entity<Utilisateur>().HasIndex(u => u.Prenom).IsUnique();
        }

        public void InitializeDb()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
            this.Restos.AddRange(
                new Resto
                {
                    Id = 1,
                    Nom = "Au Cellier",
                    Telephone = "0299972050",
                    Ville = "Fougères",
                    Description = "bla bla bla" 
                },
                new Resto
                {
                    Id = 2,
                    Nom = "L'Eveil des sens",
                    Telephone = "0243304217",
                    Ville = "Mayenne",
                    Description = "bla bla bla"
                },
                new Resto
                {
                    Id = 3,
                    Nom = "Le Carré",
                    Telephone = "0223402121",
                    Ville = "Rennes",
                    Description = "bla bla bla"
                },
                 new Resto
                 {
                     Id = 4,
                     Nom = "Le Carré",
                     Telephone = "0223402121",
                     Ville = "Rennes",
                     Description = "bla bla bla"
                 },
                 new Resto
                 {
                     Id = 5,
                     Nom = "Le Carré",
                     Telephone = "0223402121",
                     Ville = "Rennes",
                     Description = "bla bla bla"
                 }
            );
            this.Utilisateurs.Add(new Utilisateur { Id = 1, Prenom = "Pierre", Password = "BC-C2-8A-15-B2-66-C8-3C-D4-E2-31-7D-17-16-58-A8", Role="Basic" });
            this.Utilisateurs.Add(new Utilisateur { Id = 2, Prenom = "Louis", Password = "FB-32-9E-B0-0E-A1-D6-76-5D-D1-3B-8E-C0-26-3C-CB", Role = "Admin" });
            this.Sondages.Add(new Sondage { Id = 1});
            this.SaveChanges();
        }
    }
}
