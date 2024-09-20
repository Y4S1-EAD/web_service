using Microsoft.AspNetCore.Mvc;
using web_service.Models;
using web_service.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace web_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;

        public CategoryController(CategoryService categoryService, ProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        // ------------------ GET: api/category ------------------
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categories = await _categoryService.GetAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving categories.", error = ex.Message });
            }
        }

        // ------------------ GET: api/category/{id} ------------------
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var category = await _categoryService.GetAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = "Category not found." });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the category.", error = ex.Message });
            }
        }

        // ------------------ POST: api/category ------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category newCategory)
        {
            newCategory.CategoryId = null; // Ensure CategoryId is autogenerated

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            try
            {
                await _categoryService.CreateAsync(newCategory);
                return Ok(new { message = "Category created successfully.", category = newCategory });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the category.", error = ex.Message });
            }
        }

        // ------------------ PUT: api/category/{id} ------------------
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Category updatedCategory)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            try
            {
                var category = await _categoryService.GetAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = "Category not found." });
                }

                updatedCategory.CategoryId = category.CategoryId;

                await _categoryService.UpdateAsync(id, updatedCategory);

                return Ok(new { message = "Category updated successfully.", category = updatedCategory });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the category.", error = ex.Message });
            }
        }

        // ------------------ DELETE: api/category/{id} ------------------
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var category = await _categoryService.GetAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = "Category not found." });
                }

                await _categoryService.RemoveAsync(id);

                return Ok(new { message = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the category.", error = ex.Message });
            }
        }
    }
}
