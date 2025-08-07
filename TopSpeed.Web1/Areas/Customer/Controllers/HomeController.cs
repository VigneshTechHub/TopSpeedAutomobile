using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;
using TopSpeed.Application.Contracts.Presistence;
using TopSpeed.Application.ExtensionMethods;
using TopSpeed.Domain.Models;
using TopSpeed.Domain.ViewModel;


namespace TopSpeed.Web1.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? page,bool resetFilter = false)
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

            List<PostModel> posts;

            if (resetFilter)
            {
                TempData.Remove("FilteredPost");
                TempData.Remove("SelectBrandId");
                TempData.Remove("SelectVehicleTypeId");
            }
            if (TempData.ContainsKey("FilteredPost"))
            {
                posts = TempData.Get<List<PostModel>>("FilteredPost");
                TempData.Keep("FilteredPost");
            }
            else 
            {
                posts = await _unitOfWork.Post.GetAllPost();
            }

            int pageSize = 3;

            int pageNumber = page ?? 1;

            int TotalItem = posts.Count;

            int TotalPages = (int)Math.Ceiling((double)TotalItem/ pageSize);

            ViewBag.TotalPages = TotalPages;
            ViewBag.CurrentPage = pageNumber;

            var pagePosts = posts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            HttpContext.Session.SetString("PreviousUrl", HttpContext.Request.Path);

           HomePostVM homePostVM = new HomePostVM() 
           {

               Posts = pagePosts,
               BrandList = brandList,
               VehicleTypeList = vehicleTypeList,
               BrandId = (Guid?)TempData["SelectBrandId"],
               VehicleTypeId = (Guid?)TempData["SelectVehicleTypeId"],
           };

            return View(homePostVM);

        }

        [HttpPost]

        public async Task<IActionResult> Index(HomePostVM homePostVM) 
        {
        var posts = await _unitOfWork.Post.GetAllPost(homePostVM.searchBox, homePostVM.BrandId,homePostVM.VehicleTypeId);

            TempData.Put("FilteredPost",posts);
            TempData["SelectBrandId"] = homePostVM.BrandId;
            TempData["SelectVehicleTypeId"] = homePostVM.VehicleTypeId;

            return RedirectToAction("Index", new {page = 1, resetFilter = false });
        }
        //[Authorize]
        public async Task<IActionResult>  Details( Guid id , int? page)
        {
            PostModel post = await _unitOfWork.Post.GetPostById(id);

            List<PostModel> posts = new List<PostModel>();

            if (post != null) 
            {
                posts = await _unitOfWork.Post.GetAllPost(post.Id, post.BrandId);
            }
            ViewBag.CurrentPage = page;
            CustomerDetailsVM customerDetailsVM = new CustomerDetailsVM
            {
                Post = post,
                Posts = posts,
            };
            return View(customerDetailsVM);
        }

        public IActionResult GoBack(int? page) 
        {
            string? previousUrl = HttpContext.Session.GetString("PreviousUrl");
            if (!string.IsNullOrEmpty(previousUrl))
            {
                if (page.HasValue) 
                {
                    previousUrl = QueryHelpers.AddQueryString(previousUrl, "page", page.Value.ToString());

                }
                HttpContext.Session.Remove("PreviousUrl");
                return Redirect(previousUrl);
            }
            else 
            {
                return RedirectToAction("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
