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
using App.DTO.Category;
using App.Interface;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(AppDbContext context, ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        //[Authorize(Roles = "User")]
        public async Task<ActionResult<Category>> GetCategories()
        {
            var categoris = await _categoryRepo.GetAllAsync();
            var categorisDto = categoris.Select(s => s.ToCategoriDto());
            return Ok(categorisDto);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category.ToCategoriDto());
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequest updateDto)
        {
            var categoriesModel = await _categoryRepo.UpdateAsync(id, updateDto);
            if (categoriesModel == null) return NotFound();
            return Ok(categoriesModel.ToCategoriDto());
        }

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCategoryRequest categoryDTO)
        {
            var categoryModel = categoryDTO.ToFromCreateDOT();
            await _categoryRepo.CreateAsync(categoryModel);

            return CreatedAtAction(nameof(GetCategory), new { id = categoryModel.CategoryId }, categoryModel.ToCategoriDto());
        }
        [HttpPost("import")]
        public async Task<IActionResult> Import(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Không có file nào được upload.");
            }

            var result = await _categoryRepo.Import(file, id);
            return Ok(result);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepo.DeledeAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
