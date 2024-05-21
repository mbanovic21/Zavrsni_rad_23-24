namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Role
    {
        public int Id { get; set; }

        [StringLength(15)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
