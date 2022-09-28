using MagicVilla.VillaAPI.Data;
using MagicVilla.VillaAPI.Logging;
using MagicVilla.VillaAPI.VillaAPI.Models;
using MagicVilla.VillaAPI.VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MagicVilla.VillaAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogging _logger;
        private readonly ApplicationDbContext _dbContext;

        public VillaController(ILogging logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task< ActionResult<IEnumerable<VillaUpdateDTO>>> GetVillas()
        {
            _logger.Log("Obteniendo todas las Villas","Informativo");
            return Ok(await _dbContext.Villa.ToListAsync());
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaUpdateDTO>> GetVilla(int id)
        {
            if(id == 0)
            {
                _logger.Log("Ocurrio un error al obtener la villa con el id: " + id, "Error");
                return BadRequest();
            }
            var villa = await _dbContext.Villa.FirstOrDefaultAsync(x => x.Id == id);

            if(villa == null)
            {
                _logger.Log($"La villa con id: {id} no fue encontrada.", "Error");
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaUpdateDTO>> CreateVilla([FromBody]VillaCreateDTO villaDTO)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if(await _dbContext.Villa.FirstOrDefaultAsync(x=>x.Nombre.ToLower() == villaDTO.Nombre.ToLower())!= null)
            {
                ModelState.AddModelError("Error", "El nombre de la villa ya existe!");
                return BadRequest(ModelState);
            }

            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }


            var villa = await  _dbContext.Villa.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if(villa == null)
            {
                return BadRequest();
            }

            Villa model = new Villa()
            {
                Nombre = villaDTO.Nombre,
                Descripcion = villaDTO.Descripcion,
                Ubicacion = villaDTO.Ubicacion,
                Capacidad = villaDTO.Capacidad,
                Precio = villaDTO.Precio,
                Comodidad = villaDTO.Comodidad,
                ImagenUrl = villaDTO.ImagenUrl, 

            };
            await _dbContext.Villa.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetVilla",new {  Id = model.Id}, model);    
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task< IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbContext.Villa.FirstOrDefaultAsync(x => x.Id == id);
            if(villa == null)
            {
                _logger.Log($"La villa con id: {id} no fue encontrada.","Error");
                return NotFound();
            }

            _dbContext.Villa.Remove(villa);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, VillaUpdateDTO villaDTO)
        {
            if(villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }

            var villa = await _dbContext.Villa.FirstOrDefaultAsync(x => x.Id == id);
            if(villa == null)
            {
                _logger.Log($"La villa con id: {id} no fue encontrada.","Error");
                return NotFound();
            }

            villa.Nombre = villaDTO.Nombre;
            villa.Descripcion = villaDTO.Descripcion;
            villa.Capacidad = villaDTO.Capacidad;
            villa.Ubicacion = villaDTO.Ubicacion;
            villa.Precio = villaDTO.Precio;
            villa.Comodidad = villaDTO.Comodidad;
            villa.ImagenUrl = villaDTO.ImagenUrl;

            _dbContext.Villa.Update(villa);
            await _dbContext.SaveChangesAsync();


            return Ok();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task< IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbContext.Villa.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }

            VillaUpdateDTO villaDTO = new VillaUpdateDTO()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Descripcion = villa.Descripcion,
                Capacidad = villa.Capacidad,
                Ubicacion = villa.Ubicacion,
                Precio = villa.Precio,
                Comodidad = villa.Comodidad,
                ImagenUrl = villa.ImagenUrl,

            };

            patchDTO.ApplyTo(villaDTO, ModelState);

            Villa model = new Villa()
            {
                Id = villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Descripcion = villaDTO.Descripcion,
                Capacidad= villaDTO.Capacidad,
                Ubicacion = villaDTO.Ubicacion,
                Precio = villaDTO.Precio,
                Comodidad = villaDTO.Comodidad,
                ImagenUrl = villaDTO.ImagenUrl,

            };

            _dbContext.Villa.Update(model);
            await _dbContext.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            return Ok();
        }

    }
}
