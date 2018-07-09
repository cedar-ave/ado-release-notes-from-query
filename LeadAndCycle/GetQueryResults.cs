using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Npgsql;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CodeEvaler
{
    public class Rootobject2
    {
        public int count { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public string url { get; set; }
    }


    public class Fields
    {
        [JsonProperty("System.Id")]
        public int SystemId { get; set; }
        [JsonProperty("System.State")]
        public string SystemState { get; set; }
        [JsonProperty("System.AreaPath")]
        public string SystemAreaPath { get; set; }
        [JsonProperty("Microsoft.VSTS.TCM.ReproSteps")]
        public string ReproSteps { get; set; }
        [JsonProperty("Microsoft.VSTS.Common.AcceptanceCriteria")]
        public string AcceptanceCriteria { get; set; }
        public string SystemTeamProject { get; set; }
        public string SystemIterationPath { get; set; }
        [JsonProperty("System.WorkItemType")]
        public string SystemWorkItemType { get; set; }
        public string SystemReason { get; set; }
        [JsonProperty("System.CreatedDate")]
        public DateTime SystemCreatedDate { get; set; }
        public string SystemCreatedBy { get; set; }
        public DateTime SystemChangedDate { get; set; }
        public string SystemChangedBy { get; set; }
        [JsonProperty("System.Title")]
        public string SystemTitle { get; set; }
        public string SystemBoardColumn { get; set; }
        public bool SystemBoardColumnDone { get; set; }
        public DateTime MicrosoftVSTSCommonStateChangeDate { get; set; }
        public int MicrosoftVSTSCommonPriority { get; set; }
        public float MicrosoftVSTSCommonStackRank { get; set; }
        public string MicrosoftVSTSCommonValueArea { get; set; }
        public int MicrosoftVSTSCommonBusinessValue { get; set; }
        public float MicrosoftVSTSCommonTimeCriticality { get; set; }
        public string WEF_04C522F1C6B34DEEB816CB65C9F6E00A_KanbanColumn { get; set; }
        public bool WEF_04C522F1C6B34DEEB816CB65C9F6E00A_KanbanColumnDone { get; set; }
        [JsonProperty("System.Description")]
        public string SystemDescription { get; set; }
        [JsonProperty("Platform.DemoId")]
        public string DemoId { get; set; }
        [JsonProperty("Platform.IdeaId")]
        public string IdeaId { get; set; }
        [JsonProperty("Platform.ReleaseNote")]
        public string ReleaseNote { get; set; }
        [JsonProperty("Platform.VersionsAffected")]
        public string VersionsAffected { get; set; }
        [JsonProperty("Platform.VersionsToFix")]
        public string VersionstoFix { get; set; }
        [JsonProperty("Platform.ProductsAffected")]
        public string ProductsAffected { get; set; }
        [JsonProperty("Platform.Publish")]
        public string Publish { get; set; }
        [JsonProperty("Platform.Hotfixed")]
        public string Hotfixed { get; set; }
        public string ConnectionsSuccessCriteria { get; set; }
        public string WEF_3F7199A41B50441999DB29F6135D3C64_KanbanColumn { get; set; }
        public bool WEF_3F7199A41B50441999DB29F6135D3C64_KanbanColumnDone { get; set; }
        public string SystemAssignedTo { get; set; }
        public string SystemBoardLane { get; set; }
        public DateTime MicrosoftVSTSCommonActivatedDate { get; set; }
        public string MicrosoftVSTSCommonActivatedBy { get; set; }
        public string WEF_CC7F3CAD0F354434A6BEFD36292F86CE_KanbanColumn { get; set; }
        public bool WEF_CC7F3CAD0F354434A6BEFD36292F86CE_KanbanColumnDone { get; set; }
        public string WEF_CC7F3CAD0F354434A6BEFD36292F86CE_KanbanLane { get; set; }
        public DateTime MicrosoftVSTSSchedulingStartDate { get; set; }
        public string WEF_B704F54BA21A4FB680EB0D12088EE0A7_KanbanColumn { get; set; }
        public bool WEF_B704F54BA21A4FB680EB0D12088EE0A7_KanbanColumnDone { get; set; }
        public string MicrosoftVSTSTCMReproSteps { get; set; }
        public string ConnectionsGoalType { get; set; }
        public string MicrosoftVSTSCommonSeverity { get; set; }
        public string MicrosoftVSTSCommonActivity { get; set; }
        public float ConnectionsTimebox { get; set; }
    }

    public class Rootobject
    {
        public string queryType { get; set; }
        public string queryResultType { get; set; }
        public DateTime asOf { get; set; }
        public Column[] columns { get; set; }
        public Workitem[] workItems { get; set; }
    }

    public class Column
    {
        public string referenceName { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Workitem
    {
        public int id { get; set; }
        public string url { get; set; }
    }

    public static class Program
    {
        public static void Main()
        {
            (new CodeEvaler()).Eval();
        }
    }

    public class CodeEvaler
    {
        public void Eval()
        {
            MakeRequests();
        }
        private void MakeRequests()
        {
            HttpWebResponse response;
            string responseText;
            List<string> wiList = new List<string>() { };

            string[] urls = {
                //platform general - 1
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/2ce631a4-a9c8-4d2f-b8dc-e89230b91288",
                //install - 2
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/3fe6d7de-1344-4e74-a457-83534450864f",
                //services - 3
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/80b978bf-7aa7-4a50-8ce8-a1daa083c0cf",
                //loader engine - 4
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/49c438f3-582e-4a78-b280-94a73dc201dd",
                //atlas - 5
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/6909a60a-32d0-4b21-8e0e-93e172894a0d",
                //edw console - 6
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/e5f9c74e-71a2-417a-9e3c-cdfaa02bd8e0",
                //idea - 7
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/e5106416-f407-4481-9dd4-4b91c94790b3",
                //samd - 8
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/daf75204-2c20-47dc-b524-fdfd61eb552e",
                //SMD - 9
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/4b6c6bd0-67cb-4554-87b8-d4a34a715b4a",
                //DCM - 10
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/f360f5a2-41e2-4df1-aeba-ccafb481aacc",
                //SQL 2016 - 11
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/12609813-4e7d-4766-ab5c-88c1d9c6afbb",
                //Fabric - 12
                "https://healthcatalyst.visualstudio.com/DefaultCollection/8ce2fb79-1229-488d-9b40-c36ebf1699dc/_apis/wit/wiql/81670b05-ca45-41bd-9692-78835bab9d2f",

                };
            int i = 1;
            foreach (string url in urls)
            {
                if (Request_privatepreview_visualstudio_com(url, out response))
                {
                    responseText = ReadResponse(response);
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(responseText);

                    foreach (var item in results.workItems)
                    {
                        string wid = item.id.ToString();
                        wiList.Add(wid);
                        response.Close();
                    }

                    string wiListConcat = string.Join(",", wiList);
                    string wiDetails = "https://healthcatalyst.visualstudio.com/DefaultCollection/_apis/wit/workitems?ids=" + wiListConcat.ToString() + "&fields=System.Id,System.State,System.WorkItemType,Platform.VersionsAffected,Platform.ProductsAffected,Platform.Publish,Platform.ReleaseNote,Platform.DemoId,Platform.IdeaId&api-version=1.0";

                    //breakout into work item details
                    response = null;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(wiDetails);

                    request.UserAgent = "Fiddler";
                    request.Headers.Set(HttpRequestHeader.Authorization, "Basic aGNkb2NzOk1pbGxyb2NrMTIz");

                    response = (HttpWebResponse)request.GetResponse();

                    responseText = ReadResponse(response);
                    var wiResults = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject2>(responseText);
                    StringBuilder output = new StringBuilder();
                    StringBuilder csv = new StringBuilder();
                    bool first = true;
                    output.Append("[");
                    csv.Append("Id,State,AreaPath,WorkItemType,Title,DemoId,IdeaId,ReleaseNote,VersionsAffected,VersionstoFix,ProductsAffected,Publish,Hotfixed");
                    csv.Append(Environment.NewLine);
                    foreach (var value in wiResults.value)
                    {
                        if (first) {
                            first = false;
                        }
                        else {
                            output.Append(",");
                        }
                        output.Append("{\"id\":\"");
                        output.Append(value.fields.SystemId);
                        output.Append("\",\"state\":\"");
                        output.Append(value.fields.SystemState);
                        //output.Append("\",\"AreaPath\":\"");
                        //output.Append(value.fields.SystemAreaPath);
                        output.Append("\",\"type\":\"");
                        output.Append(value.fields.SystemWorkItemType);
                        //output.Append("\",\"Title\":\"");
                        //output.Append(value.fields.SystemTitle);
                        output.Append("\",\"demoId\":\"");
                        output.Append(value.fields.DemoId);
                        output.Append("\",\"ideaId\":\"");
                        output.Append(value.fields.IdeaId);
                        output.Append("\",\"releaseNote\":\"");
                        output.Append(value.fields.ReleaseNote);
                        output.Append("\",\"versionsAffected\":\"");
                        output.Append(value.fields.VersionsAffected);
                        //output.Append("\",\"VersionstoFix\":\"");
                        //output.Append(value.fields.VersionstoFix);
                        output.Append("\",\"productsAffected\":\"");
                        output.Append(value.fields.ProductsAffected);
                        output.Append("\",\"publish\":\"");
                        output.Append(value.fields.Publish);
                        //output.Append("\",\"Hotfixed\":\"");
                        //output.Append(value.fields.Hotfixed);
                        output.Append("\"}");
                        csv.Append("\"");
                        csv.Append(value.fields.SystemId);
                        csv.Append("\",\"");
                        csv.Append(value.fields.SystemState);
                        csv.Append("\",\"");
                        csv.Append(value.fields.SystemAreaPath);
                        csv.Append("\",\"");
                        csv.Append(value.fields.SystemWorkItemType);
                        csv.Append("\",\"");
                        csv.Append(value.fields.SystemTitle);
                        csv.Append("\",\"");
                        csv.Append(value.fields.DemoId);
                        csv.Append("\",\"");
                        csv.Append(value.fields.IdeaId);
                        csv.Append("\",\"");
                        csv.Append(value.fields.ReleaseNote);
                        csv.Append("\",\"");
                        csv.Append(value.fields.VersionsAffected);
                        csv.Append("\",\"");
                        csv.Append(value.fields.VersionstoFix);
                        csv.Append("\",\"");
                        csv.Append(value.fields.ProductsAffected);
                        csv.Append("\",\"");
                        csv.Append(value.fields.Publish);
                        csv.Append("\",\"");
                        csv.Append(value.fields.Hotfixed);
                        csv.Append("\",");
                        csv.Append(Environment.NewLine);
                    }
                    output.Append("]");
                    System.IO.File.WriteAllText($@"..\..\..\output\workitems{i}.json", output.ToString());
                    System.IO.File.WriteAllText($@"..\..\..\output\workitems{i}.csv", csv.ToString());
                    //  cleanup for end of loop
                    wiList.Clear();
                    i++;
                }
            }
        }

        private static string ReadResponse(HttpWebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                Stream streamToRead = responseStream;
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
                }

                using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        private bool Request_privatepreview_visualstudio_com(string url, out HttpWebResponse response)
        {
            response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.UserAgent = "Fiddler";
                request.Headers.Set(HttpRequestHeader.Authorization, "Basic aGNkb2NzOk1pbGxyb2NrMTIz");

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return false;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return false;
            }

            return true;
        }
    }
}