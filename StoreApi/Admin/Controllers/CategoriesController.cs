using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.DAL;
using StoreApi.Admin.Dtos;
using StoreApi.Admin.Dtos.CategoryDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApi.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        public readonly ICategoryRepository _catRep;
        public readonly IMapper _mapper;
        public CategoriesController(IMapper mapper,ICategoryRepository catRep)
        {
            _catRep = catRep;
            _mapper = mapper;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category =await _catRep.GetAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            CategoryDetailDto detailDto = _mapper.Map<CategoryDetailDto>(category);

            return Ok(category);
        }


        [HttpGet("")]

        public IActionResult GetAll(int page = 1)
        {
            var query = _catRep.GetAll(x => true);

            var categoryDtos = _mapper.Map<List<CategoryListItemDto>>(query.Skip((page - 1) * 4).Take(4).ToList());

            PaginatedListDto<CategoryListItemDto> listItems =
                new PaginatedListDto<CategoryListItemDto>(categoryDtos, page, 4, query.Count());

            return Ok(listItems);
        } 

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            List<CategoryListItemDto> listItemDtos = _mapper.Map<List<CategoryListItemDto>>(_catRep.GetAll(x=>true).ToList());
            return Ok(listItemDtos);
        }

        // POST api/values
        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryPostDto postDto)
        {
            if (await _catRep.IsExistAsync(x => x.Name == postDto.Name))
                return BadRequest();

            Category category = _mapper.Map<Category>(postDto);

            await _catRep.AddAsync(category);
            await _catRep.CommitAsync();

            return Created("",category);
        }

        //// PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,CategoryPostDto postDto)
        {
            var category =await _catRep.GetAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            if (await _catRep.IsExistAsync(x => x.Id != id && x.Name == postDto.Name))
                return BadRequest(new { error = new { field = "Name", message = "Name already exists!" } });

            category.Name = postDto.Name;
            _catRep.Commit();

            return Ok();
        }

        //// DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _catRep.GetAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            _catRep.Remove(category);
            _catRep.Commit();

            return Ok();
        }
    }
}

