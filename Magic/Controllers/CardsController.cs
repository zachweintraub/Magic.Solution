using System;
using System.Collections.Generic;
using MtgApiManager;
using MtgApiManager.Lib.Dto;
using MtgApiManager.Lib.Core;
using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Service;
using Microsoft.AspNetCore.Mvc;
using Magic;
using Magic.Models;

namespace Magic.Controllers
{
    public class CardsController : Controller
    {
        [HttpGet("/cards")]
        public ActionResult Index()
        {
            return View(DBCard.GetAll());
        }
    }
}