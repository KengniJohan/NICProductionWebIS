using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace NICProductionWebIS.Models
{
    [Table("NicTable")]
    public class NicModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, Column("name")]
        public required string Name { get; set; }

        [Required, Column("surname")]
        public required string Surname { get; set; }

        [Required, Column("bornPlace")]
        public required string BornPlace { get; set; }

        [Required, DataType(DataType.Date), Column( "bornDate", TypeName="date")]
        public DateTime BornDate { get; set; }

        [Required, Column( "profession")]
        public required string Profession { get; set; }

        [Column( "gender")]
        public Gender Gender { get; set; }

        [Column("fatherName")]
        public string? FatherName { get; set; }

        [Column( "motherName")]
        public string? MotherName { get; set; }

        [Required, Column( "height")]
        public int Height { get; set; }

        [Column( "photo")]
        public byte[]? Photo { get; set; }

        [Required, DataType(DataType.Date), Column("issueDate", TypeName = "date")]
        public DateTime IssueDate { get; set; }

        [Required, DataType(DataType.Date), Column("expiredDate", TypeName = "date")]
        public DateTime ExpiredDate { get; set; }
    }
}
