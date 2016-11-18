using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HTS.SAS.Entities;
using HTS.SAS.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

/// <summary>
/// Summary description for ExportData
/// </summary>
public class ExportDataClass
{
    public static string separator;
    public ExportDataClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    		
		
		#region Methods
    

    /// <summary>
    /// Method to get the ExportDataClass
    /// </summary>
    /// <remarks></remarks>
    public static void GetExportData(ExportDataEN argEn)
     {
         AccountsEn loAccountsEn = new AccountsEn();
         AccountsBAL loAccountsBAL = new AccountsBAL();
         List<AccountsEn> lolist = new List<AccountsEn>();
         loAccountsEn.TransDate = argEn.DateFrom;
         loAccountsEn.BatchDate = argEn.DateTo;
         int i;
         if (argEn.PreviousData == true)
         {
             i = 1;
         }
         else
         {
             i = 0;
         }
         string str1;
         if (argEn.DateRange == true)
         {
             str1 = "True";
         }
         else
         {
            str1 = "False";
         }
         string Str = argEn.Filepath;
         if (argEn.Interface == "CB Payment")
         {
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Payment";
             loAccountsEn.SubCategory = "Refund";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             loAccountsEn.SubType = "Student";
             lolist = loAccountsBAL.GetStCBPaymentExportData(loAccountsEn);
             CreateCSVFromGenericListCBPayment(lolist, Str);

             //Sponsor Payment
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Payment";
             loAccountsEn.SubCategory = "ds";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.SubType = "Sponsor";
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             lolist = loAccountsBAL.GetSponsorCBReciptExportData(loAccountsEn);
             CreateCSVFromGenericListCBPaymentSponsor(lolist, Str);

         }
         else if (argEn.Interface == "CB Receipt")
         {
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Receipt";
             loAccountsEn.SubCategory = "DS";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             loAccountsEn.SubType = "Student";
             lolist = loAccountsBAL.GetStCBPaymentExportData(loAccountsEn);
             CreateCSVFromGenericListCBReceipt(lolist, Str);

             //Sponsor Receipt
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Receipt";
             loAccountsEn.SubCategory = "DS";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.SubType = "Sponsor";
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             lolist = loAccountsBAL.GetSponsorCBReciptExportData(loAccountsEn);
             CreateCSVFromGenericListCBReceiptSponsor(lolist, Str);
         }
         else if (argEn.Interface == "GL Journal")
         {
             loAccountsEn.Category = "Credit Note";
             loAccountsEn.SubCategory = "Debit Note";
             separator = argEn.FileFormat;
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             loAccountsEn.SubType = "Student";
             lolist = loAccountsBAL.GetStGLJournalExportData(loAccountsEn);
             CreateCSVFromGenericListGLJournal(lolist, Str);

             //Sponsor Journal
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Credit Note";
             loAccountsEn.SubCategory = "Debit Note";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.SubType = "Sponsor";
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             lolist = loAccountsBAL.GetSponsorGLJournalExportData(loAccountsEn);
             CreateCSVFromGenericListGLJournalSponsor(lolist, Str);
         }
         else if (argEn.Interface == "All")
         {
             //Payment
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Payment";
             loAccountsEn.SubCategory = "Refund";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.SubType = "Student";
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             lolist = loAccountsBAL.GetStCBPaymentExportData(loAccountsEn);
             CreateCSVFromGenericListCBPayment(lolist, Str);

             //Receipt
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Receipt";
             loAccountsEn.SubCategory = "DS";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.SubType = "Student";
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             lolist = loAccountsBAL.GetStCBPaymentExportData(loAccountsEn);
             CreateCSVFromGenericListCBReceipt(lolist, Str);

             //Journal
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Credit Note";
             loAccountsEn.SubCategory = "Debit Note";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.SubType = "Student";
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             lolist = loAccountsBAL.GetStGLJournalExportData(loAccountsEn);
             CreateCSVFromGenericListGLJournal(lolist, Str);

             //Sponsor Journal
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Credit Note";
             loAccountsEn.SubCategory = "Debit Note";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.SubType = "Sponsor";
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             lolist = loAccountsBAL.GetSponsorGLJournalExportData(loAccountsEn);
             CreateCSVFromGenericListGLJournalSponsor(lolist, Str);

             //Sponsor Receipt
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Receipt";
             loAccountsEn.SubCategory = "DS";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.SubType = "Sponsor";
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             lolist = loAccountsBAL.GetSponsorCBReciptExportData(loAccountsEn);
             CreateCSVFromGenericListCBReceiptSponsor(lolist, Str);

             //Sponsor Payment
             separator = argEn.FileFormat;
             loAccountsEn.Category = "Payment";
             loAccountsEn.SubCategory = "ds";
             loAccountsEn.IntegrationStatus = i;
             loAccountsEn.IntegrationCode = str1;
             loAccountsEn.SubType = "Sponsor";
             loAccountsEn.UpdatedBy = argEn.LastUpdatedBy;
             loAccountsEn.UpdatedTime = argEn.LastUpdatedDateTime;
             lolist = loAccountsBAL.GetSponsorCBReciptExportData(loAccountsEn);
             CreateCSVFromGenericListCBPaymentSponsor(lolist, Str);
         }
        
      
     }

