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
                    string l_phones = String.Empty;
                    Address l_contactAddress = l_contactDetails.ContactAddress;
                    string l_address = GetAddress(l_contactAddress);
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

        private static string GetAddress(Address address)
        {
            string l_address = String.Empty;
            if (address != null)
            {
                StringBuilder l_addressBuilder = new StringBuilder();
                if (!String.IsNullOrWhiteSpace(address.address_line_1))
                {
                    l_addressBuilder.AppendLine(address.address_line_1);
                }
                if (!String.IsNullOrWhiteSpace(address.address_line_2))
                {
                    l_addressBuilder.AppendLine(address.address_line_2);
                }
                if (!String.IsNullOrWhiteSpace(address.city))
                {
                    l_addressBuilder.Append(address.city);
                    if (!String.IsNullOrWhiteSpace(address.province))
                    {
                        l_addressBuilder.Append(", ");
                        l_addressBuilder.Append(address.province);
                    }
                    if (!String.IsNullOrWhiteSpace(address.postal_code))
                    {
                        l_addressBuilder.Append(" - ");
                        l_addressBuilder.Append(address.postal_code);
                    }
                }
                l_address = l_addressBuilder.ToString();
            }
            return l_address;
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

        public void ExportAsGroupedExcelFile(IEnumerable<IEnumerable<Tuple<MemberWithContactDetails, string>>> allMembersGroupedByFamily, string fileName)
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

                foreach (IEnumerable<Tuple<MemberWithContactDetails, string>> l_memberWithContactDetails in allMembersGroupedByFamily)
                {
                    int l_familySectionStartRowNumber = l_rowNumber;
                    Tuple<MemberWithContactDetails, string> l_headTuple = l_memberWithContactDetails.First();
                    var l_familyHead = l_headTuple.Item1;
                    var l_familyName = GetFullName(l_familyHead.Contact) + "'s Family";

                    //Family Name
                    l_worksheet.Cells[l_rowNumber, 1] = l_familyName;
                    Range l_FamilyNameRange = l_worksheet.Range[l_worksheet.Cells[l_rowNumber, 1], l_worksheet.Cells[l_rowNumber, 2]];
                    l_FamilyNameRange.Merge();
                    l_FamilyNameRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    l_FamilyNameRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    l_FamilyNameRange.Font.Bold = true;
                    l_FamilyNameRange.Font.Name = "Segoe UI";
                    l_FamilyNameRange.Font.Size = 10;
                    l_FamilyNameRange.RowHeight = 24;

                    List<Phone> l_allHomePhones = new List<Phone>();
                    foreach (Tuple<MemberWithContactDetails, string> l_memberWithContactDetail in l_memberWithContactDetails)
                    {
                        IEnumerable<Phone> l_homePhones = l_memberWithContactDetail.Item1.ContactPhones.Where(p => p.phone_type_id == 2);
                        l_allHomePhones.AddRange(l_homePhones);
                    }
                    var l_homePhonesString = GetPhones(l_allHomePhones);
                    l_worksheet.Cells[l_rowNumber, 3] = l_homePhonesString;
                    Range l_phoneNumberRange = l_worksheet.Range[l_worksheet.Cells[l_rowNumber, 3], l_worksheet.Cells[l_rowNumber, 3]];
                    l_phoneNumberRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    //Individual Names
                    foreach (Tuple<MemberWithContactDetails, string> l_familyMember in l_memberWithContactDetails)
                    {
                        l_rowNumber++;
                        if (l_familyMember.Item2 != "Head")
                        {
                            l_worksheet.Cells[l_rowNumber, 2] = GetFullName(l_familyMember.Item1.Contact) + " (" + l_familyMember.Item2 + ")";
                        }
                        else
                        {
                            l_worksheet.Cells[l_rowNumber, 2] = GetFullName(l_familyMember.Item1.Contact);
                        }
                        var l_cellPhones = l_familyMember.Item1.ContactPhones.Where(p => p.phone_type_id == 1);
                        var l_cellPhonesString = GetPhones(l_cellPhones);
                        l_worksheet.Cells[l_rowNumber, 3] = l_cellPhonesString;

                        if (!String.IsNullOrWhiteSpace(l_familyMember.Item1.Contact.email_address))
                        {
                            l_worksheet.Cells[l_rowNumber, 4] = l_familyMember.Item1.Contact.email_address;
                        }
                        Range l_FamilyInfoRange = l_worksheet.Range[l_worksheet.Cells[l_rowNumber, 2], l_worksheet.Cells[l_rowNumber, 5]];
                        l_FamilyInfoRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    }
                    int l_familySectionEndRowNumber = l_rowNumber;
                    l_worksheet.Cells[l_rowNumber, 5] = GetAddress(l_familyHead.ContactAddress);
                    Range l_AddressRange = l_worksheet.Range[l_worksheet.Cells[l_familySectionStartRowNumber, 5], l_worksheet.Cells[l_familySectionEndRowNumber, 5]];
                    l_AddressRange.Merge();
                    l_AddressRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;

                    l_rowNumber++;
                    //New Line between families
                    Range l_familySeperatorRange = l_worksheet.Range[l_worksheet.Cells[l_rowNumber, 1], l_worksheet.Cells[l_rowNumber, 5]];
                    l_familySeperatorRange.Merge();
                    l_familySeperatorRange.RowHeight = 2;
                    l_familySeperatorRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkGray); ;
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
            return l_phoneBuilder.ToString().TrimEnd('\r', '\n'); ;
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