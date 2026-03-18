using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberJob.Models
{
    [Table("blogs")]
    public class Blog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title", TypeName = "json")]
        public string Title { get; set; }

        [Column("description", TypeName = "json")]
        public string Description { get; set; }

        [Column("image")]
        public string Image { get; set; }

        [Column("read_count")]
        public int ReadCount { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}