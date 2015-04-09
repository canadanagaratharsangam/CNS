using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CNS.Model;
using Microsoft.Office.Interop.Excel;

namespace CNS.ExcelExporter
{
    public class FlatExcelFileExporter
    {
        public void ExportToFlatExcelFile(List<MemberWithContactDetails> members, string name)
        {
            Application l_application = null;
            Workbook l_workbook = null;
            Worksheet l_worksheet = null;

            try
            {
                l_application = new Application();
                string l_spreadsheetTemplateFolderPath  = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + @"\ExcelTemplates\";
                l_workbook =
                    l_application.Workbooks.Open(
                        l_spreadsheetTemplateFolderPath + @"FlatAddressBook.xltx",
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                l_worksheet = (Worksheet) l_workbook.Sheets["Sheet1"];
                l_worksheet.Name = "Contacts";
                
                //TODO: Populate data
                
                l_workbook.SaveAs(
                    l_spreadsheetTemplateFolderPath + name,
                    XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                l_workbook.Close();
            }
            catch (Exception l_ex)
            {
                if (l_workbook != null)
                    l_workbook.Close(false, Type.Missing, Type.Missing);

                throw;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.FinalReleaseComObject(l_workbook);
                Marshal.FinalReleaseComObject(l_application);
            }
        }
    }
}