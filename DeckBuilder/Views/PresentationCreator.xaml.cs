using System;
using System.Collections.Generic;

using System.Linq;

using System.Text;
using DeckBuilder.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

using Windows.Storage;
using DeckBuilder.Models;
using System.Runtime.Serialization.Json;
using System.IO;

namespace DeckBuilder.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PresentationCreator : DeckBuilder.Common.LayoutAwarePage
    {
        private StorageFile _presentationJson = null;

        public PresentationCreator()
        {
            this.InitializeComponent();


            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var id = e.Parameter as string;
            loadPresentation(id);
            /*
            var viewModel = new PresentationCreatorViewModel(id);
            this.DataContext = viewModel;
            */
            
        }
        private async void loadPresentation(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var documentsFolder = KnownFolders.DocumentsLibrary;
                var presentationsFolder = await documentsFolder.GetFolderAsync("DeckBuilder");
                var json = await presentationsFolder.GetFileAsync(string.Format("{0}.json", id));
                var stream = await json.OpenStreamForReadAsync();
                var jsonSerializer = new DataContractJsonSerializer(typeof(Presentation));
                var presentation = (Presentation)jsonSerializer.ReadObject(stream);
                stream.Dispose();

                ID.Text = presentation.ID;
                Title.Text = presentation.Title;
                Subtitle.Text = presentation.Subtitle;
                Image.Text = presentation.Image;
                if (presentation.Slides != null)
                {
                    foreach (var slide in presentation.Slides)
                    {
                        SlidesGridView.Items.Add(slide);
                    }
                }
                    

            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void SavePresentation()
        {
            var presentationId = (string.IsNullOrEmpty(ID.Text)) ? Guid.NewGuid().ToString() : ID.Text;
            
            var presentation = new Presentation
                {
                    ID = presentationId,
                    Title = Title.Text,
                    Subtitle = Subtitle.Text,
                    Image = Image.Text
                };
            presentation.Slides = new List<Slide>();
            if (SlidesGridView.Items != null)
            {
                foreach (var item in SlidesGridView.Items)
                {
                    var slide = (Slide)item;
                    presentation.Slides.Add(slide);
                }
            }
                

            var jsonStream = new MemoryStream();
            var jsonSerializer = new DataContractJsonSerializer(typeof(Presentation));
            jsonSerializer.WriteObject(jsonStream, presentation);
            var json = Encoding.UTF8.GetString(jsonStream.ToArray(), 0, (int)jsonStream.Length);

            
            //_presentationJson = await KnownFolders.DocumentsLibrary.GetFileAsync(string.Format("{0}.json",presentationId));
            var documentsFolder = KnownFolders.DocumentsLibrary;
            var deckbuilderFolder = await documentsFolder.CreateFolderAsync("DeckBuilder",CreationCollisionOption.OpenIfExists);
            _presentationJson = await deckbuilderFolder.CreateFileAsync(string.Format("{0}.json",presentationId),CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(_presentationJson, json);
        }

        private async void Add_OnClick(object sender, RoutedEventArgs e)
        {
            SavePresentation();
            
            //Feedback.Text = "Presentation Saved";
            Frame.Navigate(typeof (PresentationChooser));
        }

        private void AddSlide_OnClick(object sender, RoutedEventArgs e)
        {
            var slide = new Slide
                {
                    ID = Guid.NewGuid().ToString(),
                    PresentationId = ID.Text,
                    Title = "New Slide",
                    Body = "Insert body here"
                };
            SlidesGridView.Items.Add(slide);

            SavePresentation();
        }
    }
}
