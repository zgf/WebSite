﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Debug;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebSite.Controllers.Module;
using WebSite.Models;

namespace WebSite.Controllers.Common
{
    public static class Utility
    {
        public static Pair<bool, bidder> CreateBidder(int tenderId, UserType type)
        {
            Assert(type == UserType.Vendor || type == UserType.Team);
            var record = new bidder();
            record.tendererId = tenderId;
            if (type == UserType.Team)
            {
                record.bidder_is_team = true;
            }
            else if (type == UserType.Vendor)
            {
                record.bidder_is_team = false;
            }
            return CreateRecord<bidder>(record);
        }

        public static String DateTimeToString(DateTime time)
        {
            return time.Year.ToString() + "-"
                  + time.Month.ToString() + "-"
                  + time.Day.ToString() + " "
                  + time.Hour.ToString() + "-"
                  + time.Minute.ToString() + "-"
                  + time.Second.ToString();
        }

        public static UserType GetUsetTypeByString(String type)
        {
            if (type == "Expert")
            {
                return UserType.Expert;
            }
            else if (type == "Company")
            {
                return UserType.Company;
            }
            else if (type == "Vendor")
            {
                return UserType.Vendor;
            }
            else if (type == "Team")
            {
                return UserType.Team;
            }
            else
            {
                Assert(false);
            }
            return UserType.Team;
        }

        public static bool CheckSession(UserType type, HttpSessionStateBase session)
        {
            return Convert.ToInt32(session["user_id"]) != 0 &&
                 ((UserType)session["user_type"] == type);
        }

        public static IQueryable<T> GetList<T>(Expression<Func<T, bool>> expression) where T : class
        {
            var result = new SingleTableModule<T>().FindInfo(expression);
            Assert(result != null);
            return result;
        }

        public static bool CheckUserType(String type)
        {
            return type == UserType.Expert.ToString() ||
                      type == UserType.Vendor.ToString() ||
                      type == UserType.Company.ToString();
        }

        public static void SetSession(HttpSessionStateBase session, int userId, UserType type)
        {
            session["user_id"] = userId;
            session["user_type"] = type;
        }

        public static void ClearSession(HttpSessionStateBase session)
        {
            session["user_id"] = null;
            session["user_type"] = null;
        }

        private delegate void RegisterEventHandler(int id);
        public static Pair<bool, T> CreateRecord<T>(T record) where T : class
        {
            return new SingleTableModule<T>().Create(record);
        }
        public static void SetLoginSession(HttpSessionStateBase session, int userId, String type)
        {
            Dictionary<String, RegisterEventHandler> setSessionEventMap = new Dictionary<string, RegisterEventHandler>();
            setSessionEventMap.Add(UserType.Expert.ToString(), (int id) =>
             {
                 var result = GetList<expert>(x => x.user_userId == id).SingleOrDefault();
                 Assert(result != null);
                 SetSession(session, result.expertId, UserType.Expert);
             });
            setSessionEventMap.Add(UserType.Company.ToString(), (int id) =>
            {
                var result = GetList<company>(x => x.user_userId == id).SingleOrDefault();
                Assert(result != null);
                SetSession(session, result.companyId, UserType.Company);
            });
            setSessionEventMap.Add(UserType.Vendor.ToString(), (int id) =>
            {
                var result = GetList<vendor>(x => x.user_userId == id).SingleOrDefault();
                Assert(result != null);
                SetSession(session, result.vendorId, UserType.Vendor);
            });
            setSessionEventMap[type](userId);
        }

        public static void RegisterUserTypeTable(int userId, String type)
        {
            var registerEventMap = new Dictionary<string, RegisterEventHandler>();
            registerEventMap.Add(UserType.Expert.ToString(), (int id) =>
            {
                var random = new Random();
                var number = random.Next(0, 5);
               var imageUrl = @"Protrait/" + number.ToString() + ".jpg";
               CreateRecord<expert>(new expert { user_userId = id , expert_image  = imageUrl});
            });
            registerEventMap.Add(UserType.Company.ToString(), (int id) =>
            {
                 CreateRecord<company>(new company { user_userId = id });
            });

            registerEventMap.Add(UserType.Vendor.ToString(), (int id) =>
            {
               CreateRecord<vendor>(new vendor { user_userId =  id});
            });
            registerEventMap[type](userId);
        }

        public static IQueryable<T> GetList<T, TKey>(int page, int count, Expression<Func<T, TKey>> keySelector) where T : class
        {
            return GetList<T, TKey>(page, count, x => true, keySelector);
        }

        public static IQueryable<T> GetList<T, TKey>(int page, int count, Expression<Func<T, bool>> whereSelector, Expression<Func<T, TKey>> keySelector) where T : class
        {
            return GetList<T>(whereSelector).OrderByDescending(keySelector).Skip((page - 1) * count).Take(count);
        }

        public static int GetSumCount<T, TKey>(Expression<Func<T, bool>> whereSelector, Expression<Func<T, TKey>> keySelector) where T : class
        {
            return GetList<T>(whereSelector).OrderByDescending(keySelector).Count();
        }

        public static int GetSumCount<T, TKey>(Expression<Func<T, TKey>> keySelector) where T : class
        {
            return GetSumCount(x => true, keySelector);
        }
    }
}