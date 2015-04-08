using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNS.DataAccessLayer.Sqlite;

namespace CNS.Model
{
    public class MemberWithContactDetails
    {
        public Contact Contact { get; set; }
        public Address ContactAddress { get; set; }
        public IEnumerable<Phone> ContactPhones { get; set; }

    }
}
