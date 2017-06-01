using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantsFilterPage : ViewHelpers
    {
        Dictionary<string, string> groupOptions = new Dictionary<string, string> { { "All", "All" }, { "Aquatic Herbs", "Aquatic" }, { "Dicot Herbs", "Dicot" } };
        Picker groupPicker = new Picker();
        Entry groupEntry = new Entry { Style = Application.Current.Resources["filterEntry"] as Style, Placeholder = "Group" };

        public WetlandPlantsFilterPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid {
                Padding = new Thickness(20, Device.OnPlatform(30, 20, 20), 20, 20),
                BackgroundColor = Color.FromHex("88000000")
            };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            Grid filtersContainer = new Grid { VerticalOptions = LayoutOptions.FillAndExpand };
            filtersContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            filtersContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) });

            // Name Entry
            Label nameLabel = new Label { Text = "Name", Style = Application.Current.Resources["filterLabel"] as Style };
            filtersContainer.Children.Add(nameLabel, 0, 0);
            Entry nameEntry = new Entry { Placeholder = "Scientific or Common Name", Style = Application.Current.Resources["filterEntry"] as Style };
            filtersContainer.Children.Add(nameEntry, 1, 0);
            filtersContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Family Entry
            Label familyLabel = new Label { Text = "Family", Style = Application.Current.Resources["filterLabel"] as Style };
            filtersContainer.Children.Add(familyLabel, 0, 1);
            Entry familyEntry = new Entry { Placeholder = "Scientific Name", Style = Application.Current.Resources["filterEntry"] as Style };
            filtersContainer.Children.Add(familyEntry, 1, 1);
            filtersContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Group Picker (custom picker)
            Label groupLabel = new Label { Text = "Group", Style = Application.Current.Resources["filterLabel"] as Style };
            filtersContainer.Children.Add(groupLabel, 0, 2);
            filtersContainer.Children.Add(groupEntry, 1, 2);
            Picker groupPicker = new Picker { Title = "All" };
            groupPicker.IsVisible = false;
            groupPicker.SelectedIndex = 0;
            foreach (string option in groupOptions.Keys) { groupPicker.Items.Add(option); }
            
            //if (Device.OS == TargetPlatform.iOS)
            //    groupPicker.Unfocused += async (s, e) => { groupEntry.Text = groupPicker.Items[groupPicker.SelectedIndex]; await Task.Delay(100); };
            //else
            //    groupPicker.SelectedIndexChanged += SortItems;

            groupEntry.Focused += async (s, e) => { groupEntry.Unfocus(); await Task.Delay(100); groupPicker.Focus(); };
            filtersContainer.Children.Add(groupPicker, 1, 2);
            filtersContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            innerContainer.Children.Add(filtersContainer, 0, 0);

            // Add dismiss button
            Button dismissButton = new Button
            {
                Style = Application.Current.Resources["outlineButton"] as Style,
                Text = "CLOSE",
                BorderRadius = Device.OnPlatform(0, 1, 0)
            };
            dismissButton.Clicked += OnDismissButtonClicked;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(dismissButton, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
        
    }
}
