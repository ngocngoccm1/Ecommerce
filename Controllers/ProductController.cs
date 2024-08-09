using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.DTO;
using App.Mappers;
using App.DTO.Product;
using App.Interface;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<Product>> GetAll()
        {
            var products = await _productRepo.GetAllAsync();
            var productsDto = products.Select(s => s.ToProductDto());
            return Ok(productsDto);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product.ToProductDto());
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductRequest updateDto)
        {
            var productModel = await _productRepo.UpdateAsync(id, updateDto);
            if (productModel == null) return NotFound();
            return Ok(productModel.ToProductDto());
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateProductRequest productDTO)
        {
            var productModel = productDTO.ToFromCreateDOT();
            await _productRepo.CreateAsync(productModel);

            return CreatedAtAction(nameof(GetById), new { id = productModel.ProductId }, productModel.ToProductDto());
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.DeledeAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
