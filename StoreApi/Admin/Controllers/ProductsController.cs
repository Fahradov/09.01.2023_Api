using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.DAL;
using StoreApi.Admin.Dtos;
using StoreApi.Admin.Dtos.CategoryDtos;
using StoreApi.Admin.Dtos.ProductDtos;
using StoreApi.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApi.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _prodRep;
        public readonly ICategoryRepository _catRep;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ProductsController(IMapper mapper,IWebHostEnvironment env,IProductRepository prodRep,ICategoryRepository catRep)
        {
            _prodRep = prodRep;
            _mapper = mapper; 
            _env = env;
            _catRep = catRep;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await  _prodRep.GetAsync(x => x.Id == id,"Category");

            if (product == null)
                return NotFound();

            ProductDetailDto detailDto = _mapper.Map<ProductDetailDto>(product);

            return Ok(detailDto);
            
        }
        [HttpGet("")]

        public async Task<IActionResult> GetAll(int page = 1)
        {
            var query = _prodRep.GetAll(x=>true, "Category");


            var productsDtos = _mapper.Map<List<ProductListItemDto>>(query.Skip((page - 1) * 4).Take(4).ToList());

            PaginatedListDto<ProductListItemDto> listItems =
                new PaginatedListDto<ProductListItemDto>(productsDtos, page, 4, query.Count());

            return Ok(listItems);

        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            List<ProductListItemDto> listItemDtos = _mapper.Map<List<ProductListItemDto>>(_prodRep.GetAll(x=>true, "Category").ToList());
            return Ok(listItemDtos);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm]ProductPostDto postDto)
        {
            if (!await _catRep.IsExistAsync(x => x.Id == postDto.CategoryId))
                return BadRequest(new { error = new { field = "CategoryId", message = "Category not found" } });

            if(await _prodRep.IsExistAsync(x=>x.Name==postDto.Name))
                return BadRequest(new { error = new { field = "Name", message = "Name allready exists!" } });



            Product product = _mapper.Map<Product>(postDto);
            product.Image = FileManager.Save(postDto.ImageFile, _env.WebRootPath, "uploads/products");

            await _prodRep.AddAsync(product);
            await _prodRep.CommitAsync();

            return Created("", product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductPostDto postDto)
        {
            Product product = await _prodRep.GetAsync(x => x.Id == id, "Category");

            if (product == null)
                return BadRequest();

            if(product.CategoryId!=postDto.CategoryId && !await _catRep.IsExistAsync(x=>x.Id==postDto.CategoryId))
                return BadRequest(new { error = new { field = "CategoryId", message = "Category not found" } });

            if (product.Name!=postDto.Name && await _prodRep.IsExistAsync(x => x.Name == postDto.Name))
                return BadRequest(new { error = new { field = "Name", message = "Name already exists!" } });

            product.Name = postDto.Name;
            product.SalePrice = postDto.SalePrice;
            product.CostPrice = postDto.CostPrice;
            product.DiscountPercent = postDto.DiscountPercent;
            product.CategoryId = postDto.CategoryId;


            _prodRep.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _prodRep.GetAsync(x => x.Id == id, "Category");

            if (product == null)
                return NotFound();


            _prodRep.Remove(product);
            _prodRep.Commit();

            return Ok();
        }

    }
}

