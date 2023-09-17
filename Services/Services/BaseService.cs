using Microsoft.Extensions.Logging;
using Models.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BaseService
    {
        /// <summary>
        ///     Logger de la aplicación
        /// </summary>
        internal readonly ILogger _logger;

        /// <summary>
        ///     Unidad de trabajo
        /// </summary>
        internal readonly UnitOfWork _unitOfWork;

        public BaseService(UnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
    }
}
