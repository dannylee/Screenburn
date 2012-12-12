using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckBuilder.Models
{
    public class Slide
    {
        public string ID { get; set; }

        public string PresentationId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
