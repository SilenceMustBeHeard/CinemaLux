namespace CinemaApp.Web.ViewModels.Cinema
{
    public class CinemaDetailsViewModel
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public IEnumerable<CinemaDetailsMovieViewModel> Movies { get; set; }
        = null!;
    }
}