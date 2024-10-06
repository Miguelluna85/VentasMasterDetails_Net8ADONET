using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ventas.DTOs;
using Ventas.Repositories;

namespace Ventas.Controllers;
public class ProductController(IProductRepository ProductRepository) : Controller
{
    private readonly IProductRepository productRepository = ProductRepository;

    public async Task<IActionResult> Index()
    {
        var lstProducts = await productRepository.GetAllProductsAsync();
        return View(lstProducts);
    }

    public async Task<IActionResult> Details(int id)
    {
        var Product = await productRepository.GetProductByIdAsync(id);
        return View(Product);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(ProductDTO productDTO)
    {
        if (ModelState.IsValid)
        {
            await productRepository.AddProductAsync(productDTO);
            return RedirectToAction("Index");
        }
        //es mejor usand oun viewModel para validaciones y logica de negocio
        return View(productDTO);
    }


    public async Task<IActionResult> Edit(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductDTO productDTO)
    {
        if (id != productDTO.ProductID)
            return NotFound();

        if (ModelState.IsValid)
        {
            await productRepository.UpdateProductAsync(productDTO);
            return RedirectToAction("Index");
        }
        //es mejor usand oun viewModel para validaciones y logica de negocio
        return View(productDTO);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    [HttpPost,ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await productRepository.DeleteProductAsync(id);
        return RedirectToAction(nameof(Index));
    }
}