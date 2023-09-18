using Common.Dtos.CreateEdit;
using Common.Dtos.Generic;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Linq.Expressions;

namespace MusicAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistsController : BaseController
    {
        #region Constructores

        public ArtistsController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        #region Métodos públicos

        [Route("getall")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new GenericResponseDto();

            try
            {
                response.Data = await ArtistsService.GetAll();
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsController.GetAll", ex);
                response.ErrorMessage = "Error al obtener todos los artistas";
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
                response.Data = await ArtistsService.GetById(id);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsController.GetAll", ex);
                response.ErrorMessage = "Error al obtener el artista con identificador: " + id;
                return new JsonResult(response);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] ArtistCreateEditDto artist)
        {
            var response = new GenericResponseDto();

            try
            {
                var createResponse = await ArtistsService.Create(artist);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsController.Create", ex);
                response.ErrorMessage = "Error al crear el artista";
                return new JsonResult(response);
            }
        }

        [HttpPost]
        [Route("createwithsongs")]
        public async Task<IActionResult> CreateWithSongs([FromBody] ArtistWithSongsCreateEditDto artistWithSongs)
        {
            var response = new GenericResponseDto();

            try
            {
                var createResponse = await ArtistsService.CreateWithSongs(artistWithSongs);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsController.CreateWithSongs", ex);
                response.ErrorMessage = "Error al crear el artista con las canciones";
                return new JsonResult(response);
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] ArtistCreateEditDto artist)
        {
            var response = new GenericResponseDto();

            try
            {
                var createResponse = await ArtistsService.Update(artist);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsController.Update", ex);
                response.ErrorMessage = "Error al editar el artista";
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
                var createResponse = await ArtistsService.Remove(id);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsController.Remove", ex);
                response.ErrorMessage = "Error al borrar el artista";
                return new JsonResult(response);
            }
        }

        #endregion

        #region Métodos privados



        #endregion
    }
}
