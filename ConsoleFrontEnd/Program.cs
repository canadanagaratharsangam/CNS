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
        static void Main(string[] args)
        {
            ContactsController l_ContactsController = new ContactsController();
            List<MemberWithContactDetails> l_contactDetailsSortedAlphabetically = l_ContactsController.GetAllMemberswithContactDetailsSortedAlphabetically().ToList();
            CNS.ExcelExporter.FlatExcelFileExporter l_ExcelFileExporter = new FlatExcelFileExporter();
            l_ExcelFileExporter.ExportToFlatExcelFile(l_contactDetailsSortedAlphabetically,"All Members.xlsx");
            Console.ReadLine();
        }
    }
}
