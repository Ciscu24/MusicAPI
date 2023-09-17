using Microsoft.Extensions.Logging;
using Models.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IArtistsSongsService
    {
    }

    public sealed class ArtistsSongsService : BaseService, IArtistsSongsService
    {

        #region Constructores

        public ArtistsSongsService(UnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        #endregion

        #region Implementación de métodos interfaz



        #endregion

        #region Métodos privados



        #endregion
    }
}
