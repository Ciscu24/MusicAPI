using Microsoft.Extensions.Logging;
using Models.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IArtistsService
    {
    }

    public sealed class ArtistsService : BaseService, IArtistsService
    {

        #region Constructores

        public ArtistsService(UnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        #endregion

        #region Implementación de métodos interfaz



        #endregion

        #region Métodos privados



        #endregion
    }
}
