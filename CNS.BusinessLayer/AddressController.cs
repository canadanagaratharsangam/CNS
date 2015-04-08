using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNS.DataAccessLayer.Sqlite;
using Address = CNS.Model.Address;

namespace CNS.BusinessLayer
{
    public class AddressController
    {
        public IEnumerable<Address> GetAllAddresses()
        {
            using (CNSConnection connection = new CNSConnection())
            {
                foreach (DataAccessLayer.Sqlite.Address l_address in connection.Addresses)
                {
                    yield return new Address()
                    {
                        Address1 = l_address.address_line_1,
                        Address2 = l_address.address_line_2,
                        City = l_address.city,
                        Province = l_address.province,
                        PostalCode = l_address.postal_code
                    };
                }
            }
        }
    }
}
