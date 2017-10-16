using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;
using CommonLib;

namespace WebApi.Controllers.api
{
    public class ServiceController : BaseController
    {

        [HttpGet]
        public HttpResponseMessage Test()
        {
            return AjaxContentResult(1, "Good", "", true);
        }
        [HttpPost]
        public HttpResponseMessage AddProject(string strProject)
        {
            Project p = JsonConvert.DeserializeObject<Project>(strProject);
            p.ProName = p.ProName.Replace("%2B", "+").Base64Decode();
            var lo = db.Query<Project>(" select * from Exam_XProject where ProName='" + p.ProName + "'").ToList();
            if (lo.Count == 0)
            {
                db.Insert("Exam_XProject", "ProID", true, p);
                lo = db.Fetch<Project>(" select * from Exam_XProject where ProName='" + p.ProName + "'");
            }
            else
            {
                p.CDate = DateTime.Now;
                p.ProID = lo[0].ProID;
                db.Delete("Exam_XScore", "XProjectId", null, lo[0].ProID);
                db.Execute("update Exam_XProject set CDate='" + p.CDate + "' where ProID='" + p.ProID + "'");
            }
            return AjaxContentResult(1, "[" + JsonConvert.SerializeObject(lo[0]) + "]", "", true);
        }

        [HttpPost]
        public HttpResponseMessage AddScore(string strScore)
        {
            List<Score> ls = JsonConvert.DeserializeObject<List<Score>>(strScore);
            int count = ls.Count;
            for (int i = 0; i < count; i++)
            {
                string stuid = db.ExecuteScalar<string>("select id from dbo.View_StudentInfo where OnlyCode='" + ls[i].StuId + "'");
                if (!string.IsNullOrWhiteSpace(stuid))
                {
                    ls[i].StuId = stuid;
                    db.Insert("Exam_XScore", "ID", true, ls[i]);
                }
                else
                {
                    //   db.Insert("Exam_XScore", "ID", true, ls[i]);
                    //  ls.RemoveAt(i);
                }
            }
            return AjaxContentResult(1, "", "插入完成");
        }

        [HttpPost]
        public HttpResponseMessage AddRecordWater(string strRecordWater)
        {
            var ls = JsonConvert.DeserializeObject<RecordWater>(strRecordWater);

            db.Insert("RecordWater", "ID", true, ls);

            return AjaxContentResult(1, "", "插入完成");
        }
    }
}