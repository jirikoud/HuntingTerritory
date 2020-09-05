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
    
    public partial class CheckIn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CheckIn()
        {
            this.Answers = new HashSet<Answer>();
        }
    
        public int Id { get; set; }
        public int MapItemId { get; set; }
        public Nullable<int> QuestionnaireId { get; set; }
        public string Note { get; set; }
        public System.DateTime CheckTime { get; set; }
        public System.DateTime SysCreated { get; set; }
        public Nullable<System.DateTime> SysUpdated { get; set; }
        public int SysCreator { get; set; }
        public Nullable<int> SysEditor { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual AclUser AclUserCreator { get; set; }
        public virtual AclUser AclUserEditor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual MapItem MapItem { get; set; }
        public virtual Questionnaire Questionnaire { get; set; }
    }
}