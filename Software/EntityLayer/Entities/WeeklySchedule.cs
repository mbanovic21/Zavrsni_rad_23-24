namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WeeklySchedule")]
    public partial class WeeklySchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WeeklySchedule()
        {
            Days = new HashSet<Day>();
        }

        public int Id { get; set; }

        [StringLength(12)]
        public string StartDate { get; set; }

        [StringLength(12)]
        public string EndDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Day> Days { get; set; }
    }
}
