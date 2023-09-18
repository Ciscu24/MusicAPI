using Common.Dtos.CreateEdit;
using Common.Dtos.Generic;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Linq.Expressions;

namespace MusicAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongsController : BaseController
    {
        #region Constructores

        public SongsController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        #region Métodos públicos

        [Route("getall")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new GenericResponseDto();

            try
            {
                response.Data = await SongsService.GetAll(new Expression<Func<SongModel, object>>[] { g => g.Genre });
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("SongsController.GetAll", ex);
                response.ErrorMessage = "Error al obtener todas las canciones";
                return new JsonResult(response);
            }
        }


        [Route("getallbyartistid/{artistId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllByArtistId(long artistId)
        {
            var response = new GenericResponseDto();

            try
            {
                response.Data = await SongsService.GetAllByArtistId(artistId);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("SongsController.GetAllByArtistId", ex);
                response.ErrorMessage = "Error al obtener todas las canciones de un artista";
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
                response.Data = await SongsService.GetById(id);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("SongsController.GetAll", ex);
                response.ErrorMessage = "Error al obtener la canción con identificador: " + id;
                return new JsonResult(response);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] SongCreateEditDto song)
        {
            var response = new GenericResponseDto();

            try
            {
                var createResponse = await SongsService.Create(song);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("SongsController.Create", ex);
                response.ErrorMessage = "Error al crear la canción";
                return new JsonResult(response);
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] SongCreateEditDto song)
        {
            var response = new GenericResponseDto();

            try
            {
                var createResponse = await SongsService.Update(song);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("SongsController.Update", ex);
                response.ErrorMessage = "Error al editar la canción";
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
                var createResponse = await SongsService.Remove(id);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("SongsController.Remove", ex);
                response.ErrorMessage = "Error al borrar la canción";
                return new JsonResult(response);
            }
        }

        #endregion

        #region Métodos privados



        #endregion
    }
}