    public static void CreateCSVFromGenericListGLJournal(List<AccountsEn> list, string Str)
     {

         StringBuilder csvString = new StringBuilder();
         List<AccountsDetailsEn> accountdetaillist = new List<AccountsDetailsEn>();
         List<AccountsEn> accountlist = new List<AccountsEn>();
         List<AccountsEn> finalaccountlist = new List<AccountsEn>();
        
        
         string filePath = Str;

         accountlist = list;

         foreach (AccountsEn loen in accountlist)
         {
             if (loen.Category == "Invoice" || loen.Category == "AFC" || loen.Category == "Debit Note")
             {
                 csvString.Append(loen.BatchCode.ToString());
                 csvString.Append(separator);
                 csvString.Append(loen.BatchDate.ToString("dd/MM/yyyy"));
                 csvString.Append(separator);
                 if (loen.Category == "Invoice")
                 {
                     csvString.Append("Inv -" + loen.Description.ToString());
                     csvString.Append(separator);

                 }
                 else if (loen.Category == "AFC")
                 {
                     csvString.Append("AFC -" + loen.Description.ToString());
                     csvString.Append(separator);

                 }
                 else if (loen.Category == "Debit Note")
                 {
                     csvString.Append("DN -" + loen.Description.ToString());
                     csvString.Append(separator);
                 }
                 csvString.Append(loen.ProgramInfo.Accountinfo.ToString());
                 csvString.Append(separator);
                 csvString.Append(" ");
                 csvString.Append(separator);
                 csvString.Append(loen.TransactionAmount.ToString());
                 csvString.Append(separator);
                 csvString.Append("R");
                 csvString.Append(separator);

                 csvString.AppendLine();

                 csvString.Append(loen.BatchCode.ToString());
                 csvString.Append(separator);
                 csvString.Append(loen.BatchDate.ToString("dd/MM/yyyy"));
                 csvString.Append(separator);
                 if (loen.Category == "Invoice")
                 {
                     csvString.Append("Inv -" + loen.Description.ToString());
                     csvString.Append(separator);

                 }
                 else if (loen.Category == "AFC")
                 {
                     csvString.Append("AFC -" + loen.Description.ToString());
                     csvString.Append(separator);

                 }
                 else if (loen.Category == "Debit Note")
                 {
                     csvString.Append("DN -" + loen.Description.ToString());
                     csvString.Append(separator);
                 }
                 csvString.Append(loen.ProgramInfo.Tutionacc.ToString());
                 csvString.Append(separator);
                 csvString.Append(" ");
                 csvString.Append(separator);
                 csvString.Append("-"+loen.TransactionAmount.ToString());
                 csvString.Append(separator);
                 csvString.Append("R");
                 csvString.Append(separator);

             }
            if (loen.Category == "Credit Note")
             {
                 csvString.Append(loen.BatchCode.ToString());
                 csvString.Append(separator);
                 csvString.Append(loen.BatchDate.ToString("dd/MM/yyyy"));
                 csvString.Append(separator);
                 csvString.Append("CN -" + loen.Description.ToString());
                 csvString.Append(separator);
                 csvString.Append(loen.ProgramInfo.Tutionacc.ToString());
                 csvString.Append(separator);
                 csvString.Append(" ");
                 csvString.Append(separator);
                 csvString.Append(loen.TransactionAmount.ToString());
                 csvString.Append(separator);
                 csvString.Append("R");
                 csvString.Append(separator);

                 csvString.AppendLine();

                 csvString.Append(loen.BatchCode.ToString());
                 csvString.Append(separator);
                 csvString.Append(loen.BatchDate.ToString("dd/MM/yyyy"));
                 csvString.Append(separator);
                 csvString.Append("CN -" + loen.Description.ToString());
                 csvString.Append(separator);
                 csvString.Append(loen.ProgramInfo.Accountinfo.ToString());
                 csvString.Append(separator);
                 csvString.Append(" ");
                 csvString.Append(separator);
                 csvString.Append("-"+loen.TransactionAmount.ToString());
                 csvString.Append(separator);
                 csvString.Append("R");
                 csvString.Append(separator);
             }
             //Next Line
             csvString.AppendLine();
             }
            
          
             //Next Line
             csvString.AppendLine();
     
       
        
         try 
         {
             string FileExt;
             FileExt = System.Configuration.ConfigurationSettings.AppSettings["ExportDataStudent-GL"];
             string Name;
             string path = "";
             string deli = "";
             string[] filepat = filePath.Split('\\');
             for (int z = 0; z < filepat.Length - 1; z++)
             {
                 deli = "\\";
                 path = path + filepat[z] + deli;
             }
             Name = filepat[filepat.Length - 1];
             string[] na = Name.Split('.');

             string fullFileName = System.IO.Path.Combine(path, na[0] + DateTime.Now.ToString("yyyyMMdd") + FileExt + ".txt");
             if (System.IO.File.Exists(fullFileName))
             {
                 int counter = 0;
                 do
                 {
                     counter = counter + 1;
                     fullFileName = System.IO.Path.Combine(path, na[0] + DateTime.Now.ToString("yyyyMMdd") + "(" + counter + ")" + FileExt + ".txt");
                 }
                 while (System.IO.File.Exists(fullFileName));
             }
             StreamWriter streamWriter = new StreamWriter(fullFileName);
             streamWriter.Write(csvString.ToString());
             streamWriter.Close();
         }
         catch (Exception ex) 
         {
             LogError.Log("ExportDataClass", "CreateCSVFromGenericListAPInvoice", ex.Message);
         }
        
        
     }

