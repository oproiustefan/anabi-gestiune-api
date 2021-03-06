﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anabi.DataAccess.Ef.DbModels
{
    [Table("Institutions")]
    public class InstitutionDb : BaseEntity
    {
        public int CategoryId { get; set; }
        public virtual CategoryDb Category { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string ContactData { get; set; }
        
        public virtual ICollection<HistoricalStageDb> HistoricalStages { get; set; }
    }
}
