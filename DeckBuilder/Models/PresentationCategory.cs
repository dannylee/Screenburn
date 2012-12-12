using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckBuilder.Models
{
    public class PresentationCategory
    {
        public string Title { get; set; }

        public List<Presentation> Items { get; set; } 
    }
}
