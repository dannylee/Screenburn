using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DeckBuilder.ViewModels
{
    using Models;

    class PresentationChooserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<PresentationCategory> items; 

        public List<PresentationCategory> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                this.OnPropertyChanged("Items");
            }
        } 

        public PresentationChooserViewModel()
        {
            loadPresentations();
            
            /*
            var presentations = new List<Presentation>
                {
                    new Presentation
                        {
                            Title = "Beats By Dre",
                            Category = "Client Pitch",
                            Subtitle = "Show Your Color Campaign",
                            Image = "http://th05.deviantart.net/fs70/PRE/i/2012/213/3/d/beats_by_dre_logo_by_txfdesigns-d59f6jo.png"
                        },
                    new Presentation
                        {
                            Title = "Heineken",
                            Category = "Project Learnings",
                            Subtitle = "Dial a Keg Campaign",
                            Image = "http://www.advanced-television.com/wp-content/uploads/2011/06/heineken.jpg"
                        }
                };
            var presentationsByCategories =
                presentations.GroupBy(x => x.Category)
                             .Select(x => new PresentationCategory {Title = x.Key, Items = x.ToList()});
            Items = presentationsByCategories.ToList();
            */
        }
        private async void loadPresentations()
        {
            var documentsFolder = KnownFolders.DocumentsLibrary;
            var presentationsFolder = await documentsFolder.GetFolderAsync("DeckBuilder");
            var files = await presentationsFolder.GetItemsAsync();
            
            var presentations = new List<Presentation>();
            foreach (var file in files)
            {
                var storageItem = (StorageFile) file;
                var stream = await storageItem.OpenStreamForReadAsync();
                var jsonSerializer = new DataContractJsonSerializer(typeof(Presentation));
                var presentation = (Presentation)jsonSerializer.ReadObject(stream);
                presentations.Add(presentation);
                stream.Dispose();
            }

            var presentationsByCategories =
                presentations.GroupBy(x => "All Presentations")
                             .Select(x => new PresentationCategory { Title = x.Key, Items = x.ToList() });
            Items = presentationsByCategories.ToList();
        }
        protected void OnPropertyChanged(string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
