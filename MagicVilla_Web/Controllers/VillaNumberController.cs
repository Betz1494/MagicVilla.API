using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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
            List<VillaNumberDTO>? list = new List<VillaNumberDTO>();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }

            return View(model);
        }

        //public async Task<IActionResult> UpdateVilla(int villaId)
        //{
        //    var response = await _villaService.GetAsync<APIResponse>(villaId);
        //    if (response != null && response.IsSuccess)
        //    {
        //        VillaDTO? model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
        //        return View(_mapper.Map<VillaUpdateDTO>(model));
        //    }

        //    return NotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _villaService.UpdateAsync<APIResponse>(model);
        //        if (response != null && response.IsSuccess)
        //        {
        //            return RedirectToAction(nameof(IndexVilla));
        //        }
        //    }

        //    return View(model);
        //}

        //public async Task<IActionResult> DeleteVilla(int villaId)
        //{
        //    var response = await _villaService.GetAsync<APIResponse>(villaId);
        //    if (response != null && response.IsSuccess)
        //    {
        //        VillaDTO? model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
        //        return View(model);
        //    }

        //    return NotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteVilla(VillaDTO model)
        //{

        //    var response = await _villaService.DeleteAsync<APIResponse>(model.Id);
        //    if (response != null && response.IsSuccess)
        //    {
        //        return RedirectToAction(nameof(IndexVilla));
        //    }
        //    return View(model);
        //}
    }
}
