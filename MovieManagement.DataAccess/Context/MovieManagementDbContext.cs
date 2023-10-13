using Microsoft.EntityFrameworkCore;
using MovieManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement.DataAccess.Context
{
    public class MovieManagementDbContext : DbContext
    {
        public MovieManagementDbContext(DbContextOptions<MovieManagementDbContext> options) : base(options) { }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Biography> Biographies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>().HasData(
                    new Actor { Id = 1, FirstName = "Chuck", LastName = "Norris", SensitiveInformation = "Did not actually walk on the moon without a spacesuit"},
                    new Actor { Id = 2, FirstName = "Ryan", LastName = "Gosling", SensitiveInformation = "Actually has a girlfriend" },
                    new Actor { Id = 3, FirstName = "Ana", LastName = "DeArmas", SensitiveInformation = "Is not answering my calls" }
                );

            modelBuilder.Entity<Movie>().HasData(
                    new Movie { Id = 1, Name = "Black Panther", Description = "Wakanda forever", ActorId = 1},
                    new Movie { Id = 2, Name = "Blade Runner 2077", Description = "tfw no gf", ActorId = 2 },
                    new Movie { Id = 3, Name = "The Matrix", Description = "which pill?", ActorId = 1 },
                    new Movie { Id = 4, Name = "Barbie", Description = "woke mind virus", ActorId = 3 }
                );

            modelBuilder.Entity<Biography>().HasData(
                    new Biography { Id = 1, Description = "Early internet meme.", ActorId = 1 }
                );
        }
    }
}
