using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MaxGeneric;

/// <summary>
/// Summary description for LogError
/// </summary>
public sealed class LogError
{
    string str;
    public LogError()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static void Log(string function, string clientevent, string errormessage)
    {
        lock(typeof(LogError))
        {
            try
            {
                string _logDate = DateTime.Now.ToString("dd-MM-yyyy");
                FileStream fs = null;
                string file_name = "";
               
                //file_name = System.Web.HttpContext.Current.Server.MapPath("LogErrors")+"\\" + _logDate + ".xml";
                file_name = clsGeneric.NullToString(ConfigurationManager.AppSettings["LOGFILE_PATH"]) + _logDate + ".xml";
                
                if (!File.Exists(file_name))
                {

                    fs = new FileStream(file_name, FileMode.Create, FileAccess.ReadWrite);
                    XmlWriter w = XmlWriter.Create(fs);
                    //StreamWriter str_writer = new StreamWriter(fs);
                    //XmlWriter w = XmlWriter.Create(str_writer);

                    w.WriteStartDocument();
                    w.WriteStartElement("ErrorLog");
                    w.WriteStartElement("Log");
                    w.WriteStartElement("ScreenName");
                    w.WriteString(function);
                    w.WriteEndElement();

                    w.WriteStartElement("MethodName");
                    w.WriteString(clientevent);
                    w.WriteEndElement();

                    w.WriteStartElement("ErrorMessage");
                    w.WriteString(errormessage);
                    w.WriteEndElement();

                    w.WriteStartElement("DateTime");
                    w.WriteString(DateTime.Now.ToString());
                    w.WriteEndElement();
                    w.WriteEndElement();
                    w.WriteEndElement();
                    w.Flush();
                    fs.Close();
                }
                else
                {

                    //fs = new FileStream(file_name, FileMode.Append, FileAccess.ReadWrite);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file_name);
                    XmlNode node = doc.SelectNodes("/ErrorLog/Log")[0];
                    XmlNode newnode = node.CloneNode(true);
                    newnode.SelectSingleNode("ScreenName").InnerText = function;
                    newnode.SelectSingleNode("MethodName").InnerText = clientevent;
                    newnode.SelectSingleNode("ErrorMessage").InnerText = errormessage;
                    newnode.SelectSingleNode("DateTime").InnerText = DateTime.Now.ToString();

                    doc.DocumentElement.AppendChild(newnode);
                    doc.Save(file_name);
                    
                }


                //string str = errormessage;
                //string[] arr;
                //arr = str.Split(
                //str = String.Join("", arr);
                //str_writer.WriteLine((" " + function + "," + clientevent + "," + str + "," + DateTime.Now.ToString())); 
                //str_writer.Close();                
            }

            catch (Exception ex)
            {

            }

            //MessageBox.Show(Me, ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation) 

        } 
    
    }

}
