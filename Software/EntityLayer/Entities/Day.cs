namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Day")]
    public partial class Day
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Day()
        {
            Days_Users = new HashSet<Days_Users>();
            DailyActivities = new HashSet<DailyActivity>();
        }

        public int Id { get; set; }

        [StringLength(12)]
        public string Date { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? Id_WeeklySchedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Days_Users> Days_Users { get; set; }

        public virtual WeeklySchedule WeeklySchedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyActivity> DailyActivities { get; set; }
    }
}
