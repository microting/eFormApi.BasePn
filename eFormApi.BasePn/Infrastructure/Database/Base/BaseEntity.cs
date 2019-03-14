using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.eFormApi.BasePn.Infrastructure.Database.Base
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        [StringLength(255)]
        public string WorkflowState { get; set; }
        
        public int CreatedByUserId { get; set; }
        
        public int UpdatedByUserId { get; set; }

        public int Version { get; set; }
    }
}
