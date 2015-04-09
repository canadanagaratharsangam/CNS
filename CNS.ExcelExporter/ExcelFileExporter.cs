using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CNS.DataAccessLayer.Sqlite;
using CNS.Model;
using Microsoft.Office.Interop.Excel;

namespace CNS.ExcelExporter
{
    public class ExcelFileExporter
    {
        public void ExportToFlatExcelFile(List<MemberWithContactDetails> members, string name)
        {
            Application l_application = null;
            Workbook l_workbook = null;
            Worksheet l_worksheet = null;

            try
            {
                l_application = new Application();
                string l_spreadsheetTemplateFolderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + @"\ExcelTemplates\";
                l_workbook =
                    l_application.Workbooks.Open(
                        l_spreadsheetTemplateFolderPath + @"FlatAddressBook.xltx",
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                l_worksheet = (Worksheet)l_workbook.Sheets["Contacts"];
                l_worksheet.Name = "Contacts";
                int l_rowNumber = 2;

                foreach (MemberWithContactDetails l_contactDetails in members)
                {
                    string l_firstName = l_contactDetails.Contact.first_name;
                    string l_lastName = l_contactDetails.Contact.last_name ?? "";
                    string l_email = l_contactDetails.Contact.email_address ?? "";
                    string l_address = String.Empty;
                    string l_phones = String.Empty;
                    Address l_contactAddress = l_contactDetails.ContactAddress;
                    if (l_contactAddress != null)
                    {
                        StringBuilder l_addressBuilder = new StringBuilder();
                        if (!String.IsNullOrWhiteSpace(l_contactAddress.address_line_1))
                        {
                            l_addressBuilder.AppendLine(l_contactAddress.address_line_1);
                        }
                        if (!String.IsNullOrWhiteSpace(l_contactAddress.address_line_2))
                        {
                            l_addressBuilder.AppendLine(l_contactAddress.address_line_2);
                        }
                        if (!String.IsNullOrWhiteSpace(l_contactAddress.city))
                        {
                            l_addressBuilder.Append(l_contactAddress.city);
                            if (!String.IsNullOrWhiteSpace(l_contactAddress.province))
                            {
                                l_addressBuilder.Append(", ");
                                l_addressBuilder.Append(l_contactAddress.province);
                            }
                            if (!String.IsNullOrWhiteSpace(l_contactAddress.postal_code))
                            {
                                l_addressBuilder.Append(" - ");
                                l_addressBuilder.Append(l_contactAddress.postal_code);
                            }
                        }
                        l_address = l_addressBuilder.ToString();
                    }
                    if (l_contactDetails.ContactPhones.Any())
                    {
                        l_phones = GetPhones(l_contactDetails.ContactPhones);
                    }
                    l_worksheet.Cells[l_rowNumber, 1] = l_firstName;
                    l_worksheet.Cells[l_rowNumber, 2] = l_lastName;
                    l_worksheet.Cells[l_rowNumber, 3] = l_email;
                    l_worksheet.Cells[l_rowNumber, 4] = l_address;
                    l_worksheet.Cells[l_rowNumber, 5] = l_phones;
                    l_rowNumber++;
                }

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

        public void ExportToKidsAndParentsFlatExcelFile(List<KidsWithParentContactDetails> kidsWithParentContactDetails, string fileName)
        {
            Application l_application = null;
            Workbook l_workbook = null;
            Worksheet l_worksheet = null;

            try
            {
                l_application = new Application();
                string l_spreadsheetTemplateFolderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + @"\ExcelTemplates\";
                l_workbook =
                    l_application.Workbooks.Open(
                        l_spreadsheetTemplateFolderPath + @"FlatAddressBookForKids.xltx",
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                l_worksheet = (Worksheet)l_workbook.Sheets["Kids"];
                l_worksheet.Name = "Kids";
                int l_rowNumber = 2;
                foreach (KidsWithParentContactDetails l_kidsWithParentContactDetail in kidsWithParentContactDetails)
                {
                    string l_childName = GetFullName(l_kidsWithParentContactDetail.Child);

                    StringBuilder l_parentsNameBuilder = new StringBuilder();
                    StringBuilder l_phonesBuilder = new StringBuilder();
                    StringBuilder l_emailBuilder = new StringBuilder();
                    foreach (MemberWithContactDetails l_parent in l_kidsWithParentContactDetail.Parents)
                    {
                        string l_parentName = GetFullName(l_parent.Contact);
                        l_parentsNameBuilder.AppendLine(l_parentName);
                        string l_phones = GetPhones(l_parent.ContactPhones);
                        l_phonesBuilder.Append(l_phones);
                        if (!String.IsNullOrWhiteSpace(l_parent.Contact.email_address))
                        {
                            l_emailBuilder.AppendLine(l_parent.Contact.email_address);
                        }
                    }

                    l_worksheet.Cells[l_rowNumber, 1] = l_childName;
                    l_worksheet.Cells[l_rowNumber, 2] = l_parentsNameBuilder.ToString();
                    l_worksheet.Cells[l_rowNumber, 3] = l_phonesBuilder.ToString();
                    l_worksheet.Cells[l_rowNumber, 4] = l_emailBuilder.ToString();
                    l_rowNumber++;
                }

                l_workbook.SaveAs(
                    l_spreadsheetTemplateFolderPath + fileName,
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

        public void ExportAsGroupedExcelFile(IEnumerable<IEnumerable<MemberWithContactDetails>> allMembersGroupedByFamily, string fileName)
        {
            Application l_application = null;
            Workbook l_workbook = null;
            Worksheet l_worksheet = null;

            try
            {
                l_application = new Application();
                string l_spreadsheetTemplateFolderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + @"\ExcelTemplates\";
                l_workbook =
                    l_application.Workbooks.Open(
                        l_spreadsheetTemplateFolderPath + @"GroupedAddressBook.xltx",
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                l_worksheet = (Worksheet)l_workbook.Sheets["Contacts"];
                l_worksheet.Name = "Contacts";
                int l_rowNumber = 2;


                //l_worksheet.Cells[l_rowNumber, 1] = l_childName;
                //l_worksheet.Cells[l_rowNumber, 2] = l_parentsNameBuilder.ToString();
                //l_worksheet.Cells[l_rowNumber, 3] = l_phonesBuilder.ToString();
                //l_worksheet.Cells[l_rowNumber, 4] = l_emailBuilder.ToString();
                //l_rowNumber++;

                l_workbook.SaveAs(
                    l_spreadsheetTemplateFolderPath + fileName,
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
        private string GetPhones(IEnumerable<Phone> phones)
        {
            StringBuilder l_phoneBuilder = new StringBuilder();
            foreach (Phone l_phone in phones)
            {
                string l_phoneType = l_phone.phone_type_id == 1
                    ? "c"
                    : l_phone.phone_type_id == 2 ? "h" : "w";
                l_phoneBuilder.AppendLine(l_phone.phone_number + "(" + l_phoneType + ")");
            }
            return l_phoneBuilder.ToString();
        }

        private static string GetFullName(Contact contact)
        {
            StringBuilder l_childNameBuilder = new StringBuilder();
            l_childNameBuilder.Append(contact.first_name);
            if (!String.IsNullOrWhiteSpace(contact.middle_name))
            {
                l_childNameBuilder.Append(" ");
                l_childNameBuilder.Append(contact.middle_name);
            }
            if (!String.IsNullOrWhiteSpace(contact.last_name))
            {
                l_childNameBuilder.Append(" ");
                l_childNameBuilder.Append(contact.last_name);
            }
            return l_childNameBuilder.ToString();
        }


    }
}