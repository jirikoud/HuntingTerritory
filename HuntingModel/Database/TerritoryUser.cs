//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HuntingModel.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class TerritoryUser
    {
        public int AclUserId { get; set; }
        public int TerritoryId { get; set; }
        public int Id { get; set; }
        public int TerritoryUserRole { get; set; }
    
        public virtual AclUser AclUser { get; set; }
        public virtual Territory Territory { get; set; }
    }
}
