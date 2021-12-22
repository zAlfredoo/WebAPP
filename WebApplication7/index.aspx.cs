using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication7
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dID=(Request.QueryString["dID"]);
            string From = (Request.QueryString["From"]);
            string To = (Request.QueryString["To"]);
            string connStr = "Data Source=localhost;Initial Catalog=Alfredo;user=ReadUsr;password=nintendo";
            string sql = "SELECT DISTINCT v.device_name deviceName, v.SessionName as \"Session.SessionName\", v.averageSpeedBySession as \"Session.averageSpeedBySession\"" +
                ",v.travelDistance as \"Session.travelDistance\"" +
                ",(select DATEPART(YEAR, [Timestamp]) as \"Timestamp.year\"" +
                ",DATEPART(MONTH, [Timestamp]) as \"Timestamp.month\"" +
                ",DATEPART(DAY, [Timestamp]) as \"Timestamp.day\"" +
                ",DATEPART(HOUR, [Timestamp]) as \"Timestamp.hour\"" +
                ",DATEPART(MINUTE, [Timestamp]) as \"Timestamp.minute\"" +
                ",DATEPART(SECOND, [Timestamp]) as \"Timestamp.second\"" +
                ",Longitude as \"Longitude\"" +
                ",Latitude as \"Latitude\"" +
                ",HorizontalAccuracies as \"accuracy\" from vtrip t " +
                "where t.Device_ID = v.Device_ID and t.SessionName = v.SessionName " +
                "order by [Timestamp] " +
                "FOR JSON PATH) \"Session.sessionData\"" +
                " from vTrip v WHERE";

            if (dID != null) { sql += " Device_ID='" + dID + "' AND "; };
            if (From != null) { sql += " cast([TimeStamp] as date)>='" + From + "' AND "; };
            if (To != null) { sql += " cast([TimeStamp] as date)<='" + To + "' AND "; };
            sql = sql.Substring(0, sql.Length - 5);
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql + " order by device_name, v.SessionName, v.averageSpeedBySession, v.travelDistance FOR JSON PATH;");
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Response.Write(reader[0].ToString());
            }
            conn.Close();
        }
    }
}