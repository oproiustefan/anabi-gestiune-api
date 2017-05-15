﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Anabi.DataAccess.DbModels
{
    public class AdresaDb
    {
        public int Id { get; set; }

        public int JudetId { get; set; }

        public virtual JudetDb Judet { get; set; }

        public string Strada { get; set; }

        public string Oras { get; set; }

        public string Cladire { get; set; }

        public string Scara { get; set; }

        public string Etaj { get; set; }

        public string NrApartament { get; set; }


    }
}
