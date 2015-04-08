using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNS.DataAccessLayer.Sqlite;
using CNS.Model;

namespace CNS.BusinessLayer
{
    public class ContactsController
    {
        public IEnumerable<IEnumerable<MemberWithContactDetails>> GetAllMemberswithContactDetailsGroupedAsFamily()
        {
            List<List<MemberWithContactDetails>> l_returnAddressWithContactDetails = new List<List<MemberWithContactDetails>>();
            using (CNSConnection l_connection = new CNSConnection())
            {
                foreach (Contact l_familyHead in l_connection.Relationships.Where(r => r.RelationshipType.relationshiptype_id == 1).Select(r => r.Contact1).OrderBy(c => c.first_name).ThenBy(c => c.last_name))
                {
                    List<MemberWithContactDetails> l_familyMembersContactDetails = new List<MemberWithContactDetails>();
                    MemberWithContactDetails l_memberWithContactDetails = new MemberWithContactDetails();
                    l_memberWithContactDetails.Contact = l_familyHead;
                    l_memberWithContactDetails.ContactAddress = l_familyHead.Address;
                    l_memberWithContactDetails.ContactPhones = l_familyHead.Phones;
                    l_familyMembersContactDetails.Add(l_memberWithContactDetails);

                    foreach (Relationship l_relationship in l_familyHead.Relationships)
                    {
                        MemberWithContactDetails l_relatedMemberWithContactDetails = new MemberWithContactDetails();
                        l_relatedMemberWithContactDetails.Contact = l_relationship.Contact1;
                        l_relatedMemberWithContactDetails.ContactAddress = l_relationship.Contact1.Address;
                        l_relatedMemberWithContactDetails.ContactPhones = l_relationship.Contact1.Phones;
                        l_familyMembersContactDetails.Add(l_relatedMemberWithContactDetails);
                    }

                    l_returnAddressWithContactDetails.Add(l_familyMembersContactDetails);
                }
            }
            return l_returnAddressWithContactDetails;
        }

        public IEnumerable<MemberWithContactDetails> GetAllMemberswithContactDetailsSortedAlphabetically()
        {
            List<MemberWithContactDetails> l_returnAddressWithContactDetails = new List<MemberWithContactDetails>();
            using (CNSConnection l_connection = new CNSConnection())
            {
                foreach (Contact l_contact in l_connection.Contacts.OrderBy(c => c.first_name).ThenBy(c => c.last_name))
                {
                    MemberWithContactDetails l_AddressWithContactDetails = new MemberWithContactDetails();
                    l_AddressWithContactDetails.Contact = l_contact;
                    l_AddressWithContactDetails.ContactAddress = l_contact.Address;
                    l_AddressWithContactDetails.ContactPhones = l_contact.Phones;
                    l_returnAddressWithContactDetails.Add(l_AddressWithContactDetails);
                }
            }
            return l_returnAddressWithContactDetails;
        }
    }
}