    public static void CreateCSVFromGenericListGLJournalSponsor(List<AccountsEn> list, string Str)
    {

        StringBuilder csvString = new StringBuilder();
        List<AccountsDetailsEn> accountdetaillist = new List<AccountsDetailsEn>();
        List<AccountsEn> accountlist = new List<AccountsEn>();
        List<AccountsEn> finalaccountlist = new List<AccountsEn>();


        string filePath = Str;

        accountlist = list;

        foreach (AccountsEn loen in accountlist)
        {
            if (loen.Category == "Debit Note")
            {
                if (loen.AccList.Count > 0)
                {
                    foreach (AccountsEn loaccen in loen.AccList)
                    {
                        csvString.Append(loaccen.BatchCode.ToString());
                        csvString.Append(separator);
                        csvString.Append(loaccen.BatchDate.ToString("dd/MM/yyyy"));
                        csvString.Append(separator);
                        csvString.Append("DN -" + loaccen.Description.ToString());
                        csvString.Append(separator);
                        csvString.Append(loaccen.Sponsor.GLAccount.ToString());
                        csvString.Append(separator);
                        csvString.Append(" ");
                        csvString.Append(separator);
                        csvString.Append(loaccen.TransactionAmount.ToString());
                        csvString.Append(separator);
                        csvString.Append("R");
                        csvString.Append(separator);

                        csvString.AppendLine();

                        csvString.Append(loaccen.BatchCode.ToString());
                        csvString.Append(separator);
                        csvString.Append(loaccen.BatchDate.ToString("dd/MM/yyyy"));
                        csvString.Append(separator);
                        csvString.Append("DN -" + loaccen.Description.ToString());
                        csvString.Append(separator);
                        csvString.Append(loaccen.Sponsor.GLAccount.ToString());
                        csvString.Append(separator);
                        csvString.Append(" ");
                        csvString.Append(separator);
                        csvString.Append("-" + loaccen.TransactionAmount.ToString());
                        csvString.Append(separator);
                        csvString.Append("R");
                        csvString.Append(separator);
                    }
                }
            }
            else if (loen.Category == "Credit Note")
            {
                if (loen.AccList.Count > 0)
                {
                    foreach (AccountsEn loaccen in loen.AccList)
                    {
                        csvString.Append(loaccen.BatchCode.ToString());
                        csvString.Append(separator);
                        csvString.Append(loaccen.BatchDate.ToString("dd/MM/yyyy"));
                        csvString.Append(separator);
                        csvString.Append("CN -" + loaccen.Description.ToString());
                        csvString.Append(separator);
                        csvString.Append(loaccen.Sponsor.GLAccount.ToString());
                        csvString.Append(separator);
                        csvString.Append(" ");
                        csvString.Append(separator);
                        csvString.Append(loaccen.TransactionAmount.ToString());
                        csvString.Append(separator);
                        csvString.Append("R");
                        csvString.Append(separator);

                        csvString.AppendLine();

                        csvString.Append(loaccen.BatchCode.ToString());
                        csvString.Append(separator);
                        csvString.Append(loaccen.BatchDate.ToString("dd/MM/yyyy"));
                        csvString.Append(separator);
                        csvString.Append("CN -" + loaccen.Description.ToString());
                        csvString.Append(separator);
                        csvString.Append(loaccen.Sponsor.GLAccount.ToString());
                        csvString.Append(separator);
                        csvString.Append(" ");
                        csvString.Append(separator);
                        csvString.Append("-" + loaccen.TransactionAmount.ToString());
                        csvString.Append(separator);
                        csvString.Append("R");
                        csvString.Append(separator);
                    }
                }
            }
            else if (loen.Category == "Allocation")
            {
                if (loen.AccList.Count > 0)
                {
                    foreach (AccountsEn loaccen in loen.AccList)
                    {

                                csvString.Append(loaccen.BatchCode.ToString());
                                csvString.Append(separator);
                                csvString.Append(loaccen.BatchDate.ToString("dd/MM/yyyy"));
                                csvString.Append(separator);
                                csvString.Append("Al -" + loaccen.Description.ToString());
                                csvString.Append(separator);
                                csvString.Append(loaccen.Sponsor.GLAccount.ToString());
                                csvString.Append(separator);
                                csvString.Append(" ");
                                csvString.Append(separator);
                                csvString.Append(loaccen.TransactionAmount.ToString());
                                csvString.Append(separator);
                                csvString.Append("R");
                                csvString.Append(separator);

                                csvString.AppendLine();

                                double totalpocketamt = 0.0;
                                if (loaccen.AccList.Count > 0)
                                {
                                    foreach (AccountsEn loacstcen in loaccen.AccList)
                                    {

                                        csvString.Append(loaccen.BatchCode.ToString());
                                        csvString.Append(separator);
                                        csvString.Append(loaccen.BatchDate.ToString("dd/MM/yyyy"));
                                        csvString.Append(separator);
                                        csvString.Append("Al -" + loaccen.Description.ToString());
                                        csvString.Append(separator);
                                        csvString.Append(loacstcen.Student.Programen.Accountinfo.ToString());
                                        csvString.Append(separator);
                                        csvString.Append(" ");
                                        csvString.Append(separator);
                                        csvString.Append("-" + loacstcen.Student.AccountDetailsEn.TransactionAmount.ToString());
                                        csvString.Append(separator);
                                        csvString.Append("R");
                                        csvString.Append(separator);
                                        csvString.AppendLine();
                                        totalpocketamt += loacstcen.Student.AccountDetailsEn.TempAmount;
                                    }
                                }

                    }
                }

            }
            //Next Line
            csvString.AppendLine();
        }


        //Next Line
        csvString.AppendLine();



        try
        {
            string FileExt;
            FileExt = System.Configuration.ConfigurationSettings.AppSettings["ExportDataSponsor-GL"];
            string Name;
            string path = "";
            string deli = "";
            string[] filepat = filePath.Split('\\');
            for (int z = 0; z < filepat.Length - 1; z++)
            {
                deli = "\\";
                path = path + filepat[z] + deli;
            }
            Name = filepat[filepat.Length - 1];
            string[] na = Name.Split('.');

            string fullFileName = System.IO.Path.Combine(path, na[0] + DateTime.Now.ToString("yyyyMMdd") + FileExt + ".txt");
            if (System.IO.File.Exists(fullFileName))
            {
                int counter = 0;
                do
                {
                    counter = counter + 1;
                    fullFileName = System.IO.Path.Combine(path, na[0] + DateTime.Now.ToString("yyyyMMdd") + "(" + counter + ")" + FileExt + ".txt");
                }
                while (System.IO.File.Exists(fullFileName));
            }
            StreamWriter streamWriter = new StreamWriter(fullFileName);
            streamWriter.Write(csvString.ToString());
            streamWriter.Close();
        }
        catch (Exception ex)
        {
            LogError.Log("ExportDataClass", "CreateCSVFromGenericListGLJournalSponsor", ex.Message);
        }


    }
    public static void CreateCSVFromGenericListCBPaymentSponsor(List<AccountsEn> list, string Str)
    {

        StringBuilder csvString = new StringBuilder();
        List<AccountsDetailsEn> accountdetaillist = new List<AccountsDetailsEn>();
        List<AccountsEn> accountlist = new List<AccountsEn>();
        List<AccountsEn> finalaccountlist = new List<AccountsEn>();
        String COMPANYCODE = System.Configuration.ConfigurationSettings.AppSettings["CompanyCode"];

        string filePath = Str;

        accountlist = list;
        if (accountlist != null)
        {
            if (accountlist.Count != 0)
            {

                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                List<AccountsEn> lohtlist = new List<AccountsEn>();
                foreach (AccountsEn loen in accountlist)
                {
                    if (!ht.Contains(loen.BatchCode))
                    {

                        ht.Add(loen.BatchCode, loen);
                        lohtlist.Add(loen);
                    }
                }
                for (int i = 0; i < lohtlist.Count; i++)
                {
                    List<AccountsEn> uniquelist = accountlist.FindAll(delegate(AccountsEn p) { return p.BatchCode == lohtlist[i].BatchCode; });
                    int intsta = 1;
                    foreach (AccountsEn loen in uniquelist)
                    {

                        loen.IntegrationStatus = intsta;
                        finalaccountlist.Add(loen);
                        intsta = intsta + 1;
                    }
                }

            }
        }

        foreach (AccountsEn loen in finalaccountlist)
        {

            csvString.Append(loen.BatchCode.ToString());
            csvString.Append(separator);
            csvString.Append(loen.BankCode.ToString());
            csvString.Append(separator);
            csvString.Append(COMPANYCODE);
            csvString.Append(separator);
            csvString.Append("CHQ");
            csvString.Append(separator);
            csvString.Append("READY");
            csvString.Append(separator);
            csvString.Append(loen.BatchDate.ToString("dd/MM/yyyy"));
            csvString.Append(separator);
            csvString.Append("EXT");
            csvString.Append(separator);
            csvString.Append("MYR");
            csvString.Append(separator);
            csvString.Append("1.0");
            csvString.Append(separator);
            csvString.Append(loen.IntegrationStatus.ToString());
            csvString.Append(separator);
            csvString.Append(loen.TransactionCode.ToString());
            csvString.Append(separator);
            csvString.Append(loen.CreditRefOne.ToString());
            csvString.Append(separator);
            csvString.Append(loen.CreditRef.ToString());
            csvString.Append(separator);
            csvString.Append(loen.Sponsor.Address + " " + loen.Sponsor.Address1);
            csvString.Append(separator);
            csvString.Append(loen.Sponsor.Address2.ToString());
            csvString.Append(separator);
            csvString.Append(loen.Sponsor.Contact.ToString());
            csvString.Append(separator);
            csvString.Append(loen.Sponsor.Phone.ToString());
            csvString.Append(separator);
            csvString.Append(loen.TransactionAmount.ToString());
            csvString.Append(separator);
            csvString.Append(loen.TransDate.ToString("dd/MM/yyyy"));
            csvString.Append(separator);
            csvString.Append(loen.CreditRef.ToString());
            csvString.Append(separator);
            csvString.Append(loen.Description.ToString());
            csvString.Append(separator);
            csvString.Append(loen.TransactionAmount.ToString());
            csvString.Append(separator);
            csvString.Append("0.0");
            csvString.Append(separator);
            csvString.Append("GL");
            csvString.Append(separator);
            csvString.Append(loen.Sponsor.GLAccount.ToString());
            csvString.Append(separator);
            csvString.Append(loen.TransactionAmount.ToString());
            csvString.Append(separator);
            //Next Line
            csvString.AppendLine();

            }

        try
        {
            string FileExt;
            FileExt = System.Configuration.ConfigurationSettings.AppSettings["ExportDataSponsor-CBP"];
            string Name;
            string path = "";
            string deli = "";
            string[] filepat = filePath.Split('\\');
            for (int z = 0; z < filepat.Length - 1; z++)
            {
                deli = "\\";
                path = path + filepat[z] + deli;
            }
            Name = filepat[filepat.Length - 1];
            string[] na = Name.Split('.');

            string fullFileName = System.IO.Path.Combine(@"\\windev\c$\exportSAS", na[0] + DateTime.Now.ToString("yyyyMMdd") + FileExt + ".txt");
            if (System.IO.File.Exists(fullFileName))
            {
                int counter = 0;
                do
                {
                    counter = counter + 1;
                    fullFileName = System.IO.Path.Combine(@"\\windev\c$\exportSAS", na[0] + DateTime.Now.ToString("yyyyMMdd") + "(" + counter + ")" + FileExt + ".txt");
                }
                while (System.IO.File.Exists(fullFileName));
            }
            StreamWriter streamWriter = new StreamWriter(fullFileName);
            streamWriter.Write(csvString.ToString());
            streamWriter.Close();
        }
        catch (Exception ex)
        {
            LogError.Log("ExportDataClass", "CreateCSVFromGenericListCBPaymentSponsor", ex.Message);
        }


    }
        public static void CreateCSVFromGenericListCBReceiptSponsor(List<AccountsEn> list, string Str)
    {

        StringBuilder csvString = new StringBuilder();
        List<AccountsDetailsEn> accountdetaillist = new List<AccountsDetailsEn>();
        List<AccountsEn> accountlist = new List<AccountsEn>();
        List<AccountsEn> finalaccountlist = new List<AccountsEn>();


        string filePath = Str;

        accountlist = list;
        if (accountlist != null)
        {
            if (accountlist.Count != 0)
            {

                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                List<AccountsEn> lohtlist = new List<AccountsEn>();
                foreach (AccountsEn loen in accountlist)
                {
                    if (!ht.Contains(loen.BatchCode))
                    {

                        ht.Add(loen.BatchCode, loen);
                        lohtlist.Add(loen);
                    }
                }
                for (int i = 0; i < lohtlist.Count; i++)
                {
                    List<AccountsEn> uniquelist = accountlist.FindAll(delegate(AccountsEn p) { return p.BatchCode == lohtlist[i].BatchCode; });
                    int intsta = 1;
                    foreach (AccountsEn loen in uniquelist)
                    {

                        loen.IntegrationStatus = intsta;
                        finalaccountlist.Add(loen);
                        intsta = intsta + 1;
                    }
                }

            }
        }

                   foreach (AccountsEn loaccen in accountlist)
                   {
                      
                       csvString.Append(loaccen.BatchCode.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.BankCode.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.BatchDate.ToString("dd/MM/yyyy"));
                       csvString.Append(separator);
                       csvString.Append(loaccen.IntegrationStatus.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.TransactionCode.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.CreditRef.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.Description.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.CreditRef.ToString());
                       csvString.Append(separator);
                       csvString.Append("CSH");
                       csvString.Append(separator);
                       csvString.Append("-");
                       csvString.Append(separator);
                       csvString.Append("-");
                       csvString.Append(separator);
                       csvString.Append(loaccen.TransactionAmount.ToString());
                       csvString.Append(separator);
                       csvString.Append("GL");
                       csvString.Append(separator);
                       csvString.Append(loaccen.Sponsor.GLAccount.ToString());
                       csvString.Append(separator);
                       csvString.Append("-"+loaccen.TransactionAmount.ToString());
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append("R");
                       csvString.Append(separator);
                       //Next Line
                       csvString.AppendLine();

            }
            //Next Line
            csvString.AppendLine();



        try
        {
            string FileExt;
            FileExt = System.Configuration.ConfigurationSettings.AppSettings["ExportDataSponsor-CBR"];
            string Name;
            string path = "";
            string deli = "";
            string[] filepat = filePath.Split('\\');
            for (int z = 0; z < filepat.Length - 1; z++)
            {
                deli = "\\";
                path = path + filepat[z] + deli;
            }
            Name = filepat[filepat.Length - 1];
            string[] na = Name.Split('.');

            string fullFileName = System.IO.Path.Combine(path, na[0] + DateTime.Now.ToString("yyyyMMdd") + FileExt + ".txt");
            if (System.IO.File.Exists(fullFileName))
            {
                int counter = 0;
                do
                {
                    counter = counter + 1;
                    fullFileName = System.IO.Path.Combine(path, na[0] + DateTime.Now.ToString("yyyyMMdd") + "(" + counter + ")" + FileExt + ".txt");
                }
                while (System.IO.File.Exists(fullFileName));
            }
            StreamWriter streamWriter = new StreamWriter(fullFileName);
            streamWriter.Write(csvString.ToString());
            streamWriter.Close();
        }
        catch (Exception ex)
        {
            LogError.Log("ExportDataClass", "CreateCSVFromGenericListCBPaymentSponsor", ex.Message);
        }


    }
    // public  string  GetFileName(string path, string @base, string extension)
    //{
    //    string fullFileName = System.IO.Path.Combine(path, @base + extension);
    //    if (System.IO.File.Exists(fullFileName))
    //    {
    //        int counter = 0;
    //        do
    //        {
    //            counter = counter + 1;
    //            fullFileName = System.IO.Path.Combine(path, @base + "(" + counter + ")" + extension);
    //        }
    //        while (System.IO.File.Exists(fullFileName));
    //    }
    //    return fullFileName;
    // }


