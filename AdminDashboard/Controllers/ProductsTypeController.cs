using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Repository.Interfaces;

namespace AdminDashboard.Controllers
{
    [Authorize(Policy = "AccessDenied")]
    public class ProductsTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var types = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();

            return View(types);
        }
        [Authorize]
        public async Task<IActionResult> Create(ProductType type)
        {
            try
            {
                await _unitOfWork.Repository<ProductType, int>().AddAsync(type);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("Name", "Please Enter New Type");
                return View("Index", await _unitOfWork.Repository<ProductType, int>().GetAllAsync());
            }
        }
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _unitOfWork.Repository<ProductType, int>().GetByIdAsync(id);

            _unitOfWork.Repository<ProductType, int>().Delete(type);

            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Index");
        }
    }
}