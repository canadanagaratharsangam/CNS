using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNS.BusinessLayer;
using CNS.DataAccessLayer.Sqlite;
using CNS.Model;
using Address = CNS.DataAccessLayer.Sqlite.Address;

namespace ConsoleFrontEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            ContactsController l_ContactsController = new ContactsController();
            var l_allMemberswithContactDetails = l_ContactsController.GetAllMemberswithContactDetailsGroupedAsFamily();
            var l_al2lMemberswithContactDetails = l_ContactsController.GetAllMemberswithContactDetailsSortedAlphabetically();
            Console.ReadLine();
        }
    }
}
