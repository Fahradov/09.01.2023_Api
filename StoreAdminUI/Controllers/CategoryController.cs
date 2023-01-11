using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreAdminUI.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreAdminUI.Controllers
{
    public class CategoryController : Controller
    {
        // GET: /<controller>/
        public  IActionResult Index()
        {
            string url = "https://localhost:7287/api/admin/categories?page=1";
            HttpClient client = new HttpClient();

            var response = client.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = response.Content.ReadAsStringAsync().Result;

                PaginatedCategoryVM<CategoryVM> model = JsonConvert.DeserializeObject<PaginatedCategoryVM<CategoryVM>>(content); 

                return View(model);
            }

            return View("Error");
        }
    }
}

