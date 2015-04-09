using System;
using System.Collections.Generic;
using System.Linq;
using CNS.BusinessLayer;
using CNS.ExcelExporter;
using CNS.Model;

namespace ConsoleFrontEnd
{
    class Program
    {
        private static ContactsController m_ContactsController;
        private static ExcelFileExporter m_excelFileExporter;
        static void Main(string[] args)
        {
            m_ContactsController = new ContactsController();
            m_excelFileExporter = new ExcelFileExporter();
            GenerateAlphabeticallySortedMemberList();
            GeneratMemberListGroupedAsFamily();
            GeneratAllKidsWIthParentContactInformation();
            GenerateAllAdultsGroupedAsFamily();
            GenerateCustomListOfMembers();
            Console.ReadLine();
        }

        private static void GenerateCustomListOfMembers()
        {
            IEnumerable<IEnumerable<Tuple<MemberWithContactDetails, string>>> l_allMemberswithContactDetailsGroupedAsFamily = m_ContactsController.GetAllMemberswithContactDetailsGroupedAsFamily();
            m_excelFileExporter.ExportAsGroupedExcelFile(l_allMemberswithContactDetailsGroupedAsFamily, "Master List (Grouped by Family).xlsx");
        }

        private static void GeneratAllKidsWIthParentContactInformation()
        {
            List<KidsWithParentContactDetails> l_KidsWithParentContactDetails = m_ContactsController.GetAllKidsWIthParentContactInformation();
            m_excelFileExporter.ExportToKidsAndParentsFlatExcelFile(l_KidsWithParentContactDetails, "All Children With Parent Contact Details.xlsx");
        }

        private static void GenerateAlphabeticallySortedMemberList()
        {
            List<MemberWithContactDetails> l_contactDetailsSortedAlphabetically = m_ContactsController.GetAllMemberswithContactDetailsSortedAlphabetically().ToList();
            m_excelFileExporter.ExportToFlatExcelFile(l_contactDetailsSortedAlphabetically, "All Members - Sorted Alphabetically.xlsx");
        }

        private static void GeneratMemberListGroupedAsFamily()
        {
            List<MemberWithContactDetails> l_membersListGroupedAsFamily = m_ContactsController.GetAllMembersGroupedAsFamilyFlatStructure().ToList();
            m_excelFileExporter.ExportToFlatExcelFile(l_membersListGroupedAsFamily, "All Members - Sorted by Family.xlsx");
        }

        private static void GenerateAllAdultsGroupedAsFamily()
        {
            List<MemberWithContactDetails> l_membersListGroupedAsFamily = m_ContactsController.GetAllAdultsGroupedAsFamilyFlatStructure().ToList();
            m_excelFileExporter.ExportToFlatExcelFile(l_membersListGroupedAsFamily, "All Adults - Sorted by Family.xlsx");
        }
    }
}
