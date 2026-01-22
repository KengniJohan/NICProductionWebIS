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

        [Required, Column("name"), MaxLength(50)]
        public required string Name { get; set; }

        [Required, Column("surname"), MaxLength(50)]
        public required string Surname { get; set; }

        [Required, Column("bornPlace"), MaxLength(50)]
        public required string BornPlace { get; set; }

        [Required, DataType(DataType.Date), Column( "bornDate", TypeName="date")]
        public DateTime BornDate { get; set; }

        [Required, Column( "profession"), MaxLength(50)]
        public required string Profession { get; set; }

        [Column( "gender")]
        public Gender Gender { get; set; }

        [Column("fatherName"), MaxLength(100)]
        public string? FatherName { get; set; }

        [Column( "motherName"), MaxLength(100)]
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
