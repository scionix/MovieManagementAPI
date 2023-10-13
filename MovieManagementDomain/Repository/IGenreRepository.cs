using MovieManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement.Domain.Repository
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        IEnumerable<Genre> GetGenresWithMovies();

        IEnumerable<Genre> GetGenreByName(string name);
    }
}
