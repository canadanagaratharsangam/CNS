//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CNS.DataAccessLayer.Sqlite
{
    using System;
    using System.Collections.Generic;
    
    public partial class Relationship
    {
        public long relationship_id { get; set; }
        public long contact_id { get; set; }
        public Nullable<long> related_to_contact_id { get; set; }
        public Nullable<long> relationshiptype_id { get; set; }
    
        public virtual Contact Contact { get; set; }
        public virtual Contact Contact1 { get; set; }
        public virtual RelationshipType RelationshipType { get; set; }
    }
}
