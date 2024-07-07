using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Repository.Interfaces;

namespace AdminDashboard.Controllers
{
    public class BrandController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public BrandController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
        public async Task<IActionResult> Index()
		{
			var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();

			return View(brands);
		}
        [Authorize]
        public async Task<IActionResult> Create(ProductBrand brand)
		{
			try
			{
				await _unitOfWork.Repository<ProductBrand, int>().AddAsync(brand);
				await _unitOfWork.CompleteAsync();
				return RedirectToAction("Index");
			}
			catch (System.Exception)
			{

				ModelState.AddModelError("Name", "Please Enter New Brand");
				return View("Index" , await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync());
			}
		}
        [Authorize]
        public async Task<IActionResult> Delete(int id)
		{
			var brand = await _unitOfWork.Repository<ProductBrand, int>().GetByIdAsync(id);

			_unitOfWork.Repository<ProductBrand, int>().Delete(brand);

			await _unitOfWork.CompleteAsync();

			return RedirectToAction("Index");
		}
	}
}
