﻿using System.Web.Mvc;

namespace TrackWorkout.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}