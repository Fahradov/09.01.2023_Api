using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Data.DAL;
using StoreApi.Client.Dtos.CategoryDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApi.Client.Controllers
{
    [ApiExplorerSettings(GroupName ="users")]
    [Route("api/users/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;
        public CategoriesController(StoreDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Name.ToLower()==name.ToLower());
            if (category == null)
                return NotFound();

            CategoryDetailDto detailDto = _mapper.Map<CategoryDetailDto>(category);

            return Ok(detailDto);

        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1,int take=4)
        {
            var categories = _context.Categories.Skip((page - 1) * take).Take(take).ToList();

            var listItems = _mapper.Map<List<CategoryListItemDto>>(categories);

            return Ok(listItems);
        }
    }
}