    public static void CreateCSVFromGenericListCBReceipt(List<AccountsEn> list, string Str)
    {

        StringBuilder csvString = new StringBuilder();
        List<AccountsDetailsEn> accountdetaillist = new List<AccountsDetailsEn>();
        List<AccountsEn> accountlist = new List<AccountsEn>();


        string filePath = Str;
        

        accountlist = list;
        foreach (AccountsEn loen in accountlist)
        {
            
             if (loen.AccList.Count > 0)
                {
                    accountlist = new List<AccountsEn>(); 
                   accountlist= loen.AccList;
                   int i = 1;
                   foreach (AccountsEn loaccen in accountlist)
                   {
                      
                       csvString.Append(loaccen.BatchCode.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.BankCode.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.BatchDate.ToString("dd/MM/yyyy"));
                       csvString.Append(separator);
                       csvString.Append(i.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.TransactionCode.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.CreditRef.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.Description.ToString());
                       csvString.Append(separator);
                       csvString.Append(loaccen.CreditRef.ToString());
                       csvString.Append(separator);
                       csvString.Append("CSH");
                       csvString.Append(separator);
                       csvString.Append("-");
                       csvString.Append(separator);
                       csvString.Append("-");
                       csvString.Append(separator);
                       csvString.Append(loaccen.TransactionAmount.ToString());
                       csvString.Append(separator);
                       csvString.Append("GL");
                       csvString.Append(separator);
                       csvString.Append(loaccen.ProgramInfo.Accountinfo.ToString());
                       csvString.Append(separator);
                       csvString.Append("-"+loaccen.TransactionAmount.ToString());
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append(separator);
                       csvString.Append("R");
                       csvString.Append(separator);
                       i = i + 1;
                       //Next Line
                       csvString.AppendLine();
                   }

            }
            //Next Line
            csvString.AppendLine();



        }



        try
        {
            string FileExt;
            FileExt = System.Configuration.ConfigurationSettings.AppSettings["ExportDataStudent-CBR"];
            string Name;
            string path = "";
            string deli = "";
            string[] filepat = filePath.Split('\\');
            for (int z = 0; z < filepat.Length - 1; z++)
            {
                deli = "\\";
                path = path + filepat[z] + deli;
            }
            Name = filepat[filepat.Length - 1];
            string[] na = Name.Split('.');

            string fullFileName = System.IO.Path.Combine(path, na[0] + DateTime.Now.ToString("yyyyMMdd") + FileExt + ".txt");
            if (System.IO.File.Exists(fullFileName))
            {
                int counter = 0;
                do
                {
                    counter = counter + 1;
                    fullFileName = System.IO.Path.Combine(path, na[0] + DateTime.Now.ToString("yyyyMMdd") + "(" + counter + ")" + FileExt + ".txt");
                }
                while (System.IO.File.Exists(fullFileName));
            }
            StreamWriter streamWriter = new StreamWriter(fullFileName);
            streamWriter.Write(csvString.ToString());
            streamWriter.Close();
        }
        catch (Exception ex)
        {
            LogError.Log("ExportDataClass", "CreateCSVFromGenericListCBReceipt", ex.Message);

        }


    }

