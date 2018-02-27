using PortableApp.Helpers;
using PortableApp.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace PortableApp
{
    public partial class WetlandGlossaryPage : ViewHelpers
    {
        ObservableCollection<Grouping<string, WetlandGlossary>> termsGrouped;
        ObservableCollection<WetlandGlossary> terms;
        ListView termsList;
        List<string> jumpList;

        public WetlandGlossaryPage()
        {
            // Get terms, sort them and assign them to new collection
            terms = new ObservableCollection<WetlandGlossary>(App.WetlandGlossaryRepoLocal.GetAllWetlandTerms());
            var sortedTerms = from term in terms orderby term.name group term by term.firstInitial into termGroup select new Grouping<string, WetlandGlossary>(termGroup.Key, termGroup);
            termsGrouped = new ObservableCollection<Grouping<string, WetlandGlossary>>(sortedTerms);

            // Create jump list from termsGrouped
            jumpList = new List<string>();
            foreach (Grouping<string, WetlandGlossary> index in termsGrouped) { jumpList.Add(index[0].firstInitial); };

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add navigation bar to inner container
            HeaderNavigationOptions navOptions = new HeaderNavigationOptions { titleText = "Glossary", backButtonVisible = true, homeButtonVisible = true };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            // Create ListView container
            RelativeLayout listViewContainer = new RelativeLayout {
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
            };

            // Instantiate terms ListView and add to container
            termsList = new ListView
            {
                BackgroundColor = Color.Transparent,
                ItemsSource = termsGrouped,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("Key"),
                HasUnevenRows = true
            };
            termsList.GroupHeaderTemplate = new DataTemplate(typeof(HeaderCell));
            var termTemplate = new DataTemplate(typeof(GlossaryTermTemplate));
            termTemplate.SetBinding(TextCell.TextProperty, "name");
            termTemplate.SetBinding(TextCell.DetailProperty, "definition");
            termsList.ItemTemplate = termTemplate;
            
            listViewContainer.Children.Add(termsList,
                Constraint.RelativeToParent((parent) => { return parent.X; }),
                Constraint.RelativeToParent((parent) => { return parent.Y - 50; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * .9; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; })
            );

            // Add jump list to right side
            StackLayout jumpListContainer = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            foreach (string letter in jumpList)
            {
                Label letterLabel = new Label { Text = letter, Style = Application.Current.Resources["jumpListLetter"] as Style };
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) => {
                    var firstRecordMatchingLetter = terms.Where(x => x.name[0].ToString() == letter).FirstOrDefault();
                    termsList.ScrollTo(firstRecordMatchingLetter, ScrollToPosition.Start, true);
                };
                letterLabel.GestureRecognizers.Add(tapGestureRecognizer);
                jumpListContainer.Children.Add(letterLabel);
            }

            listViewContainer.Children.Add(jumpListContainer,
                Constraint.RelativeToParent((parent) => { return parent.Width * .9; }),
                Constraint.RelativeToParent((parent) => { return parent.Y - 50; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * .1; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; })
            );
            
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            innerContainer.Children.Add(listViewContainer, 0, 1);
            
            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }        
    }

    // Custom ListView cell for glossary terms
    public class GlossaryTermTemplate : ViewCell
    {
        public GlossaryTermTemplate()
        {
            // Construct StackLayout, the cell container
            StackLayout cell = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Transparent,
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 10)
            };

            // Add name
            Label termName = new Label { TextColor = Color.White, FontSize = 12, FontAttributes = FontAttributes.Bold };
            termName.SetBinding(Label.TextProperty, new Binding("name"));
            cell.Children.Add(termName);

            // Add name
            Label termDefinition = new Label { TextColor = Color.White, FontSize = 12 };
            termDefinition.SetBinding(Label.TextProperty, new Binding("definition"));
            cell.Children.Add(termDefinition);
            
            View = cell;
        }

    }

    // Custom header cell for ListView
    public class HeaderCell : ViewCell
    {
        public HeaderCell()
        {
            this.Height = 25;
            var title = new Label {
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center
            };
            Device.OnPlatform(iOS: () => title.TextColor = Color.Black, Android: () => title.TextColor = Color.White);
            title.SetBinding(Label.TextProperty, "Key");
            View = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 25,
                BackgroundColor = Color.FromHex("00000000"),
                Padding = 5,
                Orientation = StackOrientation.Horizontal,
                Children = { title }
            };
        }
    }
}
