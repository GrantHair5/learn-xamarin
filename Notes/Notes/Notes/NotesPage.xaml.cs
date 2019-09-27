using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;
using Notes.Models;

namespace Notes
{
    public partial class NotesPage : ContentPage
    {
        public NotesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");
            var listOfNotes = new List<Note>();

            foreach (var file in files)
            {
                var note = JsonConvert.DeserializeObject<Note>(File.ReadAllText(file));
                note.Filename = file;
                note.Description = note.IsInBasket ? "Is In Basket" : "Not In Basket";
                listOfNotes.Add(note);
            }

            listView.ItemsSource = listOfNotes
               .OrderByDescending(d => d.Category)
               .ToList();
        }

        private async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoteEntryPage
            {
                BindingContext = new Note()
            });
        }

        private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new NoteEntryPage
                {
                    BindingContext = e.SelectedItem as Note
                });
            }
        }

        private async void DeleteAllClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirm", "Do you want to delete all Items?", "Yes", "No");
            if (!answer)
            {
                return;
            }

            var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");

            var notes = files.Select(filename =>
                new Note
                {
                    Filename = filename
                }).ToList();

            foreach (var note in notes)
            {
                if (File.Exists(note.Filename))
                {
                    File.Delete(note.Filename);
                }
            }

            await DisplayAlert("Success", "All items deleted", "OK");

            OnAppearing();
        }

        private async void RefreshListClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirm", "Do you want to refresh all Items to original state?", "Yes", "No");
            if (!answer)
            {
                return;
            }

            var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");

            var notes = files.Select(filename =>
                new Note
                {
                    Filename = filename
                }).ToList();

            foreach (var note in notes)
            {
                note.IsInBasket = false;
            }

            await DisplayAlert("Success", "All items refreshed", "OK");

            OnAppearing();
        }
    }
}