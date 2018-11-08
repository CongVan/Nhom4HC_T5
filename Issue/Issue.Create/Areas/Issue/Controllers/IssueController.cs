using Issue.Create.Areas.Issue.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issue.Create.Areas.Issue.Controllers
{
    public class IssueController : Controller
    {
        //
        // GET: /Issue/Issue/

        public ActionResult Create()
        {
            if (Request.QueryString["VanDeID"] != null)
            {
                ViewBag.VanDeID = Request.QueryString["VanDeID"];
                ViewBag.TextButton = "Cập nhật";
            }
            else
            {
                ViewBag.VanDeID = 0;
                ViewBag.TextButton = "Thêm mới";
            }

            return View();
        }

        public JsonResult LayDanhSachLoaiVanDe()
        {
            var result = IssueTypeModel.LoaiVanDe_LayDanhSach();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayThongTinVanDe(int VanDeID)
        {
            var result = IssueModel.LayThongTinVanDe(VanDeID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayDanhSachTaiKhoan()
        {
            var result = IssueModel.LayDanhSachTaiKhoan();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayDanhSachDuAn()
        {
            var result = IssueModel.LayDanhSachDuAn();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CapNhatVanDe()
        {
            IssueModel IM = new IssueModel();
            IM.VanDeID = Int32.Parse(Request.Form["VanDeID"].ToString());
            IM.DuAnID = Int32.Parse(Request.Form["DuAnID"].ToString());
            IM.LoaiVanDeID = Int32.Parse(Request.Form["LoaiVanDe"].ToString());
            IM.TenVanDe = Request.Form["TenVanDe"].ToString();
            IM.MoTa = Request.Form["MoTa"].ToString();
            IM.TrangThai = Int32.Parse(Request.Form["TrangThai"].ToString());
            IM.NguoiThucHien = Request.Form["NguoiThucHien"].ToString();
            IM.NgayBatDau = Request.Form["NgayBatDau"].ToString();
            IM.NgayKetThuc = Request.Form["NgayKetThuc"].ToString();
            IM.SoGioDuKien = Int32.Parse(Request.Form["SoGioDuKien"].ToString());
            IM.SoGioThucTe = Int32.Parse(Request.Form["SoGioThucTe"].ToString());
            IM.NguoiCapNhat = Session["UserName"].ToString();

            ResData res = IssueModel.CapNhatVanDe(IM);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}