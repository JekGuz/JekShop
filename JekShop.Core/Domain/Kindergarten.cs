using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JekShop.Core.Domain
{
    public interface Kindergarten
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public int ChildrenCount { get; set; }
        public string KindergartenName { get; set; }
        public string TeacherName{ get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        // Teha uus branch nimega Kindergarten
        // Teha uus GRUD e lisamine, vaatamine, uuendamine ja andmete kustamine
        // Muutujateks on Id, GroupName, ChildrenCount, KindergantenName, TeacherName, CreateAt, UpdateAt
        //Töö on hindeline.Töö panna githubi ja link saata emailile. Tunnis näitad ette.
    }
}
