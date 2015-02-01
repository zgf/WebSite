﻿using System;
using System.Linq;
using System.Web.Mvc;
using WebSite.Controllers.Module;
using WebSite.Models;
using System.Diagnostics.Debug;
using WebSite.Controllers.Common;
using System.Web;
using System.Linq.Expressions;
using System.Collections.Generic;



namespace WebSite.Controllers
{
    public class TeamController : Controller
    {
        //创建虚拟团队
        
        // GET: Team
        public ActionResult Index()
        {
            return View();
        }
        private IQueryable<T> GetList<T>(Expression<Func<T, bool>> whereSelector) where T : class
        {
            return Utility.GetList<T>(whereSelector);
        }
        private Pair<bool, T> CreateRecord<T>(T record) where T : class
        {
            return Utility.CreateRecord(record);
        }

        //团队详情
        public ActionResult Detail(int id)
        {
            //名字,内容,创建公司.时间
            var element = GetList<team>(x => x.teamId == id).SingleOrDefault();
            if (element != null)
            {
                ViewBag.name = element.team_name;
                ViewBag.content = element.team_introduction;
                ViewBag.time = "";
                ViewBag.creator = element.purchase.company.user.user_name;
                ViewBag.detailActionName = "Team";
                return View("~/Views/Shared/detail.cshtml");
            }
            else
            {
                                return HttpNotFound();
            }
        }
        //参数 json teamId:teamIdValue
        //返回值"true"和"false"
        [HttpPost]
        public ActionResult AddTeam(int teamId)
        {
            var result = false;
            if (CheckSession())
            {
                var record = new member();
                record.teamId = teamId;
                record.vendorId = (Int32)Session["user_id"];
                result = CreateRecord<member>(record).first;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private bool CheckSession()
        {
            return Utility.CheckSession(UserType.Vendor, Session);
        }
        // 提交创建虚拟团队
        // 成员姓名， 逗号分隔 group_name
        // 采购对象 purchase_object
        // 概要 summary
        private void CreateMembers(int teamId, List<String> nameList)
        {
            //Assert ExpertName Exsit


            Assert(nameList
                .Select(x =>
                GetList<vendor>(y =>
                    y.user.user_name == x &&
                    y.user.user_type == UserType.Expert.ToString())
                    .SingleOrDefault() != null)
                    .Aggregate((x, y) =>
                        x &&
                        y ));

            var vendorIdList = nameList
                .Select(x =>
                GetList<vendor>(y =>
                y.user.user_name == x &&
                y.user.user_type == UserType.Expert.ToString())
                .SingleOrDefault().vendorId).ToList();
            foreach (var vendorId in vendorIdList)
            {
                CreateRecord<member>(new member
                {
                    teamId = teamId,
                    vendorId = vendorId
                });
            }
        }
        [HttpPost]
        public ActionResult Create(team info,String memberNames, bid bidinfo)
        {
            if (CheckSession()&&ModelState.IsValid)
            {
                var result = CreateRecord<team>(info);
                if(result.first)
                {
                    var memberNmaeList = memberNames.Split(',').ToList();
                    CreateMembers(result.second.teamId,memberNmaeList);
                    var bidderResult = Utility.CreateBidder(result.second.teamId, UserType.Team);
                    if (bidderResult.first)
                    {
                        bidinfo.bidderId = bidderResult.second.bidderId;
                        var bidResult = CreateRecord<bid>(bidinfo);
                        if (bidResult.first)
                        {
                            return RedirectToAction("Detail", "Bid", new { id = bidResult.second.bidId });
                        }
                    }
                }
            }
            return RedirectToAction("Detail","Purchase",new {id = info.purchaseId });
        }

       
    }
}