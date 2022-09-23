using MagicVilla.VillaAPI.Data;
using MagicVilla.VillaAPI.VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.VillaAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;

        public VillaController(ILogger<VillaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Obteniendo todas las Villas");
            return Ok(VillaStore.GetVillas);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if(id == 0)
            {
                _logger.LogError("Ocurrio un error al obtener la villa con el id: " + id);
                return BadRequest();
            }
            var villa = VillaStore.GetVillas.FirstOrDefault(x => x.Id == id);

            if(villa == null)
            {
                _logger.LogError($"La villa con id: {id} no fue encontrada.");
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if(VillaStore.GetVillas.FirstOrDefault(x=>x.Nombre.ToLower() == villaDTO.Nombre.ToLower())!= null)
            {
                ModelState.AddModelError("Error", "El nombre de la villa ya existe!");
                return BadRequest(ModelState);
            }

            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var villa = VillaStore.GetVillas.OrderByDescending(x => x.Id).FirstOrDefault();
            if(villa == null)
            {
                return BadRequest();
            }
            villaDTO.Id = villa.Id + 1;
            VillaStore.GetVillas.Add(villaDTO);

            return CreatedAtRoute("GetVilla",new {  Id = villaDTO.Id}, villaDTO);    
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.GetVillas.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                _logger.LogError($"La villa con id: {id} no fue encontrada.");
                return NotFound();
            }

            VillaStore.GetVillas.Remove(villa);
            return Ok();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, VillaDTO villaDTO)
        {
            if(villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }

            var villa = VillaStore.GetVillas.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                _logger.LogError($"La villa con id: {id} no fue encontrada.");
                return NotFound();
            }

            villa.Nombre = villaDTO.Nombre;
            villa.Capacidad = villaDTO.Capacidad;
            villa.Precio = villaDTO.Precio;

            return Ok();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.GetVillas.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            
            patchDTO.ApplyTo(villa, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            return Ok();
        }

    }
}
