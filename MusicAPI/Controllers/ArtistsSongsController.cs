using Common.Dtos.CreateEdit;
using Common.Dtos.Generic;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.Linq.Expressions;

namespace MusicAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistsSongsController : BaseController
    {
        #region Constructores

        public ArtistsSongsController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        #region Métodos públicos

        [Route("getall")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new GenericResponseDto();

            try
            {
                response.Data = await ArtistsSongsService.GetAll(new Expression<Func<ArtistSongModel, object>>[] { a => a.Artist, a => a.Song });
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsSongsController.GetAll", ex);
                response.ErrorMessage = "Error al obtener todas las relaciónes entre artista y canción";
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
                response.Data = await ArtistsSongsService.GetById(id);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsSongsController.GetAll", ex);
                response.ErrorMessage = "Error al obtener la relación entre artista y canción con identificador: " + id;
                return new JsonResult(response);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] ArtistSongCreateEditDto artistSong)
        {
            var response = new GenericResponseDto();

            try
            {
                var createResponse = await ArtistsSongsService.Create(artistSong);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsSongsController.Create", ex);
                response.ErrorMessage = "Error al crear una relación entre artista y canción";
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
                var createResponse = await ArtistsSongsService.Remove(id);

                if (createResponse.Errors.Any())
                    response.ErrorMessage = string.Join(", ", createResponse.Errors);
                else
                    response.Data = createResponse.Id;

                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Logger.LogError("ArtistsSongsController.Remove", ex);
                response.ErrorMessage = "Error al borrar la relación entre artista y canción";
                return new JsonResult(response);
            }
        }

        #endregion

        #region Métodos privados



        #endregion
    }
}
