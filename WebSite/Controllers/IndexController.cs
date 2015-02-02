﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using WebSite.Controllers.Common;
using WebSite.Controllers.Module;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class IndexStruct
    {
        public String name;
        public Pair<string, int> time;
        public String content;
        public String image;
        public int detailId;
        public int id;
    };

    public class TeamStruct
    {
    }

    public class IndexController : Controller
    {
        private static readonly String listViewName = "~/Views/Shared/list.cshtml";

        public Pair<String, int> GetMonthAndDay(System.DateTime time)
        {
            var result = new Pair<String, int>(new String('\0', 0), 0);
            result.second = time.Day;
            switch (time.Month)
            {
                case 1:
                    result.first = "一月";
                    break;

                case 2:
                    result.first = "二月";
                    break;

                case 3:
                    result.first = "三月";
                    break;

                case 4:
                    result.first = "四月";
                    break;

                case 5:
                    result.first = "五月";
                    break;

                case 6:
                    result.first = "六月";
                    break;

                case 7:
                    result.first = "七月";
                    break;

                case 8:
                    result.first = "八月";
                    break;

                case 9:
                    result.first = "九月";
                    break;

                case 10:
                    result.first = "十月";
                    break;

                case 11:
                    result.first = "十一月";
                    break;

                case 12:
                    result.first = "十二月";
                    break;

                default:
                    break;
            }
            return result;
        }

        // GET: Index
        public ActionResult Index()
        {
            var expertList = Utility.GetList<expert>(1).ToList();
            var newsList = Utility.GetList<news>(6).ToList();
            var purchaseList = Utility.GetList<purchase>(6).ToList();
            var teamList = Utility.GetList<team>(12).ToList();

            ViewBag.experts = expertList
                .Select(record => new IndexStruct
                {
                    name = record.user.user_name,
                    image = record.expert_image,
                    content = record.user.user_introduction
                });
            ViewBag.newes = newsList
                .Select(record => new IndexStruct
                {
                    name = record.news_title,
                    detailId = record.newsId,
                    time = GetMonthAndDay(record.news_time)
                });
            ViewBag.purchases = purchaseList
                .Select(record => new IndexStruct
                {
                    name = record.purchase_title,
                    detailId = record.purchaseId,
                    time = GetMonthAndDay(record.purchase_time)
                });
            ViewBag.teams = teamList
                .Select(record => new IndexStruct
                {
                    detailId = record.teamId,
                    name = record.team_name
                });
            return View();
        }

        public ActionResult PurchaseList(int page)
        {
            const int count = 5;
            ViewBag.list = Utility.GetList<purchase, int>(page, count, x => x.purchaseId).ToList().
                Select(x => new IndexStruct
                {
                    detailId = x.purchaseId,
                    name = x.purchase_title,
                    content = new String(x.purchase_content.Take(200).ToArray()),
                    time = new Pair<string, int>(Utility.DateTimeToString(x.purchase_time), 0)
                });
            ViewBag.bigtitle = "采购信息";
            ViewBag.pageNum = page;
            ViewBag.sumPage = GetSumPage<purchase, int>(count,x => x.purchaseId);

            ViewBag.detailActionName = "Purchase";
            return View(listViewName);
        }

        public ActionResult NewsList(int page)
        {
            const int count = 5;
            ViewBag.list = Utility.GetList<news, int>
                (page, count, x => x.newsId).ToList().
                Select(x => new IndexStruct
                {
                    detailId = x.newsId,
                    name = x.news_title,
                    content = new String(x.news_content.Take(200).ToArray()),
                    time = new Pair<string, int>(Utility.DateTimeToString(x.news_time), 0)
                });
            ViewBag.sumPage = GetSumPage<news, int>(count,x => x.newsId);
            ViewBag.bigtitle = "新闻列表";
            ViewBag.pageNum = page;

            ViewBag.detailActionName = "New";

            return View(listViewName);
        }

        public ActionResult TeamList(int page)
        {
            const int count = 5;
            ViewBag.list = Utility.GetList<team, int>(page, count, x => x.teamId).ToList()
                .Select(x => new IndexStruct
                {
                    detailId = x.teamId,
                    name = x.team_name,
                    content = new String(x.team_introduction.Take(200).ToArray()),
                    time = new Pair<String, int>("", x.members.Count())
                });
            ViewBag.sumPage = GetSumPage<team, int>(count,x => x.teamId);
            ViewBag.pageNum = page;
            ViewBag.bigtitle = "虚拟团队";
            ViewBag.detailActionName = "Team";
            return View(listViewName);
        }

        public ActionResult ExpertList(int page)
        {
            const int count = 3;
            ViewBag.list = Utility.GetList<expert, int>(page, count, x => x.user_userId).ToList();
            var sum = GetSumPage<team, int>(count,x => x.teamId);
            ViewBag.sumPage = sum;
            ViewBag.pageNum = page;
            return View("~/Views/Expert/List.cshtml");
        }


        private int GetSumPage<T, TKey>(double count,Expression<Func<T, TKey>> keySelector) where T : class
        {
            return (int)(Utility.GetSumCount<T, TKey>(keySelector)/ count);
        }
    }
}