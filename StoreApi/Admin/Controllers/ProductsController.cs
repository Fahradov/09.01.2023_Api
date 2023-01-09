using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Data.DAL;
using StoreApi.Admin.Dtos.CategoryDtos;
using StoreApi.Admin.Dtos.ProductDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApi.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(StoreDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _context.Products.Include(x=>x.Category).FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            ProductDetailDto detailDto = _mapper.Map<ProductDetailDto>(product);

            return Ok(detailDto);
            
        }
        [HttpGet("")]

        public IActionResult GetAll(int page = 1)
        {
            var products = _context.Products.Include(x=>x.Category).Skip((page - 1) * 4).Take(4).ToList();


            var listItems = _mapper.Map<List<ProductListItemDto>>(products);

            return Ok(listItems);

        }

        [HttpPost("")]
        public IActionResult Create([FromForm]ProductPostDto postDto)
        {
            if (!_context.Categories.Any(x => x.Id == postDto.CategoryId))
                return BadRequest(new { error = new { field = "CategoryId", message = "Category not found" } });

            if(_context.Products.Any(x=>x.Name==postDto.Name))
                return BadRequest(new { error = new { field = "Name", message = "Name allready exists!" } });



            Product product = _mapper.Map<Product>(postDto);

            _context.Products.Add(product);
            _context.SaveChanges();

            return Created("", product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] ProductPostDto postDto)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
                return BadRequest();

            if(product.CategoryId!=postDto.CategoryId && !_context.Categories.Any(x=>x.Id==postDto.CategoryId))
                return BadRequest(new { error = new { field = "CategoryId", message = "Category not found" } });

            if (product.Name!=postDto.Name && _context.Products.Any(x => x.Name == postDto.Name))
                return BadRequest(new { error = new { field = "Name", message = "Name already exists!" } });

            product.Name = postDto.Name;
            product.SalePrice = postDto.SalePrice;
            product.CostPrice = postDto.CostPrice;
            product.DiscountPercent = postDto.DiscountPercent;
            product.CategoryId = postDto.CategoryId;


            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();


            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok();
        }

    }
}

