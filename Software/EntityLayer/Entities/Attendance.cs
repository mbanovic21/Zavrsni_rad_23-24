namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Attendance")]
    public partial class Attendance
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public bool? isPresent { get; set; }

        public int? Id_Child { get; set; }

        public int? Id_User { get; set; }

        public virtual Child Child { get; set; }

        public virtual User User { get; set; }
    }
}
