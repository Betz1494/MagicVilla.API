using AutoMapper;
using MagicVilla.VillaAPI.Logging;
using MagicVilla.VillaAPI.Models.Dto;
using MagicVilla.VillaAPI.Models;
using MagicVilla.VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla.VillaAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        private readonly ILogging _logger;
        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _dbContext;
        private readonly IVillaRepository _dbVilla;
        protected APIResponse _response;

        public VillaNumberController(ILogging logger, IMapper mapper, IVillaNumberRepository dbContext, IVillaRepository dbVilla)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _dbVilla = dbVilla;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                _logger.Log("Obteniendo todas las Villas", "Informativo");
                IEnumerable<VillaNumber> listVillaNumber = await _dbContext.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(listVillaNumber);
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

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("Ocurrio un error al obtener la villa con el id: " + id, "Error");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _dbContext.GetAsync(x => x.VillaNo == id);

                if (villa == null)
                {
                    _logger.Log($"La villa con id: {id} no fue encontrada.", "Error");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Message = $"La villa con id: {id} no fue encontrada.";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaNumberCreateDTO>(villa);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createVillaNumber)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            try
            {
                if (await _dbContext.GetAsync(x => x.VillaNo == createVillaNumber.VillaNo) != null)
                {
                    ModelState.AddModelError("Error", "El numero de esta villa ya existe!");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.Message = "El numero de esta villa ya existe!";
                    return BadRequest(_response); //ModelState
                }

                if(await _dbVilla.GetAsync(x => x.Id == createVillaNumber.VillaID) == null)
                {
                    ModelState.AddModelError("Error", "Villa ID es invalido!");
                    return BadRequest(ModelState);
                }

                if (createVillaNumber == null)
                {
                    return BadRequest(createVillaNumber);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createVillaNumber);

                await _dbContext.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberCreateDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNumber", new { Id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { $"Ocurrio un error: ", { ex.Message.ToString() } };
            }

            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villaNumber = await _dbContext.GetAsync(x => x.VillaNo == id);
                if (villaNumber == null)
                {
                    _logger.Log($"La villa con id: {id} no fue encontrada.", "Error");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Message = $"La villa con id: {id} no fue encontrada.";
                    return NotFound(_response);
                }

                await _dbContext.RemoveAsync(villaNumber);
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

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, VillaNumberUpdateDTO updateVillaNumber)
        {
            try
            {
                if (updateVillaNumber == null || id != updateVillaNumber.VillaNo)
                {
                    return BadRequest();
                }

                if (await _dbVilla.GetAsync(x => x.Id == updateVillaNumber.VillaID) == null)
                {
                    ModelState.AddModelError("Error", "Villa ID es invalido!");
                    return BadRequest(_response);
                }

                var villa = await _dbContext.GetAsync(x => x.VillaNo == id, false);
                if (villa == null)
                {
                    _logger.Log($"La villa con id: {id} no fue encontrada.", "Error");
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Message = $"La villa con id: {id} no fue encontrada.";
                    return NotFound(_response);
                }

                villa = _mapper.Map<VillaNumber>(updateVillaNumber);
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
    }
}
