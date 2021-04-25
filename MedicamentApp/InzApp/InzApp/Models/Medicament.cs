using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace InzApp.Models
{
    class Medicament
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Composition { get; set; }
        public string Dose { get; set; }
        public string Indications { get; set; }
        public bool AbilityToDrive { get; set; }
        public Medicament()
        {
        
        }
    }
}
