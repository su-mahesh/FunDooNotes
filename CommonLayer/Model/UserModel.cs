using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CommonLayer.Model
{
    [Microsoft.EntityFrameworkCore.Index(nameof(Email), IsUnique = true)]
    public class UserModel
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserID { get; set; }

        [Column(TypeName = "varchar(50)", Order = 2)]          
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)", Order = 3)]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(100)", Order = 4)]
        [Required]
        public string Email { get; set; }

        [Column(TypeName = "varchar(100)", Order = 5)]
        [Required]
        public string Password { get; set; }
    }
}
