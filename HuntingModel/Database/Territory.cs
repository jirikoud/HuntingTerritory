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
    
    public partial class Territory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Territory()
        {
            this.MapItems = new HashSet<MapItem>();
            this.MapItemTypes = new HashSet<MapItemType>();
            this.TerritoryUsers = new HashSet<TerritoryUser>();
            this.UserMapPoints = new HashSet<UserMapPoint>();
            this.MapAreas = new HashSet<MapArea>();
            this.TerritoryUserContacts = new HashSet<TerritoryUserContact>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int StewardId { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public System.DateTime SysCreated { get; set; }
        public Nullable<System.DateTime> SysUpdated { get; set; }
        public int SysCreator { get; set; }
        public Nullable<int> SysEditor { get; set; }
        public bool IsDemoTemplate { get; set; }
        public bool IsPublic { get; set; }
    
        public virtual AclUser AclUserCreator { get; set; }
        public virtual AclUser AclUserEditor { get; set; }
        public virtual AclUser AclUserSteward { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MapItem> MapItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MapItemType> MapItemTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TerritoryUser> TerritoryUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserMapPoint> UserMapPoints { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MapArea> MapAreas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TerritoryUserContact> TerritoryUserContacts { get; set; }
    }
}
