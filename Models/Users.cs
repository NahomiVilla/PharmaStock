using System.Collections.Generic;

namespace PharmaStock.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Rol { get; set; }
        public DateTime FechaRegistro { get; set; }=DateTime.UtcNow;
    }
}