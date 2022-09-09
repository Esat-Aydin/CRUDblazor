using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDblazor.Shared
{
    public class SuperHero
    {
        [Key]
        public int Id { get; set; }
        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string HeroName { get; set; } = string.Empty;
        public Comic? Comic { get; set; }
        public int ComicId { get; set; }
        
    }
}
