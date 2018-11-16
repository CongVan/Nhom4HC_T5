using Aspose.Cells;
using Aspose.Words;
using Report.Issue.Areas.Report.Models;
using Report.Issue.Areas.Report.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Report.Issue.Areas.Report.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/Report/

        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Session["UserID"].ToString()))
            {
                return Redirect("~/");
            }
            var UserID = Session["UserID"].ToString();
            ViewBag.UserID = UserID;
            return View();
        }

        public ActionResult GetDuAnByUserID()
        {
            if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
            {
                var userId = Convert.ToInt32(Session["UserID"].ToString());
                var service = new ReportService();
                var result = service.GetAllProject(userId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThongKeVanDe(string duanId)
        {
            if (!string.IsNullOrEmpty(duanId))
            {
                var service = new ReportService();
                var result = service.GetAllIssue(Convert.ToInt32(duanId));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VanDeThanhVien(string duanId, string taikhoanID)
        {
            var service = new ReportService();
            var result = service.GetReportMember(Convert.ToInt32(duanId),Convert.ToInt32(taikhoanID));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExcelVanDe(string duanId, string tenDuAn)
        {
            if (!string.IsNullOrEmpty(duanId))
            {
                var service = new ReportService();
                var result = service.GetAllIssue(Convert.ToInt32(duanId));
                return Json(ExportExcel(result,tenDuAn), JsonRequestBehavior.AllowGet);
            }
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public string ExportExcel(List<ReportIssueViewModel> data, string tenDuAn)
        {
            if (data == null) return "1";
            var pathExport = Server.MapPath("~/Templates/Report/BaoCaoIssue.xls");
            var wbExport = new Workbook(pathExport);
            if (pathExport == null) return "2";
            int rowIndex = 4;
            for (int i = 0; i < data.Count; i++)
            {
                wbExport.Worksheets[0].Cells.InsertRow(rowIndex);
                var item = data[i];
                wbExport.Worksheets[0].Cells["B" + (rowIndex + 1)].Value = item.HoTen;
                wbExport.Worksheets[0].Cells["C" + (rowIndex + 1)].Value = item.ChuaXacNhan;
                wbExport.Worksheets[0].Cells["D" + (rowIndex + 1)].Value = item.XacNhan;
                wbExport.Worksheets[0].Cells["E" + (rowIndex + 1)].Value = item.DangXuLy;
                wbExport.Worksheets[0].Cells["F" + (rowIndex + 1)].Value = item.DaXuLy;
            }

            var opts = new FindOptions
            {
                LookAtType = LookAtType.Contains,
                LookInType = LookInType.Values
            };
            var tda = wbExport.Worksheets[0].Cells.Find("<tenduan>", null, opts);
            if (tda != null)
            {
                tda.Value = tenDuAn;
            }
            var nbc = wbExport.Worksheets[0].Cells.Find("<ngaybaocao>", null, opts);
            if (nbc != null)
            {
                nbc.Value = DateTime.Now.ToString("dd/MM/yyyy");
            }
            var url = "Templates/Report/BaoCaoIssue" + "/" + DateTime.Now.Year + "/" +
                         DateTime.Now.Month + "/";
            var path = Path.Combine(Server.MapPath('~' + "/" + url));
            bool isExists = Directory.Exists(path);
            if (!isExists)
                Directory.CreateDirectory(path);
            var namefile = "BaoCaoIssue_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls";
            var pathsave = Path.GetDirectoryName(path) + @"\" + namefile;
            wbExport.Save(pathsave);
            url = url + namefile;
            return url;
        }

        public ActionResult WordPdfVanDe(string duanId, string tenDuAn, string format)
        {
            if (!string.IsNullOrEmpty(duanId))
            {
                var service = new ReportService();
                var result = service.GetAllIssue(Convert.ToInt32(duanId));
                var pathExport = Server.MapPath("~/Templates/Report/BaoCaoIssue.doc");
                var ds = new DataSet("office");
                var tb = new DataTable("TableIntro");
                tb.Columns.Add("TenDuAn");
                tb.Columns.Add("NgayBaoCao");
                var row = tb.NewRow();
                row["TenDuAn"] = tenDuAn;
                row["NgayBaoCao"] = DateTime.Now.ToString("dd/MM/yyyy");
                tb.Rows.Add(row);
                ds.Tables.Add(tb);
                ds.Tables.Add(result.ToDataTable());
                return Json(WordPdfThongKeBaoCao(pathExport, ds, format), JsonRequestBehavior.AllowGet);
            }
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public string WordPdfThongKeBaoCao(string strTemplateFileFullName, DataSet inputDataSet, string format)
        {
            var tabIdx = 0;

            const string pattern = @"<{2}.*?>{2}";
            const string pattern2 = @"«{1}.*?»{1}";
            try
            {
                var doc = new Document(strTemplateFileFullName);

                doc.MailMerge.ExecuteWithRegions(inputDataSet);

                var dbPr = new DocumentBuilder(doc);
                foreach (DataTable table in inputDataSet.Tables)
                {
                    if (table.Rows.Count <= 0 || table.Columns.Count <= 0)
                    {
                        var fields = doc.Range.Fields;
                        var tableTemp = table;
                        foreach (var field in from object f in fields
                                              select f.GetType().GetProperty("Result").GetValue(f, null).ToString() into fieldName
                                              select fieldName.Replace("»", string.Empty)
                                                  .Replace("«", string.Empty)
                                                  .Replace(">>", string.Empty)
                                                  .Replace("<<", string.Empty) into field
                                              where !field.Contains("TableStart") && !field.Contains("TableEnd")
                                              where !tableTemp.Columns.Contains(field)
                                              select field)
                        {
                            table.Columns.Add(field);
                        }

                        var emptyRow = table.NewRow();
                        table.Rows.InsertAt(emptyRow, 0);
                    }

                    if (string.IsNullOrEmpty(table.TableName))
                        table.TableName = "Table" + tabIdx;

                    tabIdx++;
                }

                foreach (DataTable table in inputDataSet.Tables)
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        var field = col.ColumnName;
                        var value = table.Rows[0][col].ToString();

                        //value = ReplaceSpecialChar(value);
                        if (dbPr.MoveToMergeField(field))
                            dbPr.Write(value);
                        //doc.Range.Replace("<<" + field + ">>", value, false, false);

                        doc.Range.Replace(new Regex(@"<<" + field + ">>"), new ReplaceWithHtmlEvaluator(value), false);
                    }
                    tabIdx++;
                }

                doc.Range.Replace(new Regex(pattern), string.Empty);
                doc.Range.Replace(new Regex(pattern2), string.Empty);

                var file = new FileInfo(strTemplateFileFullName);
                var saveOptions = Aspose.Words.Saving.SaveOptions.CreateSaveOptions(format.ToLower() == "pdf" ? Aspose.Words.SaveFormat.Pdf : Aspose.Words.SaveFormat.Doc);

                var url = "Templates/Report/BaoCaoIssue" + "/" + DateTime.Now.Year + "/" +
                                DateTime.Now.Month + "/";
                var path = Path.Combine(Server.MapPath('~' + "/" + url));
                bool isExists = Directory.Exists(path);
                if (!isExists)
                    Directory.CreateDirectory(path);
                var namefile = "BieuMauThongKe_" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + "." + format.ToLower();
                var pathsave = Path.GetDirectoryName(path) + @"\" + namefile;
                doc.Save(pathsave, saveOptions);
                return url + namefile;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
