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
        private static FlatExcelFileExporter m_FlatExcelFileExporter;
        static void Main(string[] args)
        {
            m_ContactsController = new ContactsController();
            m_FlatExcelFileExporter = new FlatExcelFileExporter();
            GenerateAlphabeticallySortedMemberList();
            GeneratMemberListGroupedAsFamily();
            GeneratAllKidsWIthParentContactInformation();
            GenerateAllAdultsGroupedAsFamily();
            Console.ReadLine();
        }

        private static void GeneratAllKidsWIthParentContactInformation()
        {
            List<KidsWithParentContactDetails> l_KidsWithParentContactDetails = m_ContactsController.GetAllKidsWIthParentContactInformation();
            m_FlatExcelFileExporter.ExportToKidsAndParentsFlatExcelFile(l_KidsWithParentContactDetails, "All Children With Parent Contact Details.xlsx");
        }

        private static void GenerateAlphabeticallySortedMemberList()
        {
            List<MemberWithContactDetails> l_contactDetailsSortedAlphabetically = m_ContactsController.GetAllMemberswithContactDetailsSortedAlphabetically().ToList();
            m_FlatExcelFileExporter.ExportToFlatExcelFile(l_contactDetailsSortedAlphabetically, "All Members - Sorted Alphabetically.xlsx");
        }

        private static void GeneratMemberListGroupedAsFamily()
        {
            List<MemberWithContactDetails> l_membersListGroupedAsFamily = m_ContactsController.GetAllMembersGroupedAsFamilyFlatStructure().ToList();
            m_FlatExcelFileExporter.ExportToFlatExcelFile(l_membersListGroupedAsFamily, "All Members - Sorted by Family.xlsx");
        }

        private static void GenerateAllAdultsGroupedAsFamily()
        {
            List<MemberWithContactDetails> l_membersListGroupedAsFamily = m_ContactsController.GetAllAdultsGroupedAsFamilyFlatStructure().ToList();
            m_FlatExcelFileExporter.ExportToFlatExcelFile(l_membersListGroupedAsFamily, "All Adults - Sorted by Family.xlsx");
        }
    }
}
