//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sporty.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class SportType
    {
        public SportType()
        {
            this.Exercise = new HashSet<Exercise>();
            this.Plan = new HashSet<Plan>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public System.Guid UserId { get; set; }
    
        public virtual ICollection<Exercise> Exercise { get; set; }
        public virtual ICollection<Plan> Plan { get; set; }
        public virtual User User { get; set; }
    }
}