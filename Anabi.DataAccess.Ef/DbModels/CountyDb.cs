﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Anabi.DataAccess.Ef.DbModels
{
    [Table("Counties")]
    public class CountyDb
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abreviation { get; set; }

        public ICollection<AddressDb> Addresses { get; set; }
    }
}
