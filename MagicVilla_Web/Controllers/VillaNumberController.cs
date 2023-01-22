using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService, IMapper mapper)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new List<VillaNumberDTO>();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaVM = new VillaNumberCreateVM();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(x => new SelectListItem
                    {
                        Text = x.Nombre,
                        Value = x.Id.ToString()
                    });
            }

            return View(villaVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Habitacion creada correctamente!";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if(response.Errors.Count > 0)
                    {
                        TempData["error"] = "Ocurrio un error";
                        ModelState.AddModelError("Errors", response.Errors.FirstOrDefault());
                    }
                }
            }
            //Si genera error, regresa los datos del combo.
            var resp = await _villaService.GetAllAsync<APIResponse>();
            if(resp != null && resp.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result))
                    .Select( x=> new SelectListItem
                    {
                        Text=x.Nombre,
                        Value=x.Id.ToString()
                    });
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int VillaNo)
        {
            VillaNumberUpdateVM villaNumberVM = new VillaNumberUpdateVM();
            var response = await _villaNumberService.GetAsync<APIResponse>(VillaNo);
            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);
            }

            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(x => new SelectListItem
                    {
                        Text = x.Nombre,
                        Value = x.Id.ToString()
                    });

                return View(villaNumberVM);
            }

            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Habitacion actualizada correctamente!";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.Errors.Count > 0)
                    {
                        TempData["error"] = "Ocurrio un error";
                        ModelState.AddModelError("Errors", response.Errors.FirstOrDefault());
                    }
                }
            }
            //Si genera error, regresa los datos del combo.
            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result))
                    .Select(x => new SelectListItem
                    {
                        Text = x.Nombre,
                        Value = x.Id.ToString()
                    });
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {

            var response = await _villaNumberService.DeleteAsync<APIResponse>(villaNo);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Habitacion eliminada correctamente!";
            }
            else
            {
                TempData["error"] = "Ocurrio un error.";
            }
            
            return RedirectToAction(nameof(IndexVillaNumber));
        }
    }
}
