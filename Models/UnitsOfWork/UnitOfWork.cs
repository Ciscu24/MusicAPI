using Microsoft.Extensions.Logging;
using Models.Context;
using Models.Models;
using Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UnitsOfWork
{
    public class UnitOfWork
    {

        #region Miembros Privados

        /// <summary>
        ///     Contexto de la base de datos
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        ///     Logger de la aplicación
        /// </summary>
        private readonly ILogger _logger;

        #endregion

        #region Repositorios

        public GenericRepository<ArtistModel> ArtistsRepository { get; set; }

        public GenericRepository<GenreModel> GenresRepository { get; set; }

        public GenericRepository<SongModel> SongsRepository { get; set; }

        public GenericRepository<ArtistSongModel> ArtistsSongsRepository { get; set; }

        #endregion

        #region Constructores

        public UnitOfWork(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            #region Inicialización de los repositorios

            ArtistsRepository = new GenericRepository<ArtistModel>(_context, _logger);
            GenresRepository = new GenericRepository<GenreModel>(_context, _logger);
            SongsRepository = new GenericRepository<SongModel>(_context, _logger);
            ArtistsSongsRepository = new GenericRepository<ArtistSongModel>(_context, _logger);

            #endregion
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        ///     Guarda los cambios pendientes en los contextos de bases de datos
        /// </summary>
        /// <returns></returns>
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
