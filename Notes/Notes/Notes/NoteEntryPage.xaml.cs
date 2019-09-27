using System;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;
using Notes.Models;

namespace Notes
{
    public partial class NoteEntryPage : ContentPage
    {
        public NoteEntryPage()
        {
            InitializeComponent();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            if (string.IsNullOrWhiteSpace(note.Filename))
            {
                // Save
                note.Date = DateTime.UtcNow;
                var noteText = JsonConvert.SerializeObject(note);
                var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.notes.txt");
                File.WriteAllText(filename, noteText);
            }
            else
            {
                var noteText = JsonConvert.SerializeObject(note);
                // Update
                File.WriteAllText(note.Filename, noteText);
            }

            await Navigation.PopAsync();
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            if (File.Exists(note.Filename))
            {
                File.Delete(note.Filename);
            }

            await Navigation.PopAsync();
        }

        private async void AddToBasketClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            // Update
            note.IsInBasket = true;
            File.WriteAllText(note.Filename, JsonConvert.SerializeObject(note));

            await Navigation.PopAsync();
        }
    }
}