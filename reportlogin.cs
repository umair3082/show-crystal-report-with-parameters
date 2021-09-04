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
   public static  class Class_change_sourceLocationOfCrystalReport
    {
        
        
       public static  string[] readText = File.ReadAllLines(Application.StartupPath + "\\Connection.txt");

        //DB_A504E8_umair3082
        public static string dbname= "accountapi";
        //public static string dbname = readText[0];
        public static string servername = readText[0];
        public static string uid= "omi";
        //public static string uid = readText[0];
        public static string pass= "admin12345)(*&";
        //public static string pass = "3082prince";
        // public static string text = File.ReadAllText(Application.StartupPath + "\\Connection.txt");

        //public static bool CheckDbConnection(string connectionString)
        //{
        //    bool TrueFalse = false;
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        TrueFalse = true;
        //    }

        //    return TrueFalse;
        //}
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
