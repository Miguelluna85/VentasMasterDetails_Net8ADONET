using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using Ventas.DTOs;
using Ventas.Repositories;

namespace Ventas.Controllers;
public class SaleController(
                IProductRepository ProductRepository,
                ISaleRepository SaleRepository) : Controller
{
    IProductRepository productRepository = ProductRepository;
    ISaleRepository saleRepository = SaleRepository;

    public async Task<IActionResult> Index()
    {
        IEnumerable<SaleDTO> sales = await saleRepository.GetAllSalesAsync();
        return View(sales);
    }

    public async Task<IActionResult> Details(int id)
    {
        SaleDTO sale = await saleRepository.GetSaleByIdAsync(id);
        if (sale == null)
            return NotFound();

        IEnumerable<ProductDTO> products = await productRepository.GetAllProductsAsync();
        ViewBag.Products = products;

        return View(sale);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Products = await productRepository.GetAllProductsAsync();
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(SaleDTO saleDTO)
    {
        if (ModelState.IsValid)
        {
            await saleRepository.AddSaleAsync(saleDTO);
            return RedirectToAction("Index");
        }

        ViewBag.Products = await productRepository.GetAllProductsAsync();
        return View(saleDTO);
    }

    public async Task<IActionResult> Edit(int id)
    {
        SaleDTO sale = await saleRepository.GetSaleByIdAsync(id);

        if (sale == null)
            return NotFound();

        ViewBag.Products = await productRepository.GetAllProductsAsync();

        return View(sale);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Edit(int id, SaleDTO saleDTO)
    {
        if (id != saleDTO.SaleID)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await saleRepository.UpdateProductAsync(saleDTO);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

            }
        }

        ViewBag.Products = await productRepository.GetAllProductsAsync();
        return View(saleDTO);
    }

    public async Task<IActionResult> Delete(int id)
    {
        SaleDTO sale = await saleRepository.GetSaleByIdAsync(id);

        if (sale == null)
            return NotFound();

        ViewBag.Products = await productRepository.GetAllProductsAsync();

        return View(sale);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await saleRepository.DeleteSaleAsync(id);
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Price(int id)
    {
        ProductDTO product = await productRepository.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Json(product.Price);
    }

}
