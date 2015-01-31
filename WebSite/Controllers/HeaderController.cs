﻿using System;
using System.Linq;
using System.Web.Mvc;
using WebSite.Controllers.Module;
using WebSite.Models;
using System.Diagnostics.Debug;
using WebSite.Controllers.Common;
using System.Collections.Generic;

namespace WebSite.Controllers
{
    public class HeaderController : Controller
    {
        delegate String HeaderEventHandler(int user_id);
        private Dictionary<UserType,HeaderEventHandler> handerEventMap = new Dictionary<UserType, HeaderEventHandler>();
        // GET: Header
        HeaderController()
        {
            handerEventMap.Add(UserType.Expert,(int id)=> {
                var result = new SingleTableModule<expert>().FindInfo(x => x.expertId == id).SingleOrDefault();
                Assert(result != null);
                return result.user.user_name;
            });
            handerEventMap.Add(UserType.Vendor, (int id) =>
            {
                var result = new SingleTableModule<vendor>().FindInfo(x => x.vendorId == id).SingleOrDefault();
                Assert(result != null);
                return result.user.user_name;
            });
            handerEventMap.Add(UserType.Company, (int id) =>
            {
                var result = new SingleTableModule<company>().FindInfo(x => x.companyId == id).SingleOrDefault();
                Assert(result != null);
                return result.user.user_name;
            });
        }
        public ActionResult Index()
        {
            if (Session["user_id"] == null || Session["user_type"] == null)
            {
                ViewBag.userName = "注册";
                ViewBag.login = false;
            }
            else
            {
                
                var id = (Int32)Session["user_id"];
                var type = (UserType)Session["user_type"];
                ViewBag.userName = handerEventMap[type](id);
                ViewBag.login = true;
            }
            return View(/*TODO View path*/);
        }
    }
}