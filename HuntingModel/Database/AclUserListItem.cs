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
    
    public partial class AclUserListItem
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public bool IsDisabled { get; set; }
        public int AccountType { get; set; }
        public int MaxTerritoryCount { get; set; }
        public int CurrentTerritoryCount { get; set; }
    }
}