using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Xml;
using TopSpeed.Application.ApplicationConstants;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Domain.ApplicationEnums;
using TopSpeed.Domain.Models;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = CustomRole.MasterAdmin + "," + CustomRole.Admin)]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostenvironment;

        private readonly ILogger<BrandController> _logger;
        public BrandController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostenvironment, ILogger<BrandController> logger)
        {
            _unitOfWork = unitOfWork;

            _webHostenvironment = webHostenvironment;

            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try 
            {
            List<BrandModel> brands = await _unitOfWork.Brand.GetAllAsync();

                _logger.LogInformation("Brand List Fitched From Database Successfully");

                throw new ArgumentException();
                return View(brands);
            }

            catch(Exception ex) 
            {
                _logger.LogError("Something Went Wrong");
                return View();
            }

           
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();

        } 

        [HttpPost]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            string webrootpath = _webHostenvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newfileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webrootpath, @"images\brand");

                var extention = Path.GetExtension(file[0].FileName);

                using (var filestream = new FileStream(Path.Combine(upload, newfileName + extention), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }
                brand.BrandLogo = @"\images\brand\" + newfileName + extention;
            }
            if (ModelState.IsValid)
            {
                await _unitOfWork.Brand.Create(brand);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = CommonMessage.RecordCreated;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            BrandModel brand = await _unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            BrandModel brand = await _unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(BrandModel brand)
        {

            string webrootpath = _webHostenvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newfileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webrootpath, @"images\brand");

                var extention = Path.GetExtension(file[0].FileName);

                // Delete old Images
                var objfrmdb = await _unitOfWork.Brand.GetByIdAsync(brand.Id);

                if (objfrmdb.BrandLogo != null)
                {
                    var oldimgpath = Path.Combine(webrootpath, objfrmdb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldimgpath))
                    {
                        System.IO.File.Delete(oldimgpath);
                    }
                }

                using (var filestream = new FileStream(Path.Combine(upload, newfileName + extention), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }
                brand.BrandLogo = @"\images\brand\" + newfileName + extention;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Brand.Update(brand);
                await _unitOfWork.SaveAsync();

                TempData["Warning"] = CommonMessage.RecordUpdated;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            BrandModel brand = await _unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(BrandModel brand)
        {
            string webrootpath = _webHostenvironment.WebRootPath;

            if (!string.IsNullOrEmpty(brand.BrandLogo))
            {
                // Delete old Images
                var objfrmdb = await _unitOfWork.Brand.GetByIdAsync(brand.Id);

                if (objfrmdb.BrandLogo != null)
                {
                    var oldimgpath = Path.Combine(webrootpath, objfrmdb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldimgpath))
                    {
                        System.IO.File.Delete(oldimgpath);
                    }
                }

                await _unitOfWork.Brand.Delete(brand);
                await _unitOfWork.SaveAsync();

                TempData["Error"] = CommonMessage.RecordDeleted;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
