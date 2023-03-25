using AutoMapper;
using MagicVilla.VillaAPI.Data;
using MagicVilla.VillaAPI.Logging;
using MagicVilla.VillaAPI.Models;
using MagicVilla.VillaAPI.Models.Dto;
using MagicVilla.VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MagicVilla.VillaAPI.Controllers.v1
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaController : ControllerBase
    {
        private readonly ILogging _logger;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbContext;
        protected APIResponse _response;

        public VillaController(ILogging logger, IVillaRepository dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        //[ResponseCache(Duration = 30)]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas(int? capacidad, string busqueda = null, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                _logger.Log("Obteniendo todas las Villas", "Informativo");

                IEnumerable<Villa> listVilla;
                
                if(capacidad > 0)
                {
                    listVilla = await _dbContext.GetAllAsync(x => x.Capacidad == capacidad, pageSize: pageSize, pageNumber: pageNumber);
                }
                else
                {
                    listVilla  = await _dbContext.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
                }

                if(!string.IsNullOrEmpty(busqueda))
                {
                    listVilla = listVilla.Where(x => x.Nombre.ToLower().Contains(busqueda.ToLower()));
                }

                Pagination pagination = new Pagination() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<VillaDTO>>(listVilla);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { $"Ocurrio un error: ", { ex.Message.ToString() } };
            }

            return _response;

        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("Ocurrio un error al obtener la villa con el id: " + id, "Error");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _dbContext.GetAsync(x => x.Id == id);

                if (villa == null)
                {
                    _logger.Log($"La villa con id: {id} no fue encontrada.", "Error");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    //_response.IsSuccess = false;
                    //_response.Errors.Add($"La villa con id: {id} no fue encontrada.");
                    ModelState.AddModelError("Errors", $"La villa con id: {id} no fue encontrada.");
                    return NotFound(ModelState); //_response
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { $"Ocurrio un error: ", { ex.Message.ToString() } };
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createVilla)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            try
            {
                if (await _dbContext.GetAsync(x => x.Nombre.ToLower() == createVilla.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("Errors", "El nombre de la villa ya existe!");
                    //_response.StatusCode = HttpStatusCode.BadRequest;
                    //_response.IsSuccess = false;
                    //_response.Errors.Add("El nombre de la villa ya existe!");
                    return BadRequest(ModelState); //ModelState
                }

                if (createVilla == null)
                {
                    return BadRequest(createVilla);
                }

                Villa villa = _mapper.Map<Villa>(createVilla);
                villa.FechaCreacion = DateTime.Now;

                await _dbContext.CreateAsync(villa);
                _response.Result = _mapper.Map<VillaCreateDTO>(villa);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { $"Ocurrio un error: ", { ex.Message.ToString() } };
            }

            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villa = await _dbContext.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    _logger.Log($"La villa con id: {id} no fue encontrada.", "Error");
                    //_response.StatusCode = HttpStatusCode.NotFound;
                    //_response.IsSuccess = false;
                    //_response.Errors.Add($"La villa con id: {id} no fue encontrada.");
                    ModelState.AddModelError("Errors", $"La villa con id: {id} no fue encontrada.");
                    return NotFound(ModelState); //_response
                }

                await _dbContext.RemoveAsync(villa);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { $"Ocurrio un error: ", { ex.Message.ToString() } };
            }

            return _response;

        }

        [HttpPut(Name = "UpdateVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(VillaUpdateDTO updateVilla)
        {
            try
            {

                var villa = await _dbContext.GetAsync(x => x.Id == updateVilla.Id, false);
                if (villa == null)
                {
                    _logger.Log($"La villa con id: {updateVilla.Id} no fue encontrada.", "Error");
                    //_response.StatusCode = HttpStatusCode.NotFound;
                    //_response.IsSuccess = false;
                    //_response.Errors.Add($"La villa con id: {updateVilla.Id} no fue encontrada.");
                    ModelState.AddModelError("Errors", $"La villa con id: {updateVilla.Id} no fue encontrada.");
                    return NotFound(ModelState);
                }

                villa = _mapper.Map<Villa>(updateVilla);
                await _dbContext.UpdateAsync(villa);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { $"Ocurrio un error: ", { ex.Message.ToString() } };
            }

            return _response;

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbContext.GetAsync(x => x.Id == id, false);
            if (villa == null)
            {
                return BadRequest();
            }

            VillaUpdateDTO villaUpdate = _mapper.Map<VillaUpdateDTO>(villa);

            patchDTO.ApplyTo(villaUpdate, ModelState);
            Villa model = _mapper.Map<Villa>(villaUpdate);

            await _dbContext.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}
