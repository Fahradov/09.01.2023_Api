﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApi.Client.Controllers
{
    [ApiExplorerSettings(GroupName = "users")]
    [Route("api/users/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        public IActionResult Register()
        {
            return Ok();
        }
    }
}

