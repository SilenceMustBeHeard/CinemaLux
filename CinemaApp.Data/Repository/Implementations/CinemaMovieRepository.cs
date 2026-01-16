using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Implementations
{
    public class CinemaMovieRepository:BaseRepository<CinemaMovie,Guid> ,ICinemaMovieRepository
    {
        public CinemaMovieRepository(CinemaAppDbContext context)
            : base(context)
        {
        }
    }
}
