using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace MusicAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        #region Miembros privados

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Miembros internos

        internal ILogger Logger;

        internal IArtistsService ArtistsService;

        internal ISongsService SongsService;

        internal IGenresService GenresService;

        internal IArtistsSongsService ArtistsSongsService;

        #endregion

        #region Constructores

        public BaseController(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            Logger = (ILogger)serviceProvider.GetService(typeof(ILogger));
            ArtistsService = (IArtistsService)serviceProvider.GetService(typeof(IArtistsService));
            SongsService = (ISongsService)serviceProvider.GetService(typeof(ISongsService));
            GenresService = (IGenresService)serviceProvider.GetService(typeof(IGenresService));
            ArtistsSongsService = (IArtistsSongsService)serviceProvider.GetService(typeof(IArtistsSongsService));
        }

        #endregion
    }
}
