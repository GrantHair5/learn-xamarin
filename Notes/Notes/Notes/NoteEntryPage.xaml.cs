using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var listOfCategories = new Dictionary<string, string>
            {
                { "None", "None" },
                {"Bakery", "Bakery" },
                { "Cleaning", "Cleaning" },
                { "Drinks", "Drinks"},
                { "Fruit", "Fruit" },
                { "Frozen", "Frozen" },
                { "Eggs", "Eggs" },
                { "Meat", "Meat" },
                { "Vegetables", "Vegetables" }
            };

            var picker = CategoryPicker;

            foreach (var category in listOfCategories.Keys)
            {
                picker.Items.Add(category);
            }

            var note = (Note)BindingContext;

            if (note.Date == DateTime.MinValue)
            {
                AddToBasketButton.IsVisible = false;
                RemoveFromBasketButton.IsVisible = false;
                return;
            }

            if (!note.IsInBasket)
            {
                AddToBasketButton.IsVisible = true;
                RemoveFromBasketButton.IsVisible = false;
                return;
            }

            AddToBasketButton.IsVisible = false;
            RemoveFromBasketButton.IsVisible = true;

            picker.SelectedItem = note.Category;

            picker.SelectedIndexChanged += (sender, args) =>
            {
                note.Category = picker.SelectedIndex == -1 ? "None"
                    : picker.Items[picker.SelectedIndex];
            };
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

        private async void RemoveFromBasketClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            // Update
            note.IsInBasket = false;
            File.WriteAllText(note.Filename, JsonConvert.SerializeObject(note));

            await Navigation.PopAsync();
        }
    }
}