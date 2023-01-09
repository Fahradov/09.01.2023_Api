using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data.DAL;
using StoreApi.Client.Dtos.ProductDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApi.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientProductsController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly IMapper _mapper;
        public ClientProductsController(StoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {


            var product = _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            if (product == null)
                return NotFound();

            ProductDetailDto detailDto = _mapper.Map<ProductDetailDto>(product);

            return Ok(detailDto);
        }

        [HttpGet("")]
        public IActionResult GetAll(int page = 1, int take = 4)
        {

            var products = _context.Products.Include(x => x.Category).Skip((page - 1) * take).Take(take).ToList();


            var listItems = _mapper.Map<List<ProductListItemDto>>(products);

            return Ok(listItems);
        }
    }
}

