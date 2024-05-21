namespace EntityLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class User
    {
        public int Id { get; set; }

        public byte[] ProfileImage { get; set; }

        [StringLength(11)]
        public string PIN { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(12)]
        public string DateOfBirth { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(64)]
        public string Password { get; set; }

        [StringLength(64)]
        public string Salt { get; set; }

        public int? Id_role { get; set; }

        public int? Id_Group { get; set; }

        public virtual Group Group { get; set; }
    }
}
