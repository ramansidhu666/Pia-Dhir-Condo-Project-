using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Property
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public List<string> GetData(string DName)
        {
            List<string> listCountryName = new List<string>();
           
            List<string> result = new List<string>();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ServiceDataBase"].ConnectionString.ToString());

            Property1.MLSDataWebServiceSoapClient mlsClient = new Property1.MLSDataWebServiceSoapClient();

            DataTable dt = mlsClient.GetProperties_Condo(DName, "0", "0", "0", "0", "0", "0");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add("MLS:"+dt.Rows[i]["MLS"].ToString()+"-- "+Convert.ToString(dt.Rows[i]["Address"]) +","+dt.Rows[i]["Municipality"].ToString()+" For "+dt.Rows[i]["SaleLease"].ToString()+" $"+dt.Rows[i]["ListPrice"].ToString());
            }
            return result;
            //using (SqlCommand cmd = new SqlCommand("select Dname from DEPTDHII where Dname like '%'+@SearchText+'%'", con))
            //    {
            //        con.Open();
            //        cmd.Parameters.AddWithValue("@SearchText", DName);
            //        SqlDataReader dr = cmd.ExecuteReader();
            //        while (dr.Read())
            //        {
            //            result.Add(dr["DName"].ToString());
            //        }
            //        return result;
            //    }
            
        }   
    }
}
