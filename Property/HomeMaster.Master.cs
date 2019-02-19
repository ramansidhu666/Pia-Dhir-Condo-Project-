using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Property_cls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using Newtonsoft.Json;

namespace Property
{
    public partial class HomeMaster : System.Web.UI.MasterPage
    {
        #region Global
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
        cls_Property clsobj = new cls_Property();
        public class Searches
        {
            public string label { get; set; }
            public string value { get; set; }
            public string category { get; set; }
            public string Search_text { get; set; }
        }
        
        #endregion Global
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["FirstName"] = null;
                BindMenusList();
                SiteSetting();
                bindBnanners();
                //GetCondoProp();
                InsertFeatureProperties();
                GetFeatureProperties();
                GetImages();
                DataTable dt = new DataTable();
                try
                {
                    conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString.ToString());
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    string str = "select URL from tblFiles where RunFirst=1";
                    SqlDataAdapter adp = new SqlDataAdapter(str, conn);
                    adp.Fill(dt);
                    //dt.TableName = "tbl_Virtual";
                }
                catch (Exception ex)
                { }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
                string videoURL = "";
                if (dt.Rows.Count > 0)
                {
                    videoURL = dt.Rows[0]["URL"].ToString();
                }

                frame1.Attributes["src"] = videoURL+ "?autoplay=1";

            }
        }
        void GetFeatureProperties()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.CommandText = "GetResidentialProperties";

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                DataTable dtFinal = dt.Rows.Cast<System.Data.DataRow>().Take(15).CopyToDataTable();
                if (dtFinal.Rows.Count > 0)
                {
                    
                        rptCondos.DataSource = dtFinal;
                        rptCondos.DataBind();

                    
                    lblTotalRecords.Text = dt.Rows.Count + " Condos For Sale in The GTA";
                }
               
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
        void InsertFeatureProperties()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd.CommandText = "InsertFeaturedProperties";

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception EX)
            {

            }
        }

        [System.Web.Script.Services.ScriptMethod()]
      
        public void GetImages()
        {
            Property1.MLSDataWebServiceSoapClient mlsClient = new Property1.MLSDataWebServiceSoapClient(); 
            DataTable dt = clsobj.GetDreamHouseList();
            if (dt.Rows.Count > 0)
            {
                rptImages.DataSource = dt;
                rptImages.DataBind();
               
            }
            mlsClient = null;
        }
        public void GetCondoProp()
        {
            Property1.MLSDataWebServiceSoapClient mlsClient = new Property1.MLSDataWebServiceSoapClient();
            DataTable dt = mlsClient.GetProperties_Condo("0", "0", "0", "0", "0", "0", "0");
            DataTable dtFinal = dt.Rows.Cast<System.Data.DataRow>().Skip(10).Take(15).CopyToDataTable();
            if (dtFinal.Rows.Count > 0)
            {
                rptCondos.DataSource = dtFinal;
                rptCondos.DataBind();

            }
            mlsClient = null;
            lblTotalRecords.Text = dt.Rows.Count + " Condos For Sale in The GTA";
        }
        
        private void BindMenusList()
        {
            StringBuilder StrMenu = new StringBuilder();
            DataTable dt = new DataTable();
            DataTable dtSubmenu = new DataTable();
            DataTable condos_dt = new DataTable();
            condos_dt = clsobj.GetDreamHouse();
            dt = clsobj.GetMenuList();
            if (dt.Rows.Count > 0)
            {
                string PageName = dt.Rows[0]["PageName"].ToString();
                StrMenu.Append("<a class='toggleMenu' href='#'></a>");
                StrMenu.Append("<ul class='nav'>");
                StrMenu.Append("<li class='test' style='background:none;'><a href='../Home.aspx' title='Home'>Home</a></li>");
                StrMenu.Append("<li>");
                StrMenu.Append("<a href='../about.aspx' title='About us'>About us</a>");
                StrMenu.Append("</li>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    clsobj.PageID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    dtSubmenu = clsobj.GetSubMenuBy_PageID();
                    if (dtSubmenu.Rows.Count > 0)
                    {
                        StrMenu.Append("<li><a href=#>" + dt.Rows[i]["PageName"] + "</a>");//</li>
                        StrMenu.Append("<ul>");
                        for (int j = 0; j < dtSubmenu.Rows.Count; j++)
                        {
                            StrMenu.Append("<li><a href='../StaticPages.aspx?PageID=" + dtSubmenu.Rows[j]["id"] + "' title='" + dtSubmenu.Rows[j]["PageName"] + "'>" + dtSubmenu.Rows[j]["PageName"] + "</a> </li>");
                        }
                        StrMenu.Append("</ul>");
                        StrMenu.Append("</li>");
                    }
                    else
                    {
                        StrMenu.Append("<li><a href='../StaticPages.aspx?PageID=" + dt.Rows[i]["id"] + "' title='" + dt.Rows[i]["PageName"] + "'>" + dt.Rows[i]["PageName"] + "</a>");//</li>
                    }
                }

                StrMenu.Append("<li><a href=#>Pre Constructions</a>");//</li>
                if (condos_dt.Rows.Count > 0)
                {
                    StrMenu.Append("<ul >");

                    for (int j = 0; j < condos_dt.Rows.Count; j++)
                    {
                        StrMenu.Append("<li><a href='../DreamHouseDetail.aspx?Id=" + condos_dt.Rows[j]["Id"] + "' title=''>" + condos_dt.Rows[j]["Title"] + "</a></li>");
                    }
                    StrMenu.Append("</ul>");
                }
                StrMenu.Append("<li><a href=#>Neighbourhood Guide</a>");//</li>
                StrMenu.Append("<ul>");
                StrMenu.Append("<li><a href='http://www.edu.gov.on.ca/' title='Schools' target='_blank'>Schools</ a> </li>");
                StrMenu.Append("<li><a href='http://www.trebhome.com/about_GTA/Neighbourhood/index.html' title='Neighbourhoods' target='_blank'>Neighbourhoods</a> </li>");
                StrMenu.Append("<li><a href='RealEstateNews.aspx' title='News' target='_blank'>News</a> </li>");

                StrMenu.Append("</ul>");
                StrMenu.Append("<li class='test' style='background:none;'><a href='ContactUs.aspx' title='Contact Us'>Contact Us</a></li>");
                StrMenu.Append("</ul>");
            }
            dynamicmenus.Text = StrMenu.ToString();
        }
        
        protected void SiteSetting()
        {
            try
            {
                DataTable dt = clsobj.GetSiteSettings();
                DataTable dt1 = clsobj.GetUserInfo();
                if (dt.Rows.Count > 0)
                {
                    //lblemailid.Text = Convert.ToString(dt.Rows[0]["Email"]);
                    siteTitle.Text = Convert.ToString(dt.Rows[0]["Title"]);
                    //lblemail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                    //lblmobile.Text = Convert.ToString(dt.Rows[0]["PhoneNumber"]);
                    //lblemail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                    //lblBrkrOneName.Text = Convert.ToString(dt1.Rows[0]["FirstName"]) + " " + Convert.ToString(dt1.Rows[0]["LastName"]);
                    //lblphn.Text = Convert.ToString(dt.Rows[0]["Mobile"]);
                    byte[] favimage = (byte[])dt.Rows[0]["Favicon.ico"];
                    if (favimage.Length > 0)
                    {
                        Session["MyFavicon"] = favimage;
                        favicon.Visible = true;
                        favicon.Href = "~/ShowFavicon.aspx";
                    }
                    else
                    {
                        favicon.Visible = false;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void bindBnanners()
        {
            StringBuilder html = new StringBuilder();
            DataTable dt = clsobj.GetAllBanner();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string Images;
                Images = "/admin/uploadfiles/" + dt.Rows[i]["FileName"].ToString();
                if (Images != "")
                {
                    html.AppendLine("<img src='" + Images + "'  data-thumb='" + Images + "'  alt='banner" + i + "' />");
                }
            }
            ltrImgsf.Text = html.ToString();
        }
    }
}