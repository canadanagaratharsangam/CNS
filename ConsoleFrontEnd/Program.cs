using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNS.BusinessLayer;
using CNS.Model;

namespace ConsoleFrontEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Printing Addresses");
            AddressController l_addressController = new AddressController();
            foreach (Address l_address in l_addressController.GetAllAddresses())
            {
                Console.WriteLine(l_address.Address1);
            }
            Console.ReadLine();
        }
    }
}