    public static void CreateCSVFromGenericListCBPayment(List<AccountsEn> list, string Str)
    {

        StringBuilder csvString = new StringBuilder();
        List<AccountsDetailsEn> accountdetaillist = new List<AccountsDetailsEn>();
        List<AccountsEn> accountlist;

        String COMPANYCODE = System.Configuration.ConfigurationSettings.AppSettings["CompanyCode"];
        string filePath = Str;
        int i = 0;

        accountlist = list;
        foreach (AccountsEn loen in accountlist)
        {
            if (loen.Category == "Payment")
            {
                
                if (loen.AccList.Count > 0)
                {
                    AccountsEn myLocatedObject = loen.AccList.Find(delegate(AccountsEn p) { return p.Category == "Payment"; });
                    accountlist = new List<AccountsEn>(); 
                   accountlist= loen.AccList;
                   int payline = 1;
                    foreach (AccountsEn loaccen in accountlist)
                    {
                        if (loaccen.Category == "STA")
                        {
                            csvString.Append(myLocatedObject.BatchCode.ToString());
                            csvString.Append(separator);
                            csvString.Append(myLocatedObject.BankCode.ToString());
                            csvString.Append(separator);
                            csvString.Append(COMPANYCODE);
                            csvString.Append(separator);
                            csvString.Append("CHQ");
                            csvString.Append(separator);
                            csvString.Append("READY");
                            csvString.Append(separator);
                            csvString.Append(myLocatedObject.BatchDate.ToString("dd/MM/yyyy"));
                            csvString.Append(separator);
                            csvString.Append("EXT");
                            csvString.Append(separator);
                            csvString.Append("MYR");
                            csvString.Append(separator);
                            csvString.Append("1.0");
                            csvString.Append(separator);
                            csvString.Append(payline.ToString());
                            csvString.Append(separator);
                            csvString.Append(" ");
                            csvString.Append(separator);
                            csvString.Append(loaccen.VoucherNo.ToString());
                            csvString.Append(separator);
                            csvString.Append(loaccen.CreditRef.ToString());
                            csvString.Append(separator);
                            csvString.Append(loaccen.Student.SASI_Add1 + " " + loaccen.Student.SASI_Add2 + " " + loaccen.Student.SASI_Add3);
                            csvString.Append(separator);
                            csvString.Append(loaccen.Student.SASI_City + " " + loaccen.Student.SASI_State + " " + loaccen.Student.SASI_Country);
                            csvString.Append(separator);
                            csvString.Append(loaccen.Student.SASI_Country);
                            csvString.Append(separator);
                            csvString.Append(loaccen.Student.SASI_Postcode);
                            csvString.Append(separator);
                            csvString.Append(loaccen.TransactionAmount.ToString());
                            csvString.Append(separator);
                            csvString.Append(loaccen.TransDate.ToString("dd/MM/yyyy"));
                            csvString.Append(separator);
                            csvString.Append(loaccen.CreditRef.ToString());
                            csvString.Append(separator);
                            csvString.Append(myLocatedObject.Description.ToString());
                            csvString.Append(separator);
                            csvString.Append(loaccen.TransactionAmount.ToString());
                            csvString.Append(separator);
                            csvString.Append("0.0");
                            csvString.Append(separator);
                            csvString.Append("GL");
                            csvString.Append(separator);
                            csvString.Append(loaccen.ProgramInfo.Accountinfo.ToString());
                            csvString.Append(separator);
                            csvString.Append(loaccen.TransactionAmount.ToString());
                            csvString.Append(separator);
                            //Next Line
                            csvString.AppendLine();
                            payline = payline + 1;

                        }
                    }

                }
            }
            else if (loen.Category == "Refund")
            {
                accountlist = new List<AccountsEn>();
                accountlist = loen.AccList;
                int rdline = 0;

                foreach (AccountsEn loaccen in accountlist)
                {
                    csvString.Append(loaccen.BatchCode.ToString());
                    csvString.Append(separator);
                    csvString.Append(loaccen.BankCode.ToString());
                    csvString.Append(separator);
                    csvString.Append(COMPANYCODE);
                    csvString.Append(separator);
                    csvString.Append("CHQ");
                    csvString.Append(separator);
                    csvString.Append("READY");
                    csvString.Append(separator);
                    csvString.Append(loaccen.BatchDate.ToString("dd/MM/yyyy"));
                    csvString.Append(separator);
                    csvString.Append("EXT");
                    csvString.Append(separator);
                    csvString.Append("MYR");
                    csvString.Append(separator);
                    csvString.Append("1.0");
                    csvString.Append(separator);
                    csvString.Append(rdline.ToString());
                    csvString.Append(separator);
                    csvString.Append(" ");
                    csvString.Append(separator);
                    csvString.Append(loaccen.VoucherNo.ToString());
                    csvString.Append(separator);
                    csvString.Append(loaccen.CreditRef.ToString());
                    csvString.Append(separator);
                    csvString.Append(loaccen.Student.SASI_Add1 + " " + loaccen.Student.SASI_Add2 + " " + loaccen.Student.SASI_Add3);
                    csvString.Append(separator);
                    csvString.Append(loaccen.Student.SASI_City + " " + loaccen.Student.SASI_State);
                    csvString.Append(separator);
                    csvString.Append(loaccen.Student.SASI_Country);
                    csvString.Append(separator);
                    csvString.Append(loaccen.Student.SASI_Postcode);
                    csvString.Append(separator);
                    csvString.Append(loaccen.TransactionAmount.ToString());
                    csvString.Append(separator);
                    csvString.Append(loaccen.TransDate.ToString("dd/MM/yyyy"));
                    csvString.Append(separator);
                    csvString.Append(loaccen.CreditRef.ToString());
                    csvString.Append(separator);
                    csvString.Append(loaccen.Description.ToString());
                    csvString.Append(separator);
                    csvString.Append(loaccen.TransactionAmount.ToString());
                    csvString.Append(separator);
                    csvString.Append("0.0");
                    csvString.Append(separator);
                    csvString.Append("GL");
                    csvString.Append(separator);
                    csvString.Append(loaccen.ProgramInfo.Accountinfo.ToString());
                    csvString.Append(separator);
                    csvString.Append(loaccen.TransactionAmount.ToString());
                    csvString.Append(separator);
                    //Next Line
                    csvString.AppendLine();
                    rdline = rdline + 1;
                }
            }
        }

        try
        {
            string FileExt;
            FileExt = System.Configuration.ConfigurationSettings.AppSettings["ExportDataStudent-CBP"];
            string Name;
            string path = "";
            string deli = "";
            string[] filepat = filePath.Split('\\');
            for (int z = 0; z < filepat.Length - 1; z++)
            {
                deli = "\\";
                path = path + filepat[z] + deli;
            }
            Name = filepat[filepat.Length - 1];
            string[] na = Name.Split('.');

            string fullFileName = System.IO.Path.Combine(@"\\windev\c$\exportSAS", na[0] + DateTime.Now.ToString("yyyyMMdd") + FileExt + ".txt");
            if (System.IO.File.Exists(fullFileName))
            {
                int counter = 0;
                do
                {
                    counter = counter + 1;
                    fullFileName = System.IO.Path.Combine(@"\\windev\c$\exportSAS", na[0] + DateTime.Now.ToString("yyyyMMdd") + "(" + counter + ")" + FileExt + ".txt");
                }
                while (System.IO.File.Exists(fullFileName));
            }
            StreamWriter streamWriter = new StreamWriter(fullFileName);
            streamWriter.Write(csvString.ToString());
            streamWriter.Close();
        }
        catch (Exception ex)
        {
            LogError.Log("ExportDataClass", "CreateCSVFromGenericListCBPayment", ex.Message);
        }


    }

        #endregion
}
