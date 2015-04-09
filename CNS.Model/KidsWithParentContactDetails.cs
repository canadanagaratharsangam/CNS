using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNS.DataAccessLayer.Sqlite;

namespace CNS.Model
{
    public class KidsWithParentContactDetails
    {
        public Contact Child { get; set; }

        public List<MemberWithContactDetails> Parents { get; set; }
    }
}
