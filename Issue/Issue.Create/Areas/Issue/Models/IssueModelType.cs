﻿using Issue.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Issue.Create.Areas.Issue.Models
{
    public class IssueTypeModel
    {
        public string Value { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }

        public static List<IssueTypeModel> LoaiVanDe_LayDanhSach()
        {
            List<IssueTypeModel> res = new List<IssueTypeModel>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString(), CommandType.StoredProcedure, "sp_LoaiVanDe_LayDanhSach");
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        IssueTypeModel it = new IssueTypeModel();
                        it.Value = "txtLoaiVanDe";
                        it.ID = ds.Tables[0].Rows[i]["LoaiVanDeID"].ToString();
                        it.Name = ds.Tables[0].Rows[i]["TenLoaiVanDe"].ToString();

                        res.Add(it);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return res;

        }
    }
}