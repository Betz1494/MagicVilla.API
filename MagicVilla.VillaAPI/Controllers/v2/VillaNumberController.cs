using AutoMapper;
using MagicVilla.VillaAPI.Logging;
using MagicVilla.VillaAPI.Models.Dto;
using MagicVilla.VillaAPI.Models;
using MagicVilla.VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MagicVilla.VillaAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
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
            _response = new();
        }


        //[MapToApiVersion("2.0")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


    }
}
