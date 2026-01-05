using CinemaApp.Data.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Interfaces
{
    public interface IManagerRepository:
        IRepository<Manager, Guid>, IRepositoryAsync<Manager, Guid>
    {
    }
}
