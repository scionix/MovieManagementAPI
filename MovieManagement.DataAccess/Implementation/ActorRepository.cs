using Microsoft.EntityFrameworkCore;
using MovieManagement.DataAccess.Context;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement.DataAccess.Implementation
{
    public class ActorRepository : GenericRepository<Actor>, IActorRepository
    {
        public ActorRepository(MovieManagementDbContext context) : base(context) { }

        public IEnumerable<Actor> GetActorsWithMovies()
        {
            var actorsWithMovies = _context.Actors.Include(a => a.Movies).ToList();
            return actorsWithMovies;
        }

        public IEnumerable<Actor> GetActorsWithBiography()
        {
            var actorsWithBio = _context.Actors.Include(a => a.Biography).ToList();
            return actorsWithBio;
        }

        public IEnumerable<Actor> GetActorsByFullName(string firstName, string lastName)
        {
            var actorsByName = _context.Actors.Where(a => a.FirstName == firstName && a.LastName == lastName).Include(a => a.Movies).ToList();
            return actorsByName;
        }
    }
}
