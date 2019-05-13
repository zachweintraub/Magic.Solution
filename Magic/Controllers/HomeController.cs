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
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {

            CardService service = new CardService();
            var result = service.Where(x => x.Page, 1)
                  .Where(x => x.PageSize, 100)
                  .All();

            return View(result);
        }

        [HttpGet("/show/{pageNum}")]
        public ActionResult Show(int pageNum)
        {

            CardService service = new CardService();
            var result = service.Where(x => x.Page, pageNum)
                  .Where(x => x.PageSize, 100)
                  .All();

            ViewBag.Page = pageNum;
            ViewBag.Service = result;
            return View();
        }

        [HttpPost("/cards/new/{pageNum}")]
        public ActionResult Create(string cardId, int pageNum)
        {
            DBCard.SaveCard(cardId);
            return RedirectToAction("Show", pageNum);
        }
    }
}