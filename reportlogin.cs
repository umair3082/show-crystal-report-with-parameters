using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace accountsAPI
{
   public static  class reportlogin
    {
        public static string dbname= "Db";   
        public static string servername = "ServerName";
        public static string uid= "UserID";        
        public static string pass= "Password";
        
        public static void  SetCrystalLogin( ReportDocument oRpt)
        {
          ConnectionInfo oConnectInfo = new ConnectionInfo();
           oConnectInfo.DatabaseName = dbname;
            oConnectInfo.ServerName = servername;
            oConnectInfo.UserID = uid;
            oConnectInfo.Password = pass;
            // Set the logon credentials for all tables
            SetCrystalTablesLogin(oConnectInfo, oRpt.Database.Tables);
            // Check for subreports
            foreach (Section oSection in oRpt.ReportDefinition.Sections)
            {
                foreach (ReportObject oRptObj in oSection.ReportObjects)
                {
                    if (oRptObj.Kind == ReportObjectKind.SubreportObject)
                    {
                        // This is a subreport so set the logon credentials for this report's tables
                        SubreportObject oSubRptObj = oRptObj as SubreportObject;

                        // Open the subreport
                        ReportDocument oSubRpt = oSubRptObj.OpenSubreport(oSubRptObj.SubreportName);

                        SetCrystalTablesLogin(oConnectInfo, oSubRpt.Database.Tables);
                    }
                }
            }

            oRpt.Refresh();
            oRpt.SetDatabaseLogon(uid, pass, servername, dbname, false);
            //oRpt.SetDatabaseLogon(readText[0], "3082prince", readText[0], readText[0], false);
            oRpt.VerifyDatabase();
            oRpt.Refresh();
        }

        private static void SetCrystalTablesLogin(ConnectionInfo oConnectInfo, Tables oTables)
        {
            foreach (Table oTable in oTables)
            {
                TableLogOnInfo oLogonInfo = oTable.LogOnInfo;
                oLogonInfo.ConnectionInfo = oConnectInfo;

                oTable.ApplyLogOnInfo(oLogonInfo);
            }
        }

    }
}
