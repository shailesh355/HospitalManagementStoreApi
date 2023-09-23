using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

/// <summary>
/// Summary description for Gen_Error_Rpt
/// </summary>
public class Gen_Error_Rpt
{
    public Gen_Error_Rpt()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Reporting the Error in the log.txt
    /// </summary>
    /// <param name="sMethod_Name">Methodname in which error occurs</param>
    /// <param name="e">Exception</param>
    public static void Write_Error(string sMethod_Name, Exception e)
    {
        string sFile = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
        using (StreamWriter w = File.AppendText(sFile))
        {
            w.Write("{0}-{1}->", DateTime.Now.ToShortDateString(),
            DateTime.Now.ToLongTimeString());
            if (sMethod_Name != null)
                w.Write(sMethod_Name + "-");
            if (e != null)
                w.Write(e.Message);
            w.WriteLine();
            w.Flush();
            w.Close();
        }
    }

    public static void Write_Error(string error)
    {
        string sFile = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
        using (StreamWriter w = File.AppendText(sFile))
        {
            w.Write("{0}-{1}->", DateTime.Now.ToShortDateString(),
            DateTime.Now.ToLongTimeString());
            if (error != null)
                w.Write(error);
            w.WriteLine();
            w.Flush();
            w.Close();
        }
    }

    public static void Write_Log(string file_data, string file_name)
    {
        string sFile = AppDomain.CurrentDomain.BaseDirectory + file_name + ".txt";
        using (StreamWriter w = File.AppendText(sFile))
        {
            w.Write("{0}-{1}->", DateTime.Now.ToShortDateString(),
            DateTime.Now.ToLongTimeString());
            if (file_data != null)
                w.Write(file_data);
            w.WriteLine();
            w.Flush();
            w.Close();
        }
    }
}