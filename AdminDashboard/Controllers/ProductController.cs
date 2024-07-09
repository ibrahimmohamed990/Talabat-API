using AdminDashboard.Helpers;
using AdminDashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.ProductSpecfications;

namespace AdminDashboard.Controllers
{
    [Authorize(Policy = "AccessDenied")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

		public ProductController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
		}
        public async Task<IActionResult> Index()
        {
            // Get All Products  
            var spes = new ProductWithSpecifications();

            var products = await _unitOfWork.Repository<Product, int>().GetAllAsyncWithSpecification(spes);

            var mappedProduct = _mapper.Map<IEnumerable<ProductDetailsDto>>(products);


            return View(mappedProduct);
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Create (ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");
                }

                else
                    model.PictureUrl = "images/products/hat-react2.png";

                var mappedProduct = _mapper.Map<ProductViewModel , Product>(model);

                await _unitOfWork.Repository<Product, int>().AddAsync(mappedProduct);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Edit (int id)
        {
            var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(id);

            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id , ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if(ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    if(model.PictureUrl != null)
                    {
                        PictureSetting.DeleteFile(model.PictureUrl, "products");
                        model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");
                    }

                    else
                        model.PictureUrl = PictureSetting.UploadFile(model.Image, "products");

                    var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);

                    _unitOfWork.Repository<Product, int>().Update(mappedProduct);

                    var result = await _unitOfWork.CompleteAsync();

                    if(result > 0)
                        return RedirectToAction("Index");   
                }
            }

            return View(model);
            
        }
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(id);

            var mappedProduct = _mapper.Map<Product,ProductViewModel>(product);

            return View(mappedProduct);
        }

        [HttpPost]
		public async Task<IActionResult> Delete(int id , ProductViewModel model)
        {
			if (id != model.Id)
				return NotFound();

            try
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(id);

                if (product.PictureURL != null)
                    PictureSetting.DeleteFile(product.PictureURL, "products");

                _unitOfWork.Repository<Product, int>().Delete(product);

                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {

                return View(model);
            }
		}


	}
}
