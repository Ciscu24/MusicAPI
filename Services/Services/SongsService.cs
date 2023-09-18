using Common.Dtos.CreateEdit;
using Common.Dtos.Entities;
using Common.Dtos.Generic;
using Microsoft.Extensions.Logging;
using Models.Models;
using Models.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ISongsService
    {
        /// <summary>
        ///     Obtiene todas las canciones
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<SongDto>> GetAll(Expression<Func<SongModel, object>>[] includes = null);

        /// <summary>
        ///     Obtiene todas las canciónes de un artista
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        Task<IEnumerable<SongDto>> GetAllByArtistId(long artistId);

        /// <summary>
        ///     Obtiene una canción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SongDto> GetById(long id);

        /// <summary>
        ///     Crea una canción en BBDD
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Create(SongCreateEditDto song);

        /// <summary>
        ///     Edita una canción de la BBDD
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Update(SongCreateEditDto song);

        /// <summary>
        ///     Elimina una canción de la BBDD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Remove(long id);
    }

    public sealed class SongsService : BaseService, ISongsService
    {

        #region Constructores

        public SongsService(UnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        #endregion

        #region Implementación de métodos interfaz

        /// <summary>
        ///     Obtiene todas las canciones
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SongDto>> GetAll(Expression<Func<SongModel, object>>[] includes = null)
        {
            try
            {
                var response = await Task.FromResult(_unitOfWork.SongsRepository.GetAll(includes));
                return response.Select(s => s.ToDto(true));
            }
            catch (Exception ex)
            {
                _logger.LogError("SongsService.GetAll", ex);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene todas las canciónes de un artista
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SongDto>> GetAllByArtistId(long artistId)
        {
            try
            {
                var response = await Task.FromResult(_unitOfWork.ArtistsSongsRepository.GetAll(g => g.ArtistId == artistId,
                    new Expression<Func<ArtistSongModel, object>>[] { a => a.Song }));
                return response.Select(s => s.Song.ToDto(false));

            }
            catch (Exception ex)
            {
                _logger.LogError("SongsService.GetAllByArtistId", ex);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene una canción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SongDto> GetById(long id)
        {
            try
            {
                var response = await Task.FromResult(_unitOfWork.SongsRepository.Get(id));
                return response.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("SongsService.GetById", ex);
                throw;
            }
        }

        /// <summary>
        ///     Crea una canción en BBDD
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Create(SongCreateEditDto song)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                var errors = await CheckErrors(song, false);

                if (!errors.Any())
                {
                    var songModel =         new SongModel();
                    songModel.Title =       song.Title;
                    songModel.Duration =    song.Duration;
                    songModel.Quality =     song.Quality;
                    songModel.ReleaseDate = song.ReleaseDate;
                    songModel.GenreId =     song.GenreId;

                    _unitOfWork.SongsRepository.Add(songModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = songModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("SongsService.Create", ex);
                throw;
            }
        }

        /// <summary>
        ///     Edita una canción de la BBDD
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Update(SongCreateEditDto song)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                var errors = await CheckErrors(song, true);

                if (!errors.Any())
                {
                    var songModel =         await Task.FromResult(_unitOfWork.SongsRepository.Get(song.Id));
                    songModel.Title =       song.Title;
                    songModel.Duration =    song.Duration;
                    songModel.Quality =     song.Quality;
                    songModel.ReleaseDate = song.ReleaseDate;
                    songModel.GenreId =     song.GenreId;

                    _unitOfWork.SongsRepository.Update(songModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = songModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("SongsService.Update", ex);
                throw;
            }
        }

        /// <summary>
        ///     Elimina una canción de la BBDD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Remove(long id)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                var errors = await CheckRemoveErrors(id);

                if (!errors.Any())
                {
                    var songModel = await Task.FromResult(_unitOfWork.SongsRepository.Get(id));
                    _unitOfWork.SongsRepository.Remove(songModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = songModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("SongsService.Remove", ex);
                throw;
            }
        }

        #endregion

        #region Métodos privados

        /// <summary>
        ///     Comprueba si hay errores en el Dto de la canción
        /// </summary>
        /// <param name="song"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        private async Task<List<string>> CheckErrors(SongCreateEditDto song, bool isEdit)
        {
            try
            {
                var response = new List<string>();
                if (song != null)
                {
                    // Si estamos editando, comprobaremos si la canción con ese Id pasado por parametro existe
                    if (isEdit)
                    {
                        var songModel = await Task.FromResult(_unitOfWork.SongsRepository.Get(song.Id));
                        if (songModel == null)
                            response.Add("No existe ninguna canción con el Id: " + song.Id);
                    }

                    // Comprobamos si hay alguna canción con ese título
                    if (await Task.FromResult(_unitOfWork.SongsRepository.Any(a => a.Title == song.Title)))
                        response.Add("El título ya existe");
                }
                else
                    response.Add("El Dto está vacío");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("SongsService.CheckErrors", ex);
                throw;
            }
        }

        /// <summary>
        ///     Compruba si hay errores antes de eliminar una canción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<string>> CheckRemoveErrors(long id)
        {
            try
            {
                var response = new List<string>();
                var songModel = await Task.FromResult(_unitOfWork.SongsRepository.Get(id));

                if (songModel == null)
                    response.Add("No existe ninguna canción con el Id: " + id);
                    

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("SongsService.CheckRemoveErrors", ex);
                throw;
            }
        }

        #endregion
    }
}
