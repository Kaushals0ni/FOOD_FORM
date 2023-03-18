using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaseStudy_PNA.Models;

namespace CaseStudy_PNA.Controllers
{
    public class FoodController : Controller
    {
        DBFoodEntities context = new DBFoodEntities();

        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(TBLUser us)
        {
            TBLUser user = context.TBLUsers.Where(a => a.UserId.Equals(us.UserId)).FirstOrDefault();
            if(user.Password.Equals(us.Password))
            {
                Session["user"] = user.UserName;
                return RedirectToAction("getData");
            }
            else
            {
                return Json("Invalid User id or Password!",JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult logout()
        {
            Session.Remove("user");
            return RedirectToAction("login");
        }
        public ActionResult GetData()
        {
            if(Session["user"] == null)
            {
                return RedirectToAction("login");
            }
            else
            {
                return View(context.TBLFoods.ToList());
            }
        }
        public ActionResult insertData()
        {
            if (Session["user"].Equals(null))
            {
                return RedirectToAction("login");
            }
            else
            {
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult insertData(TBLFood tf)
        {
            if(tf.Cuisine.Equals("Mexican") && tf.Price > 300)
            {
                tf.Discount = 40;
            }
            else if(tf.Cuisine.Equals("Mexican") || tf.Cuisine.Equals("Italian"))
            {
                tf.Discount = 20;
            }
            else
            {
                tf.Discount = 0;
            }
            context.TBLFoods.Add(tf);
            context.SaveChanges();
            return RedirectToAction("GetData");
        }

        public ActionResult updateData(int id)
        {
            if (Session["user"].Equals(null))
            {
                return RedirectToAction("login");
            }
            else
            {
                TBLFood tf = context.TBLFoods.Find(id);
                return View(tf);
            }
        }
        [HttpPost]
        public ActionResult updateData(TBLFood formData)
        {
            TBLFood tableData = context.TBLFoods.Find(formData.Id_PK);
            tableData.Price = formData.Price;
            tableData.DishName = formData.DishName;
            tableData.Cuisine = formData.Cuisine;
            context.SaveChanges();
            return RedirectToAction("GetData");
        }

        public ActionResult deleteData(int id)
        {
            if (Session["user"].Equals(null))
            {
                return RedirectToAction("login");
            }
            else
            {
                TBLFood tf = context.TBLFoods.Find(id);
                context.TBLFoods.Remove(tf);
                context.SaveChanges();
                return RedirectToAction("GetData");
            }
        }

        public ActionResult registerUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult registerUser(TBLUser tBLUser)
        {
            context.TBLUsers.Add(tBLUser);
            context.SaveChanges();
            return RedirectToAction("login");
        }
    }
}