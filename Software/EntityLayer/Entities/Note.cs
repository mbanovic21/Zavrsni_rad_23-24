namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Note
    {
        public int Id { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Behaviour { get; set; }

        public int? Id_child { get; set; }

        public virtual Child Child { get; set; }
    }
}
