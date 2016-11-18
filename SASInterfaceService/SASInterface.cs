using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using HTS.SAS.Entities;
using HTS.SAS.BusinessObjects;
using HTS.SAS.DataAccessObjects;
using HTS.Utilities;
using System.Xml.Serialization;
using System.Configuration;
using System.Messaging;
using System.IO;
using System.Xml;
using System.Threading;


namespace SASInterfaceService
{
    public partial class SASInterface : ServiceBase
    {

        private List<ExportDataEN> lolist = new List<ExportDataEN>();
        private System.Timers.Timer ExportTimer = new System.Timers.Timer();
        private string loInterfaceMSMQ;
        private GenericAsynchronousMQListener<ExportDataEN> ExportDataQueue = null;
        public SASInterface()
        {
            InitializeComponent();
            loInterfaceMSMQ = @".\Private$\SampleInterface";
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            //Purge ExportData MSMQ                
            this.PurgeExportDataMSMQ();

            //Load Active ExportData & and store in memory
            this.GetActiveData();

            //Strate Read Export Data MSMQ
            ExportDataQueue = new GenericAsynchronousMQListener<ExportDataEN>(
                loInterfaceMSMQ, this.ProcessExportData, q_QueueErrorEvent);
            ExportDataQueue.MaxThreads = 1;
            ExportDataQueue.Enabled = true;

            //Initiate data
            this.ExportTimer.Elapsed += new System.Timers.ElapsedEventHandler(ExportTimer_Elapsed);
            this.ExportTimer.Interval = 60000;
            this.ExportTimer.AutoReset = true;
            this.ExportTimer.Start();


        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            ExportTimer.Enabled = false;
        }

        /// <summary>
        /// Method to raise error event
        /// </summary>
        private void q_QueueErrorEvent(object listener, QueueErrorMessage message)
        {
            //HandleException(message.queueException);
        }
        /// <summary>
        /// Method to Load All Active Export Data
        /// </summary>
        private void GetActiveData()
        {
            try
            {
                int i = 0;
                ExportDataEN loen = new ExportDataEN();
                ExportDataDAL lods = new ExportDataDAL();
                loen.InterfaceID = "";
                lolist = lods.GetList(loen);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Method to Read ExportData from MSMQ and check the conditions
        /// </summary>         
        private void ProcessExportData(ExportDataEN loen)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(MyOperation), loen);
            
        } 

        /// <summary>
        /// Method to Purge Export data MSMQ
        /// </summary>
        private void PurgeExportDataMSMQ()
        {
            try
            {
                MessageQueue mq;
                if (MessageQueue.Exists(@".\Private$\SampleInterface"))
                {
                    mq = new System.Messaging.MessageQueue(@".\Private$\SampleInterface");
                    mq.Purge();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Read Data from MSMQ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                try
                {
                    for (int i = 0; i < lolist.Count; i++)
                    {
                        if (lolist[i].Frequency == "daily")
                        {
                            TimeSpan ts = TimeSpan.Parse(lolist[i].TimeofExport);
                            TimeSpan cus = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                            if (Convert.ToInt32(ts.TotalMinutes) == Convert.ToInt32(cus.TotalMinutes))
                            {
                                lolist[i].DateRange = true;
                                lolist[i].DateFrom = DateTime.Now;
                                lolist[i].DateTo = DateTime.Now;
                                ExportDataClass.GetExportData(lolist[i]);
                                Thread.Sleep(60000);
                            }
                            
                        }
                        else if (lolist[i].Frequency == "weekly")
                        {
                            DateTime dt = DateTime.Now;
                            int dw = (int)dt.DayOfWeek;
                            DateTime firstday = dt.AddDays(-dw);
                            DateTime lastday = dt.AddDays(6 - dw);

                            if ("Saturday" == DateTime.Now.DayOfWeek.ToString())
                            {
                            TimeSpan ts = TimeSpan.Parse(lolist[i].TimeofExport);
                            TimeSpan cus = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                            if (Convert.ToInt32(ts.TotalMinutes) == Convert.ToInt32(cus.TotalMinutes))
                            {
                                lolist[i].DateRange = true;
                                lolist[i].DateFrom = firstday;
                                lolist[i].DateTo = lastday;
                                    ExportDataClass.GetExportData(lolist[i]);
                                    Thread.Sleep(60000);
                                }
                            }
                            
                        }
                        else if (lolist[i].Frequency == "monthly")
                        {
                            DateTime first = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                            DateTime LASTDAY = new  DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                            if (LASTDAY == DateTime.Now)
                            {
                            TimeSpan ts = TimeSpan.Parse(lolist[i].TimeofExport);
                            TimeSpan cus = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));
                            if (Convert.ToInt32(ts.TotalMinutes) == Convert.ToInt32(cus.TotalMinutes))
                            {
                                lolist[i].DateRange = true;
                                    lolist[i].DateFrom = first;
                                    lolist[i].DateTo = LASTDAY;
                                    ExportDataClass.GetExportData(lolist[i]);
                                    Thread.Sleep(60000);
                                }
                            }
                            
                        }

                    }

                }
                catch (Exception ex)
                {

                }
            }

        }
        private void MyOperation(object obj)
        {
            lock (this)
            {
                try
                {
                    ExportDataEN loexdata = new ExportDataEN();
                    loexdata = (ExportDataEN)obj;
                    if (loexdata != null)
                    {

                        int k = lolist.FindIndex(delegate(ExportDataEN otd) { return otd.InterfaceID == loexdata.InterfaceID; });
                        if (k != -1)
                        {
                            lolist[k] = loexdata;
                        }
                        else
                        {
                            lolist.Add(loexdata);
                        }
                    }

                }
                catch (MessageQueueException ex)
                {
                    if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                    {
                        // do nothing                        
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string SerializeAnObject(object obj)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            try
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                doc.Load(stream);
            }
            catch (Exception ex)
            {
       
                throw ex;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
            return doc.InnerXml;
        }
        /// <summary>
        /// DeSerialize an object
        /// </summary>
        /// <param name="xmlOfAnObject"></param>
        /// <returns></returns>
        private object DeSerializeAnObject(string xmlOfAnObject)
        {
            ExportDataEN myObject = new ExportDataEN();
            System.IO.StringReader read = new StringReader(xmlOfAnObject);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(myObject.GetType());
            System.Xml.XmlReader reader = new XmlTextReader(read);
            try
            {
                myObject = (ExportDataEN)serializer.Deserialize(reader);
                return myObject;
            }
            catch
            {
                throw;
            }
            finally
            {
                reader.Close();
                read.Close();
                read.Dispose();
            }
        }
    }
}
