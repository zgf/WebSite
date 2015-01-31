﻿using System;
using System.Linq;
using System.Web.Mvc;
using WebSite.Controllers.Module;
using WebSite.Models;
using System.Diagnostics.Debug;
using System.EnterpriseServices;
using System.Linq.Expressions;
using System.Web;
using WebSite.Controllers.Common;

namespace WebSite.Controllers
{
    public class ExpertController : Controller
    {
     

        // GET: ExpertHome
        //专家个人中心.基本信息
        public ActionResult Index()
        {
            return Info();
        }
        public ActionResult Home()
        {
            return Info();
        }
        private bool CheckSession()
        {
            return Utility.CheckSession(UserType.Expert, Session);
        }
        private IQueryable<T> GetList<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return Utility.GetList<T>(expression);
        }

     
        public ActionResult Detail(int id)
        {
            var db = new SingleTableModule<expert>();

            var element = db.FindInfo(x=>x.expertId == id).SingleOrDefault();
            if (element != null)
            {
                ViewBag.name = element.user.user_name;
                ViewBag.content = element.user.user_introduction;
                return View("~/Views/Shared/detail.cshtml");
            }
            else
            {
                throw new HttpException(404, "Product not found.");
            }
        }
        private ActionResult Info()
        {
            if (CheckSession())
            {
                var sessionId = Convert.ToInt32(Session["user_id"]);
                var query = GetList<expert>(x => x.expertId == sessionId);
                var result = query.SingleOrDefault();
                Assert(result != null);
                ViewBag.home = result;
                return View();
            }
            return RedirectToAction("Index", "Index");
        }
        //企业邀请 列表
        public ActionResult InvitationList()
        {
            if (CheckSession())
            {
                var sessionId = Convert.ToInt32(Session["user_id"]);
                ViewBag.list = GetList<invitation>(x => x.expertId== sessionId);
                return View();
            }
            return RedirectToAction("Index", "Index");
            
        }
        //我发布的审核意见列表 
        public ActionResult AuditList()
        {
            if (CheckSession())
            {
                var sessionId = Convert.ToInt32(Session["user_id"]);

                return View(GetList<audit>(x => x.expertId==sessionId));
            }
            return RedirectToAction("Index", "Index");

        }
        ////管理员的专家列表
        //public ActionResult List(int page)
        //{
        //    var table = new SingleTableModule<expert>();
        //    var result = table.FindInfo().Skip(page * 8).Take(8);
        //    return View(result);
        //}
    }
}