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
    public interface IArtistsService
    {
        /// <summary>
        ///     Obtiene todos los artistas
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<ArtistDto>> GetAll(Expression<Func<ArtistModel, object>>[] includes = null);

        /// <summary>
        ///     Obtiene un artista
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArtistDto> GetById(long id);

        /// <summary>
        ///     Crea un artista en BBDD
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Create(ArtistCreateEditDto artist);

        /// <summary>
        ///     Crea un artista con canciones en BBDD
        /// </summary>
        /// <param name="artistWithSongs"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> CreateWithSongs(ArtistWithSongsCreateEditDto artistWithSongs);

        /// <summary>
        ///     Edita un artista de la BBDD
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Update(ArtistCreateEditDto artist);

        /// <summary>
        ///     Elimina un artista de la BBDD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Remove(long id);
    }

    public sealed class ArtistsService : BaseService, IArtistsService
    {

        #region Constructores

        public ArtistsService(UnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        #endregion

        #region Implementación de métodos interfaz

        /// <summary>
        ///     Obtiene todos los artistas
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ArtistDto>> GetAll(Expression<Func<ArtistModel, object>>[] includes = null)
        {
            try
            {
                var response = await Task.FromResult(_unitOfWork.ArtistsRepository.GetAll(includes));
                return response.Select(s => s.ToDto(true));
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsService.GetAll", ex);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene un artista
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArtistDto> GetById(long id)
        {
            try
            {
                var response = await Task.FromResult(_unitOfWork.ArtistsRepository.Get(id));
                return response.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsService.GetById", ex);
                throw;
            }
        }

        /// <summary>
        ///     Crea un artista en BBDD
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Create(ArtistCreateEditDto artist)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                var errors = await CheckErrors(artist, false);

                if (!errors.Any())
                {
                    var artistModel =   new ArtistModel();
                    artistModel.Name =  artist.Name;

                    _unitOfWork.ArtistsRepository.Add(artistModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = artistModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsService.Create", ex);
                throw;
            }
        }

        /// <summary>
        ///     Crea un artista con canciones en BBDD
        /// </summary>
        /// <param name="artistWithSongs"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> CreateWithSongs(ArtistWithSongsCreateEditDto artistWithSongs)
        {
            try
            {
                var response =  new CreateEditRemoveResponseDto();
                var errors =    await CheckErrors(new ArtistCreateEditDto() { Name = artistWithSongs.Name }, false);

                if (!errors.Any())
                {
                    var songsService =  new SongsService(_unitOfWork, _logger);
                    var songsIds =      new List<long>();

                    foreach (var song in artistWithSongs.Songs)
                    {
                        var responseCreate = await songsService.Create(song);
                        if (!responseCreate.Errors.Any())
                            songsIds.Add((long)responseCreate.Id);
                    }

                    var artistModel =   new ArtistModel();
                    artistModel.Name =  artistWithSongs.Name;
                    _unitOfWork.ArtistsRepository.Add(artistModel);
                    await _unitOfWork.SaveChanges();

                    // Generamos la relación entre artista y canción
                    songsIds.ForEach(id => _unitOfWork.ArtistsSongsRepository.Add(new ArtistSongModel() { ArtistId = artistModel.Id, SongId = id }));

                    await _unitOfWork.SaveChanges();
                    response.Id = artistModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsService.CreateWithSongs", ex);
                throw;
            }
        }

        /// <summary>
        ///     Edita un artista de la BBDD
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Update(ArtistCreateEditDto artist)
        {
            try
            {
                var response = new CreateEditRemoveResponseDto();
                var errors = await CheckErrors(artist, true);

                if (!errors.Any())
                {
                    var artistModel = await Task.FromResult(_unitOfWork.ArtistsRepository.Get(artist.Id));
                    artistModel.Name = artist.Name;

                    _unitOfWork.ArtistsRepository.Update(artistModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = artistModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsService.Update", ex);
                throw;
            }
        }

        /// <summary>
        ///     Elimina un artista de la BBDD
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
                    var artistModel = await Task.FromResult(_unitOfWork.ArtistsRepository.Get(id));
                    _unitOfWork.ArtistsRepository.Remove(artistModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = artistModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsService.Remove", ex);
                throw;
            }
        }

        #endregion

        #region Métodos privados

        /// <summary>
        ///     Comprueba si hay errores en el Dto del artista
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        private async Task<List<string>> CheckErrors(ArtistCreateEditDto artist, bool isEdit)
        {
            try
            {
                var response = new List<string>();
                if (artist != null)
                {
                    // Si estamos editando, comprobaremos si el artista con ese Id pasado por parametro existe
                    if (isEdit)
                    {
                        var artistModel = await Task.FromResult(_unitOfWork.ArtistsRepository.Get(artist.Id));
                        if (artistModel == null)
                            response.Add("No existe ningún artista con el Id: " + artist.Id);
                    }

                    // Comprobamos si hay algún artista con ese nombre
                    if (await Task.FromResult(_unitOfWork.ArtistsRepository.Any(a => a.Name == artist.Name)))
                        response.Add("El nombre ya existe");
                }
                else
                    response.Add("El Dto está vacío");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsService.CheckErrors", ex);
                throw;
            }
        }

        /// <summary>
        ///     Compruba si hay errores antes de eliminar un artista
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<string>> CheckRemoveErrors(long id)
        {
            try
            {
                var response = new List<string>();
                var artistModel = await Task.FromResult(_unitOfWork.ArtistsRepository.Get(id));

                if (artistModel == null)
                    response.Add("No existe ningún artista con el Id: " + id);


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ArtistsService.CheckRemoveErrors", ex);
                throw;
            }
        }

        #endregion
    }
}
