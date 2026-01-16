using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Implementations
{
    public class RepositoryRegistrationException : Exception
    {
        public RepositoryRegistrationException(string message) : base(message) { }
    }

}
