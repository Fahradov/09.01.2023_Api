using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entities;
using Store.Data.DAL;
using StoreApi.Admin.Dtos.CategoryDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApi.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        public readonly StoreDbContext _context;
        public readonly IMapper _mapper;
        public CategoriesController(StoreDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            CategoryDetailDto detailDto = _mapper.Map<CategoryDetailDto>(category);

            return Ok(category);
        }


        [HttpGet("")]

        public IActionResult GetAll(int page = 1)
        {
            var categories = _context.Categories.Skip((page - 1) * 4).Take(4).ToList();

            var listItems = _mapper.Map<List<CategoryListItemDto>>(categories);

            return Ok(listItems);

        }

        // POST api/values
        [HttpPost("")]
        public IActionResult Create(CategoryPostDto postDto)
        {
            if (_context.Categories.Any(x => x.Name == postDto.Name))
                return BadRequest();

            Category category = _mapper.Map<Category>(postDto);

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Created("",category);
        }

        //// PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Update(int id,CategoryPostDto postDto)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            if (_context.Categories.Any(x => x.Id != id && x.Name == postDto.Name))
                return BadRequest(new { error = new { field = "Name", message = "Name already exists!" } });

            category.Name = postDto.Name;
            _context.SaveChanges();

            return Ok();
        }

        //// DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok();
        }
    }
}

