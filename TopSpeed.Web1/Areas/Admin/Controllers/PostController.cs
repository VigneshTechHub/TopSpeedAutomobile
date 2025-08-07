using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TopSpeed.Application.ApplicationConstants;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Application.Services.Interface;
using TopSpeed.Domain.ApplicationEnums;
using TopSpeed.Domain.Models;
using TopSpeed.Domain.ViewModel;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Web1.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = CustomRole.MasterAdmin +","+ CustomRole.Admin)]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostenvironment;

        private readonly IUserNameService _userName;

        public PostController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostenvironment, IUserNameService userName)
        {
            _unitOfWork = unitOfWork;

            _webHostenvironment = webHostenvironment;

            _userName = userName;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<PostModel> posts = await _unitOfWork.Post.GetAllPost();

            return View(posts);
        }

        [HttpGet]
        public IActionResult Create()
        {

            IEnumerable<SelectListItem> brandList = _unitOfWork.Brand.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()

            });
            IEnumerable<SelectListItem> vehicleTypeList = _unitOfWork.VehicleType.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()

            });

            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType))
                .Cast<EngineAndFuelType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });


            IEnumerable<SelectListItem> transMission = Enum.GetValues(typeof(Transmission))
                .Cast<Transmission>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            PostVM postVM = new PostVM 
            {
            Post = new PostModel(),
            BrandList = brandList,
            VehicleTypeList = vehicleTypeList,
            EngineAndFuelTypeList = engineAndFuelType,
            TransmissionList = transMission
            };

            return View(postVM);

        }

        [HttpPost]
        public async Task<IActionResult> Create(PostVM postVM)
        {
            string webrootpath = _webHostenvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newfileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webrootpath, @"images\post");

                var extention = Path.GetExtension(file[0].FileName);

                using (var filestream = new FileStream(Path.Combine(upload, newfileName + extention), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }
                postVM.Post.VehicleImage = @"\images\post\" + newfileName + extention;
            }
            if (ModelState.IsValid)
            {
                await _unitOfWork.Post.Create(postVM.Post);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = CommonMessage.RecordCreated;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            PostModel post = await _unitOfWork.Post.GetPostById(id);

            post.CreatedBy = await _userName.GetUserName(post.CreatedBy);

            post.ModifiedBy = await _userName.GetUserName(post.ModifiedBy);
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            PostModel post = await _unitOfWork.Post.GetPostById(id);
            IEnumerable<SelectListItem> brandList = _unitOfWork.Brand.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()

            });
            IEnumerable<SelectListItem> vehicleTypeList = _unitOfWork.VehicleType.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()

            });

            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType))
                .Cast<EngineAndFuelType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });


            IEnumerable<SelectListItem> transMission = Enum.GetValues(typeof(Transmission))
                .Cast<Transmission>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            PostVM postVM = new PostVM
            {
                Post = post,
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transMission
            };
            return View(postVM);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(PostVM postVM)
        {

            string webrootpath = _webHostenvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newfileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webrootpath, @"images\post");

                var extention = Path.GetExtension(file[0].FileName);

                // Delete old Images
                var objfrmdb = await _unitOfWork.Post.GetByIdAsync(postVM.Post.Id);

                if (objfrmdb.VehicleImage != null)
                {
                    var oldimgpath = Path.Combine(webrootpath, objfrmdb.VehicleImage.Trim('\\'));

                    if (System.IO.File.Exists(oldimgpath))
                    {
                        System.IO.File.Delete(oldimgpath);
                    }
                }

                using (var filestream = new FileStream(Path.Combine(upload, newfileName + extention), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }
                postVM.Post.VehicleImage = @"\images\post\" + newfileName + extention;
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Post.Update(postVM.Post);
                await _unitOfWork.SaveAsync();

                TempData["Warning"] = CommonMessage.RecordUpdated;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            PostModel post = await _unitOfWork.Post.GetByIdAsync(id);
            IEnumerable<SelectListItem> brandList = _unitOfWork.Brand.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()

            });
            IEnumerable<SelectListItem> vehicleTypeList = _unitOfWork.VehicleType.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()

            });

            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType))
                .Cast<EngineAndFuelType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });


            IEnumerable<SelectListItem> transMission = Enum.GetValues(typeof(Transmission))
                .Cast<Transmission>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            PostVM postVM = new PostVM
            {
                Post = post,
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transMission
            };
            return View(postVM);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(PostVM postVM)
        {
            string webrootpath = _webHostenvironment.WebRootPath;

            if (!string.IsNullOrEmpty(postVM.Post.VehicleImage))
            {
                // Delete old Images
                var objfrmdb = await _unitOfWork.Post.GetByIdAsync(postVM.Post.Id);

                if (objfrmdb.VehicleImage != null)
                {
                    var oldimgpath = Path.Combine(webrootpath, objfrmdb.VehicleImage.Trim('\\'));

                    if (System.IO.File.Exists(oldimgpath))
                    {
                        System.IO.File.Delete(oldimgpath);
                    }
                }

                await _unitOfWork.Post.Delete(postVM.Post);
                await _unitOfWork.SaveAsync();

                TempData["Error"] = CommonMessage.RecordDeleted;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
