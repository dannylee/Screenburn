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

    class PresentationCreatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Presentation presentation;

        public Presentation Presentation
        {
            get
            {
                return presentation;
            }
            set
            {
                presentation = value;
                this.NotifyPropertyChanged("Presentation");
            }
        }

        public PresentationCreatorViewModel(string id)
        {
            loadPresentation(id);
        }

        private async void loadPresentation(string id)
        {
            Presentation = new Presentation();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var documentsFolder = KnownFolders.DocumentsLibrary;
                    var presentationsFolder = await documentsFolder.GetFolderAsync("DeckBuilder");
                    var json = await presentationsFolder.GetFileAsync(string.Format("{0}.json", id));
                    var stream = await json.OpenStreamForReadAsync();
                    var jsonSerializer = new DataContractJsonSerializer(typeof (Presentation));
                    var tempPresentation = (Presentation) jsonSerializer.ReadObject(stream);
                    var nuPresentation = new Presentation
                        {
                            ID = "asdlfkjsdiofjsdfijsdfisdlfsd",
                            Title = "Test",
                            Subtitle = "Subtitle",
                            Image = "http://www.advanced-television.com/wp-content/uploads/2011/06/heineken.jpg",
                            Slides = new List<Slide>()
                        };
                    Presentation = nuPresentation;
/*                    Presentation.Title = tempPresentation.Title;
                    Presentation.Subtitle = tempPresentation.Subtitle;*/
                    stream.Dispose();
                }
            }
            catch (Exception ex)
            {
                
            }

        }
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
