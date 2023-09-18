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
    public interface IArtistsSongsService
    {
        /// <summary>
        ///     Obtiene todas las relaciones de artistas con canciones
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ArtistSongDto>> GetAll(Expression<Func<ArtistSongModel, object>>[] includes = null);

        /// <summary>
        ///     Obtiene la relación entre artista y canción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArtistSongDto> GetById(long id);

        /// <summary>
        ///     Crea una relación entre artista y canción
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Create(ArtistSongCreateEditDto artistSong);

        /// <summary>
        ///     Elimina una relación entre artista y canción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Remove(long id);
    }

    public sealed class ArtistsSongsService : BaseService, IArtistsSongsService
    {

        #region Constructores

        public ArtistsSongsService(UnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        #endregion

        #region Implementación de métodos interfaz

        /// <summary>
        ///     Obtiene todas las relaciones de artistas con canciones
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ArtistSongDto>> GetAll(Expression<Func<ArtistSongModel, object>>[] includes = null)
        {
            try
            {
                var response = await Task.FromResult(_unitOfWork.ArtistsSongsRepository.GetAll(includes));
                return response.Select(s => s.ToDto(true));
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsSongsService.GetAll", ex);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene la relación entre artista y canción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArtistSongDto> GetById(long id)
        {
            try
            {
                var response = await Task.FromResult(_unitOfWork.ArtistsSongsRepository.Get(id));
                return response.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsSongsService.GetById", ex);
                throw;
            }
        }

        /// <summary>
        ///     Crea una relación entre artista y canción
        /// </summary>
        /// <param name="artistSong"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Create(ArtistSongCreateEditDto artistSong)
        {
            try
            {
                var response =  new CreateEditRemoveResponseDto();
                var errors =    await CheckErrors(artistSong);

                if (!errors.Any())
                {
                    var artistSongModel =       new ArtistSongModel();
                    artistSongModel.ArtistId =  artistSong.ArtistId;
                    artistSongModel.SongId =    artistSong.SongId;

                    _unitOfWork.ArtistsSongsRepository.Add(artistSongModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = artistSongModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsSongsService.Create", ex);
                throw;
            }
        }

        /// <summary>
        ///     Elimina una relación entre artista y canción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Remove(long id)
        {
            try
            {
                var response =  new CreateEditRemoveResponseDto();
                var artistSongModel = await Task.FromResult(_unitOfWork.ArtistsSongsRepository.Get(id));

                if (artistSongModel != null)
                {
                    _unitOfWork.ArtistsSongsRepository.Remove(artistSongModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = artistSongModel.Id;
                }
                else
                    response.Errors.Add("No existe ninguna relación con el Id: " + id);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsSongsService.Remove", ex);
                throw;
            }
        }

        #endregion

        #region Métodos privados

        /// <summary>
        ///     Comprueba si hay errores en el Dto de la relación entre artista y canción
        /// </summary>
        /// <param name="artistSong"></param>
        /// <returns></returns>
        private async Task<List<string>> CheckErrors(ArtistSongCreateEditDto artistSong)
        {
            try
            {
                var response = new List<string>();
                if (artistSong != null)
                {
                    // Comprobamos si hay alguna relación con los mismos Ids
                    if (await Task.FromResult(_unitOfWork.ArtistsSongsRepository.Any(a => a.ArtistId == artistSong.ArtistId && a.SongId == artistSong.SongId)))
                        response.Add("La relación entre artista y canción ya existe");
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

        #endregion
    }
}
