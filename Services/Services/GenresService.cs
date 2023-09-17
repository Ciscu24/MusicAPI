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
    public interface IGenresService
    {
        /// <summary>
        ///     Obtiene todos los géneros musicales
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GenreDto>> GetAll(Expression<Func<GenreModel, object>>[] includes = null);

        /// <summary>
        ///     Obtiene un género musical
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GenreDto> GetById(long id);

        /// <summary>
        ///     Crea un género musical en BBDD
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Create(GenreCreateEditDto genre);

        /// <summary>
        ///     Edita un género musical de la BBDD
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Update(GenreCreateEditDto genre);

        /// <summary>
        ///     Elimina un género musical de la BBDD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CreateEditRemoveResponseDto> Remove(long id);
    }

    public sealed class GenresService : BaseService, IGenresService
    {

        #region Constructores

        public GenresService(UnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        #endregion

        #region Implementación de métodos interfaz

        /// <summary>
        ///     Obtiene todos los géneros musicales
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GenreDto>> GetAll(Expression<Func<GenreModel, object>>[] includes = null)
        {
            try
            {
                var response = await Task.FromResult(_unitOfWork.GenresRepository.GetAll(includes));
                return response.Select(s => s.ToDto(true));
            }
            catch (Exception ex)
            {
                _logger.LogError("GenresService.GetAll", ex);
                throw;
            }
        }

        /// <summary>
        ///     Obtiene un género musical
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GenreDto> GetById(long id)
        {
            try
            {
                var response = await Task.FromResult(_unitOfWork.GenresRepository.Get(id));
                return response.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("GenresService.GetById", ex);
                throw;
            }
        }

        /// <summary>
        ///     Crea un género musical en BBDD
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Create(GenreCreateEditDto genre)
        {
            try
            {
                var response =  new CreateEditRemoveResponseDto();
                var errors =    await CheckErrors(genre, false);

                if (!errors.Any())
                {
                    var genreModel =    new GenreModel();
                    genreModel.Name =   genre.Name;

                    _unitOfWork.GenresRepository.Add(genreModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = genreModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GenresService.Create", ex);
                throw;
            }
        }

        /// <summary>
        ///     Edita un género musical de la BBDD
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        public async Task<CreateEditRemoveResponseDto> Update(GenreCreateEditDto genre)
        {
            try
            {
                var response =  new CreateEditRemoveResponseDto();
                var errors =    await CheckErrors(genre, true);

                if (!errors.Any())
                {
                    var genreModel = await Task.FromResult(_unitOfWork.GenresRepository.Get(genre.Id));
                    genreModel.Name = genre.Name;

                    _unitOfWork.GenresRepository.Update(genreModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = genreModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GenresService.Update", ex);
                throw;
            }
        }

        /// <summary>
        ///     Elimina un género musical de la BBDD
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
                    var genreModel = await Task.FromResult(_unitOfWork.GenresRepository.Get(id));
                    _unitOfWork.GenresRepository.Remove(genreModel);
                    await _unitOfWork.SaveChanges();
                    response.Id = genreModel.Id;
                }
                else
                    response.Errors = errors;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GenresService.Remove", ex);
                throw;
            }
        }

        #endregion

        #region Métodos privados

        /// <summary>
        ///     Comprueba si hay errores en el Dto del género musical
        /// </summary>
        /// <param name="genre"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public async Task<List<string>> CheckErrors(GenreCreateEditDto genre, bool isEdit) 
        {
            try
            {
                var response = new List<string>();
                if(genre != null)
                {
                    // Si estamos editando, comprobaremos si el género con ese Id pasado por parametro existe
                    if (isEdit)
                    {
                        var genereModel = await Task.FromResult(_unitOfWork.GenresRepository.Get(genre.Id));
                        if (genereModel == null)
                            response.Add("No existe ningun género musical con el Id: " + genre.Id);
                    }
                        
                    // Comprobamos si hay algún género con ese nombre
                    if(await Task.FromResult(_unitOfWork.GenresRepository.Any(a => a.Name == genre.Name)))
                        response.Add("El nombre ya existe");
                }
                else
                    response.Add("El Dto está vacío");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GenresService.CheckErrors", ex);
                throw;
            }
        }

        /// <summary>
        ///     Compruba si hay errores antes de eliminar un género
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<string>> CheckRemoveErrors(long id)
        {
            try
            {
                var response = new List<string>();
                var genreModel = await Task.FromResult(_unitOfWork.GenresRepository.Get(id));

                if (genreModel != null)
                {
                    var songsWithGenre = await Task.FromResult(_unitOfWork.SongsRepository.Any(a => a.GenreId == id));
                    if (songsWithGenre)
                        response.Add("Existen canciones con ese género musical");
                }
                else
                    response.Add("No existe ningun género musical con el Id: " + id);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("GenresService.CheckRemoveErrors", ex);
                throw;
            }
        }

        #endregion
    }
}
