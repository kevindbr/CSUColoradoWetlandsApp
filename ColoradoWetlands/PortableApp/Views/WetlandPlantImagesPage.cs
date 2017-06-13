﻿using CarouselView.FormsPlugin.Abstractions;
using PortableApp.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantImagesPage : ViewHelpers
    {

        public WetlandPlantImagesPage(WetlandPlant plant, ObservableCollection<WetlandPlant> plants)
        {

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            NavigationOptions navOptions = new NavigationOptions { titleText = plant.scinamenoauthorstripped, backButtonVisible = true, nextAndPreviousVisible = true, plant = plant, plants = plants };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);
            
            CarouselViewControl carouselControl = new CarouselViewControl();
            carouselControl.ItemsSource = plant.Images;

            DataTemplate imageTemplate = new DataTemplate(() =>
            {
                Grid cell = new Grid { BackgroundColor = Color.FromHex("88000000") };
                cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                cell.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
                cell.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                // Add image credit
                var imageCredit = new Label { FontSize = 16, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0, 10, 0, 0) };
                imageCredit.SetBinding(Label.TextProperty, new Binding("ImageCredit"));
                cell.Children.Add(imageCredit, 0, 0);

                // Add image
                var image = new Image { Aspect = Aspect.AspectFit, Margin = new Thickness(10, 0, 10, 0) };
                image.SetBinding(Image.SourceProperty, new Binding("ImagePath"));
                cell.Children.Add(image, 0, 1);

                return cell;
            });

            carouselControl.ItemTemplate = imageTemplate;
            carouselControl.Position = 0;
            carouselControl.InterPageSpacing = 10;
            carouselControl.Orientation = CarouselViewOrientation.Horizontal;
            
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(carouselControl, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

    }
    
}
