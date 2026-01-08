using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.GCommon
{
    public static class ExceptionMessages
    {
        public const string RepoInterfaceNotFound = "The {0} could not be added to Service Collection, because no interface matching the convention could be found! " +
            "Confention : I<RepositoryName>.";
    }
}
