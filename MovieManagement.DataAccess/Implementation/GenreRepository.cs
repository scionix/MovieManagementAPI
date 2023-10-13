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
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieManagementDbContext context) : base(context) { }

        public IEnumerable<Genre> GetGenresWithMovies()
        {
            var genresWithMovies = _context.Genres.Include(a => a.Movies).ToList();
            return genresWithMovies;
        }

        public IEnumerable<Genre> GetGenreByName(string name)
        {
            var actorsByName = _context.Genres.Where(a => a.Name == name).Include(a => a.Movies).ToList();
            return actorsByName;
        }
    }
}
