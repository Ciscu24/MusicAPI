using Common.Dtos.CreateEdit;
using Common.Dtos.Entities;
using Common.Dtos.Generic;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Linq.Expressions;

namespace MusicAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenresController : BaseController
    {
        #region Constructores

        public GenresController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        #region Métodos públicos

        [Route("getall")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new GenericResponseDto();

            try
            {
                response.Data = await GenresService.GetAll(new Expression<Func<GenreModel, object>>[] { g => g.Songs });
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("GenresController.GetAll", ex);
                response.ErrorMessage = "Error al obtener todos los géneros musicales";
                return new JsonResult(response);
            }
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = new GenericResponseDto();

            try
            {
                response.Data = await GenresService.GetById(id);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("GenresController.GetAll", ex);
                response.ErrorMessage = "Error al obtener el género musical con identificador: " + id;
                return new JsonResult(response);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] GenreCreateEditDto genre)
        {
            var response = new GenericResponseDto();

            try
            {
                var createResponse = await GenresService.Create(genre);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("GenresController.Create", ex);
                response.ErrorMessage = "Error al crear el género musical";
                return new JsonResult(response);
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] GenreCreateEditDto genre)
        {
            var response = new GenericResponseDto();

            try
            {
                var createResponse = await GenresService.Update(genre);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("GenresController.Update", ex);
                response.ErrorMessage = "Error al editar el género musical";
                return new JsonResult(response);
            }
        }

        [HttpPost]
        [Route("remove/{id}")]
        public async Task<IActionResult> Remove(long id)
        {
            var response = new GenericResponseDto();

            try
            {
                var createResponse = await GenresService.Remove(id);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("GenresController.Remove", ex);
                response.ErrorMessage = "Error al borrar el género musical";
                return new JsonResult(response);
            }
        }

        #endregion

        #region Métodos privados



        #endregion
    }
}
