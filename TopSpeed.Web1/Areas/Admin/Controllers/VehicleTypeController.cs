using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TopSpeed.Application.ApplicationConstants;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Domain.Models;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = CustomRole.MasterAdmin +","+ CustomRole.Admin)]
    public class VehicleTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostenvironment;

        public VehicleTypeController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostenvironment)
        {
            _unitOfWork = unitOfWork;

            _webHostenvironment = webHostenvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<VehicleTypeModel> vehicleTypes = await _unitOfWork.VehicleType.GetAllAsync();

            return View(vehicleTypes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleTypeModel vehicleType)
        {
           
            if (ModelState.IsValid)
            {
                await _unitOfWork.VehicleType.Create(vehicleType);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = CommonMessage.RecordCreated;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
         VehicleTypeModel vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);

            return View(vehicleType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            VehicleTypeModel vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);

            return View(vehicleType);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(VehicleTypeModel vehicleType)
        {

            
            if (ModelState.IsValid)
            {
                await _unitOfWork.VehicleType.Update(vehicleType);
                await _unitOfWork.SaveAsync();

                TempData["Warning"] = CommonMessage.RecordUpdated;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            VehicleTypeModel vehicleType = await _unitOfWork.VehicleType.GetByIdAsync(id);

            return View(vehicleType);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(VehicleTypeModel vehicleType)
        {
           

                await _unitOfWork.VehicleType.Delete(vehicleType);
                await _unitOfWork.SaveAsync();

                TempData["Error"] = CommonMessage.RecordDeleted;
                return RedirectToAction(nameof(Index));
            
            
        }
    }
}
