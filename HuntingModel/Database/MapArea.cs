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
    
    public partial class MapArea
    {
        public int Id { get; set; }
        public int TerritoryId { get; set; }
        public string Name { get; set; }
        public string PolygonData { get; set; }
        public int SysCreator { get; set; }
        public System.DateTime SysCreated { get; set; }
        public Nullable<int> SysEditor { get; set; }
        public Nullable<System.DateTime> SysUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
    
        public virtual AclUser AclUserCreator { get; set; }
        public virtual AclUser AclUserEditor { get; set; }
        public virtual Territory Territory { get; set; }
    }
}