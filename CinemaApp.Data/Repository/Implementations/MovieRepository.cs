using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Implementations
{
    public class MovieRepository : BaseRepository<Movie, Guid>, IMovieRepository
    {
        public MovieRepository(CinemaAppDbContext context) 
            : base(context)
        {
        }

        public async Task<IEnumerable<Movie>> GetAllActiveAsync()
           => await _dbSet.Where(m => m.IsDeleted).ToListAsync();

 
        public Movie? GetByTitle(string title)
            => _dbSet.FirstOrDefault(m => m.Title == title);
    }
}
