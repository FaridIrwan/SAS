using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using HTS.SAS.Entities;
using System.Xml.Serialization;
using System.Configuration;
using System.Messaging;
using System.IO;
using System.Xml;

namespace HTS.SAS.DataAccessObjects
{
    public class ExportDataDAL
    {
        private string csConnectionStr = "SASNEWConnectionString";
        public static Int32 j = 0;
        public System.Messaging.MessageQueue mq;
        public ExportDataDAL()
        {
            //Q Creation
            if (MessageQueue.Exists(@".\Private$\SampleInterface"))
            {
                mq = new System.Messaging.MessageQueue(@".\Private$\SampleInterface");
                //MessageBox.Show("Already there");
            }
            else
            {
                mq = MessageQueue.Create(@".\Private$\SampleInterface",true);
                //Queue2 q2 = new Queue2();
                //q2.Show();
            }
        }

        /// <summary>
        /// Getlist  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>
        public List<ExportDataEN> GetList(ExportDataEN argEn)
        {
            List<ExportDataEN> loEnList = new List<ExportDataEN>();
            argEn.InterfaceID = argEn.InterfaceID.Replace("*", "%");
            string sqlCmd = "select * from SAS_ExportData where Interfaceid <> '0'";
            if (argEn.InterfaceID.Length != 0) sqlCmd = sqlCmd + " and Interfaceid like '" + argEn.InterfaceID + "'";
            Microsoft.Practices.EnterpriseLibrary.Data.Database coDb = DatabaseFactory.CreateDatabase(csConnectionStr);
            try
            {
                using (DbCommand cmd = coDb.GetSqlStringCommand(sqlCmd))
                {
                    using (IDataReader loReader = coDb.ExecuteReader(cmd))
                    {
                        while (loReader.Read())
                        {
                            ExportDataEN loItem = LoadObject(loReader);
                            loEnList.Add(loItem);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loEnList;
        }

        /// <summary>
        /// GetItem  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>
        public ExportDataEN GetItem(ExportDataEN argEn)
        {
            ExportDataEN loItem = new ExportDataEN();
            string sqlCmd = "Select * FROM SAS_ExportData WHERE InterfaceID = @InterfaceID";
            Microsoft.Practices.EnterpriseLibrary.Data.Database coDb = DatabaseFactory.CreateDatabase(csConnectionStr);
            try
            {
                using (DbCommand cmd = coDb.GetSqlStringCommand(sqlCmd))
                {
                    coDb.AddInParameter(cmd, "@InterfaceID", DbType.String, argEn.InterfaceID);
                    using (IDataReader loReader = coDb.ExecuteReader(cmd))
                    {
                        if (loReader != null)
                        {
                            loReader.Read();
                            loItem = LoadObject(loReader);
                        }
                        loReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loItem;
        }

        /// <summary>
        /// Insert  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>
        public bool Insert(ExportDataEN argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            argEn.InterfaceID = GetAutoNumber("IT");
            string sqlCmd = "Select count(*) as cnt From SAS_ExportData WHERE InterfaceID = '"+argEn.InterfaceID+ "'";
            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database loDbSel = DatabaseFactory.CreateDatabase(csConnectionStr);
                using (DbCommand cmdSel = loDbSel.GetSqlStringCommand(sqlCmd))
                {
                    using (IDataReader dr = loDbSel.ExecuteReader(cmdSel))
                    {
                        if (dr.Read())
                            iOut = GetValue<int>(dr, "cnt");
                        if (iOut > 0)
                            throw new Exception("Insertion Failed! Record Already Exist!");
                    }
                    if (iOut == 0)
                    {

                        sqlCmd = "INSERT INTO SAS_ExportData(InterfaceID,FileFormat,Interface,Frequency,TimeofExport,Filepath,PreviousData,DateRange,DateFrom,DateTo,LastUpdatedBy,LastUpdatedDateTime) VALUES (@InterfaceID,@FileFormat,@Interface,@Frequency,@TimeofExport,@Filepath,@PreviousData,@DateRange,@DateFrom,@DateTo,@LastUpdatedBy,@LastUpdatedDateTime) ";
                        Microsoft.Practices.EnterpriseLibrary.Data.Database loDbIns = DatabaseFactory.CreateDatabase(csConnectionStr);
                        using (DbCommand cmd = loDbIns.GetSqlStringCommand(sqlCmd))
                        {
                            loDbIns.AddInParameter(cmd, "@InterfaceID", DbType.String, argEn.InterfaceID);
                            loDbIns.AddInParameter(cmd, "@FileFormat", DbType.String, argEn.FileFormat);
                            loDbIns.AddInParameter(cmd, "@Interface", DbType.String, argEn.Interface);
                            loDbIns.AddInParameter(cmd, "@Frequency", DbType.String, argEn.Frequency);
                            loDbIns.AddInParameter(cmd, "@TimeofExport", DbType.String, argEn.TimeofExport);
                            loDbIns.AddInParameter(cmd, "@Filepath", DbType.String, argEn.Filepath);
                            loDbIns.AddInParameter(cmd, "@PreviousData", DbType.Boolean, argEn.PreviousData);
                            loDbIns.AddInParameter(cmd, "@DateRange", DbType.Boolean, argEn.DateRange);
                            loDbIns.AddInParameter(cmd, "@DateFrom", DbType.DateTime, argEn.DateFrom);
                            loDbIns.AddInParameter(cmd, "@DateTo", DbType.DateTime, argEn.DateTo);
                            loDbIns.AddInParameter(cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
                            loDbIns.AddInParameter(cmd, "@LastUpdatedDateTime", DbType.DateTime, argEn.LastUpdatedDateTime);
                            int liRowAffected = loDbIns.ExecuteNonQuery(cmd);
                            if (liRowAffected > -1)
                            {
                                System.Messaging.Message mm = new System.Messaging.Message(argEn, new System.Messaging.XmlMessageFormatter(new Type[] { typeof(ExportDataEN), typeof(string) }));
                                mm.Label = argEn.InterfaceID;
                                MessageQueueTransaction Transaction = new MessageQueueTransaction();
                                Transaction.Begin();
                                     mq.Send(mm, Transaction);
                                     Transaction.Commit();
                                lbRes = true;
                            }
                            else
                                throw new Exception("Insertion Failed! No Row has been updated...");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        /// <summary>
        /// Update  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>
        public bool Update(ExportDataEN argEn)
        {
            bool lbRes = false;
            int iOut = 0;
            string sqlCmd = "Select count(*) as cnt From SAS_ExportData WHERE InterfaceID = '"+argEn.InterfaceID+ "'";
            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database loDbSel = DatabaseFactory.CreateDatabase(csConnectionStr);
                using (DbCommand cmdSel = loDbSel.GetSqlStringCommand(sqlCmd))
                {
                    using (IDataReader dr = loDbSel.ExecuteReader(cmdSel))
                    {
                        if (dr.Read())
                            iOut = GetValue<int>(dr, "cnt");
                        if (iOut < 0)
                            throw new Exception("Update Failed! No Record Exist!");
                    }
                    if (iOut != 0)
                    {
                        sqlCmd = "UPDATE SAS_ExportData SET InterfaceID = @InterfaceID, FileFormat = @FileFormat, Interface = @Interface, Frequency = @Frequency, TimeofExport = @TimeofExport, Filepath = @Filepath, PreviousData = @PreviousData,DateRange = @DateRange, DateFrom = @DateFrom, DateTo = @DateTo, LastUpdatedBy = @LastUpdatedBy, LastUpdatedDateTime = @LastUpdatedDateTime WHERE InterfaceID = @InterfaceID";
                        Microsoft.Practices.EnterpriseLibrary.Data.Database loDbUpd = DatabaseFactory.CreateDatabase(csConnectionStr);
                        using (DbCommand cmd = loDbUpd.GetSqlStringCommand(sqlCmd))
                        {
                            loDbUpd.AddInParameter(cmd, "@InterfaceID", DbType.String, argEn.InterfaceID);
                            loDbUpd.AddInParameter(cmd, "@FileFormat", DbType.String, argEn.FileFormat);
                            loDbUpd.AddInParameter(cmd, "@Interface", DbType.String, argEn.Interface);
                            loDbUpd.AddInParameter(cmd, "@Frequency", DbType.String, argEn.Frequency);
                            loDbUpd.AddInParameter(cmd, "@TimeofExport", DbType.String, argEn.TimeofExport);
                            loDbUpd.AddInParameter(cmd, "@Filepath", DbType.String, argEn.Filepath);
                            loDbUpd.AddInParameter(cmd, "@PreviousData", DbType.Boolean, argEn.PreviousData);
                            loDbUpd.AddInParameter(cmd, "@DateRange", DbType.Boolean, argEn.DateRange);
                            loDbUpd.AddInParameter(cmd, "@DateFrom", DbType.DateTime, argEn.DateFrom);
                            loDbUpd.AddInParameter(cmd, "@DateTo", DbType.DateTime, argEn.DateTo);
                            loDbUpd.AddInParameter(cmd, "@LastUpdatedBy", DbType.String, argEn.LastUpdatedBy);
                            loDbUpd.AddInParameter(cmd, "@LastUpdatedDateTime", DbType.DateTime, argEn.LastUpdatedDateTime);
                            int liRowAffected = loDbUpd.ExecuteNonQuery(cmd);
                            if (liRowAffected > -1)
                            {
                                System.Messaging.Message mm = new System.Messaging.Message(argEn, new System.Messaging.XmlMessageFormatter(new Type[] { typeof(ExportDataEN), typeof(string) }));
                                mm.Label = argEn.InterfaceID;
                                MessageQueueTransaction Transaction = new MessageQueueTransaction();
                                Transaction.Begin();
                                mq.Send(mm, Transaction);
                                Transaction.Commit();
                                lbRes = true;
                            }
                            else
                                throw new Exception("Update Failed! No Row has been updated...");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        /// <summary>
        /// Delete  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>
        public bool Delete(ExportDataEN argEn)
        {
            bool lbRes = false;
            string sqlCmd = "DELETE FROM SAS_ExportData WHERE InterfaceID = @InterfaceID";
            Microsoft.Practices.EnterpriseLibrary.Data.Database coDb = DatabaseFactory.CreateDatabase(csConnectionStr);
            try
            {
                using (DbCommand cmd = coDb.GetSqlStringCommand(sqlCmd))
                {
                    coDb.AddInParameter(cmd, "@InterfaceID", DbType.String, argEn.InterfaceID);
                    int liRowAffected = coDb.ExecuteNonQuery(cmd);
                    if (liRowAffected > -1)
                        lbRes = true;
                    else
                        throw new Exception("Deletion Failed! No Row has been deleted...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lbRes;
        }

        /// <summary>
        /// Load  SAS_ExportData Data...
        /// <summary>
        /// <param name=sender></param>
        /// <param name= e></param>
        private ExportDataEN LoadObject(IDataReader argReader)
        {
            ExportDataEN loItem = new ExportDataEN();
            loItem.InterfaceID = GetValue<string>(argReader, "InterfaceID");
            loItem.FileFormat = GetValue<string>(argReader, "FileFormat");
            loItem.Interface = GetValue<string>(argReader, "Interface");
            loItem.Frequency = GetValue<string>(argReader, "Frequency");
            loItem.TimeofExport = GetValue<string>(argReader, "TimeofExport");
            loItem.Filepath = GetValue<string>(argReader, "Filepath");
            loItem.PreviousData = GetValue<bool>(argReader, "PreviousData");
            loItem.PreviousData = GetValue<bool>(argReader, "DateRange");
            loItem.DateFrom = GetValue<DateTime>(argReader, "DateFrom");
            loItem.DateTo = GetValue<DateTime>(argReader, "DateTo");
            loItem.LastUpdatedBy = GetValue<string>(argReader, "LastUpdatedBy");
            loItem.LastUpdatedDateTime = GetValue<DateTime>(argReader, "LastUpdatedDateTime");

            return loItem;
        }

        private static T GetValue<T>(IDataReader argReader, string argColNm)
        {
            if (!argReader.IsDBNull(argReader.GetOrdinal(argColNm)))
                return (T)argReader.GetValue(argReader.GetOrdinal(argColNm));
            else
                return default(T);
        }

        /// <summary>
        /// Method to Get AutoNumber
        /// </summary>
        /// <param name="Description">Description as Input</param>
        /// <returns>Returns AutoNumber</returns>
        public string GetAutoNumber(string Description)
        {
            string AutoNo = "";
            int CurNo = 0;
            int NoDigit = 0;
            int AutoCode = 0;
            int i = 0;
            string SqlStr;
            SqlStr = "select * from SAS_AutoNumber where SAAN_Des='" + Description + "'";

            try
            {
                Microsoft.Practices.EnterpriseLibrary.Data.Database loDbAut = DatabaseFactory.CreateDatabase(csConnectionStr);
                DbCommand cmd = loDbAut.GetSqlStringCommand(SqlStr);
                IDataReader loReader = loDbAut.ExecuteReader(cmd);

                if (loReader.Read())
                {
                    AutoCode = Convert.ToInt32(loReader["SAAN_Code"]);
                    CurNo = Convert.ToInt32(loReader["SAAN_CurNo"]) + 1;
                    NoDigit = Convert.ToInt32(loReader["SAAN_NoDigit"]);
                    AutoNo = Convert.ToString(loReader["SAAN_Prefix"]);
                    if (CurNo.ToString().Length < NoDigit)
                    {
                        while (i < NoDigit - CurNo.ToString().Length)
                        {
                            AutoNo = AutoNo + "0";
                            i = i + 1;
                        }
                        AutoNo = AutoNo + CurNo;
                    }
                    loReader.Close();

                }

                AutoNumberEn loItem = new AutoNumberEn();
                loItem.SAAN_Code = AutoCode;
                AutoNumberDAL cods = new AutoNumberDAL();
                cods.GetItem(loItem);

                loItem.SAAN_Code = Convert.ToInt32(AutoCode);
                loItem.SAAN_CurNo = CurNo;
                loItem.SAAN_AutoNo = AutoNo;


                cods.Update(loItem);


                return AutoNo;
            }

            catch (Exception ex)
            {
                Console.Write("Error in connection : " + ex.Message);
                return ex.ToString();
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
                //this.logger.LogException(LogLevel.Error, "Serialize Object", ex);                 
                throw ex;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
            return doc.InnerXml;
        }
    }

}
//---------------------------------------------------------------------------------

