using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TestUXMeetic.Resources;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using ImageTools.IO.Png;
using Nokia.Graphics.Imaging;
using Microsoft.Xna.Framework.Media;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace TestUXMeetic
{
    public partial class MainPage : PhoneApplicationPage
    {
        private WriteableBitmap _cartoonImageBitmap = null;


        private bool isSettingsOpen = false;
        private bool isContactOpen = false;
        private TranslateTransform move = new TranslateTransform();

        private TransformGroup panelTransforms = new TransformGroup();

        private TranslateTransform settingmove = new TranslateTransform();
        private TransformGroup settingsTransforms = new TransformGroup();

        private TranslateTransform contactmove = new TranslateTransform();
        private TransformGroup contactTransforms = new TransformGroup();

        public static Double _realScaleFactor = (Double)Application.Current.Host.Content.ScaleFactor / 100.0;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            panelTransforms.Children.Add(move);

            settingsTransforms.Children.Add(settingmove);

            contactTransforms.Children.Add(contactmove);

            LayoutRoot.RenderTransform = panelTransforms;

            SettingsPane.RenderTransform = settingsTransforms;

            ContactPane.RenderTransform = contactTransforms;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _cartoonImageBitmap = new WriteableBitmap((int)ImageBlur.Width, (int)ImageBlur.Height);
            base.OnNavigatedTo(e);
            ListBoxItem myItem = new ListBoxItem();
            imageBox.Items.Add(myItem);
        }

        private void LayoutRoot_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {

        }

        private void LayoutRoot_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            var posX = LayoutRoot.TransformToVisual(Container).Transform(new Point(0, 0)).X;
            //Debug.WriteLine("pos X : " + posX + " move X : " + move.X);
            SettingsPane.Visibility = posX > 200 ? Visibility.Visible : Visibility.Collapsed;
            ContactPane.Visibility = posX > 200 ? Visibility.Collapsed : Visibility.Visible;



            if (posX >= 200 && posX <= 590)
            {

                move.X += e.DeltaManipulation.Translation.X;
                //  if (move.X > 380) move.X = 380;
                //Debug.WriteLine("SettinsMoveX = " + settingmove.X + " translation.X/5 = " + (e.DeltaManipulation.Translation.X / 5));
                settingmove.X += (e.DeltaManipulation.Translation.X / 5);

                if (settingmove.X > 100) settingmove.X = 100;

                Double opacityValue = move.X / 380;
                if (opacityValue < 0.35) opacityValue = 0.35;
                SettingsPane.Opacity = opacityValue;


            }
            else if (posX <= 200 && posX >= -190)
            {
                move.X += e.DeltaManipulation.Translation.X;
                // if (move.X < -380) move.X = -380;

                contactmove.X += (e.DeltaManipulation.Translation.X / 5);
            }


            var myBlurPos = posX + 280;

            var opacValue = myBlurPos / 400;
            if (opacValue < 0) opacValue = 0;
            if (opacValue > 1) opacValue = 1;


            Debug.WriteLine("my blur pos" + myBlurPos + " opacvalue : " + opacValue);
            ImageBlur.Opacity = opacValue;
            RectBlur.Opacity = (double)(opacValue / 2);

        }

        private void OpenSettings()
        {

            SettingsOpenState.Storyboard = null;
            Storyboard OpenStateStory = new Storyboard();

            DoubleAnimation animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                To = 380 - move.X,
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Projection).(PlaneProjection.GlobalOffsetX)"));
            Storyboard.SetTargetName(animation, "LayoutRoot");


            DoubleAnimation animationSettings = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                To = 100 - settingmove.X,
            };
            Storyboard.SetTargetProperty(animationSettings, new PropertyPath("(UIElement.Projection).(PlaneProjection.GlobalOffsetX)"));
            Storyboard.SetTargetName(animationSettings, "SettingsPane");

            OpenStateStory.Children.Add(animation);
            OpenStateStory.Children.Add(animationSettings);


            SettingsOpenState.Storyboard = OpenStateStory;

            OpenStateStory.Begin();
        }

        private void OpenContact()
        {
            SettingsOpenState.Storyboard = null;
            Storyboard OpenContactStory = new Storyboard();


            DoubleAnimation animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                To = -380 - move.X,
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Projection).(PlaneProjection.GlobalOffsetX)"));
            Storyboard.SetTargetName(animation, "LayoutRoot");


            DoubleAnimation animationContact = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                To = -100 - contactmove.X,
            };

            Storyboard.SetTargetProperty(animationContact, new PropertyPath("(UIElement.Projection).(PlaneProjection.GlobalOffsetX)"));
            Storyboard.SetTargetName(animationContact, "ContactPane");

            OpenContactStory.Children.Add(animation);
            OpenContactStory.Children.Add(animationContact);


            SettingsOpenState.Storyboard = OpenContactStory;

            OpenContactStory.Begin();
        }
        private void CloseSidePanels()
        {
            SettingsClosedState.Storyboard = null;
            Storyboard CloseStateStory = new Storyboard();


            DoubleAnimation animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                To = -move.X,



            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Projection).(PlaneProjection.GlobalOffsetX)"));
            Storyboard.SetTargetName(animation, "LayoutRoot");


            DoubleAnimation animationSettings = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                To = -settingmove.X,



            };
            Storyboard.SetTargetProperty(animationSettings, new PropertyPath("(UIElement.Projection).(PlaneProjection.GlobalOffsetX)"));
            Storyboard.SetTargetName(animationSettings, "SettingsPane");

            DoubleAnimation animationContact = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                To = -contactmove.X,



            };
            Storyboard.SetTargetProperty(animationContact, new PropertyPath("(UIElement.Projection).(PlaneProjection.GlobalOffsetX)"));
            Storyboard.SetTargetName(animationContact, "ContactPane");


            CloseStateStory.Children.Add(animation);
            CloseStateStory.Children.Add(animationSettings);
            CloseStateStory.Children.Add(animationContact);


            SettingsClosedState.Storyboard = CloseStateStory;

            CloseStateStory.Begin();

        }

        private void LayoutRoot_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            //if (move.X > 240)
            //{
            //    move.X = 380;
            //    settingmove.X = 100;
            //    SettingsPane.Opacity = 1;
            //}
            //<Storyboard>
            //            <DoubleAnimation Duration="0" To="380" Storyboard.TargetProperty="(UIElement.Projection).(PlaneProjection.GlobalOffsetX)" Storyboard.TargetName="LayoutRoot" d:IsOptimized="True"/>
            //            <DoubleAnimation Duration="0" To="100" Storyboard.TargetProperty="(UIElement.Projection).(PlaneProjection.GlobalOffsetX)" Storyboard.TargetName="SettingsPane" d:IsOptimized="True"/>
            //        </Storyboard>

            var posX = LayoutRoot.TransformToVisual(Container).Transform(new Point(0, 0)).X;
            if (posX > 440)
                OpenSettings();
            else if (posX < -40)
                OpenContact();
            else
                CloseSidePanels();


            //VisualStateManager.GoToState(this, "SettingsOpenState", true);
            //isSettingsOpen = true;

            //OpenStateStory.

            //SettingsOpenState.Storyboard = 


            //Storyboard.SetTarget(animation,move);
            //Storyboard.SetTargetProperty(animation, new PropertyPath(TranslateTransform.XProperty));

        }

        private void toSettings_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SettingsPane.Visibility = Visibility.Visible;
            ContactPane.Visibility = Visibility.Collapsed;
            if (isSettingsOpen)
            {
                CloseSidePanels();
                isSettingsOpen = false;
            }
            else
            {
                OpenSettings();
                isSettingsOpen = true;
            }

        }

        private void toContact_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SettingsPane.Visibility = Visibility.Collapsed;
            ContactPane.Visibility = Visibility.Visible;
            if (isContactOpen)
            {
                CloseSidePanels();
                isContactOpen = false;
            }
            else
            {
                OpenContact();
                isContactOpen = true;
            }
        }



        private void Setting_Contact_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            OpenContact();
            isContactOpen = true;
            isSettingsOpen = false;
            SettingsPane.Visibility = Visibility.Collapsed;
            ContactPane.Visibility = Visibility.Visible;
        }

        private void toBlurPage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BlurPage.xaml", UriKind.Relative));
        }

        private async void testBlur_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var rootElement = ContactPane as FrameworkElement;

            // BitmapImage myIm = new BitmapImage();
            //SaveJPGToLib(rootElement);
            WriteableBitmap myWB = new WriteableBitmap((int)rootElement.ActualWidth, (int)rootElement.ActualHeight);
            myWB.Render(rootElement, new MatrixTransform());
            myWB.Invalidate();

            //      byte[] bytes = myWB.ToByteArray();

            //Stream stream = App.GetResourceStream(new Uri("Assets/bonbon.jpg", UriKind.Relative)).Stream;
            using (var stream = new MemoryStream())
            {
                var encoder = new PngEncoder();
                encoder.IsWritingUncompressed = true;
                var img = ImageTools.ImageExtensions.ToImage(myWB);
                encoder.Encode(img, stream);
                // myWB.SaveJpeg(stream, myWB.PixelWidth, myWB.PixelHeight, 0, 100);
                //await stream.WriteAsync(myWB.ToByteArray(), 0, bytes.Count());
                stream.Seek(0, SeekOrigin.Begin);
                var backgroundSource = new StreamImageSource(stream);
                var filterEffect = new FilterEffect(backgroundSource);

                BlurFilter blurFilter = new BlurFilter();


                blurFilter.KernelSize = 30;
                filterEffect.Filters = new[] { blurFilter };

                var renderer = new WriteableBitmapRenderer(filterEffect, _cartoonImageBitmap);

                _cartoonImageBitmap = await renderer.RenderAsync();

                ImageBlur.Source = _cartoonImageBitmap;
                ImageBlur.Visibility = Visibility.Visible;

                Debug.WriteLine("test");
            }
        }

        public static void SaveJPGToLib(FrameworkElement element)
        {
            WriteableBitmap bmpCurrentScreenImage = new WriteableBitmap((int)element.ActualWidth, (int)element.ActualHeight);
            bmpCurrentScreenImage.Render(element, new MatrixTransform());
            bmpCurrentScreenImage.Invalidate();

            using (var stream = new MemoryStream())
            {
                // Save the picture to the Windows Phone media library.
                bmpCurrentScreenImage.SaveJpeg(stream, bmpCurrentScreenImage.PixelWidth, bmpCurrentScreenImage.PixelHeight, 0, 100);
                stream.Seek(0, SeekOrigin.Begin);

                string filename = "screenshots\\" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) + ".jpg";
                new MediaLibrary().SavePicture(filename, stream);
            }

            MessageBox.Show("Saved in your media library!", "Done", MessageBoxButton.OK);
        }





        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}