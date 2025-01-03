using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GymManagement.UWP.Views.Dashboard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            this.InitializeComponent();
            UpcomingWorkoutsList.SelectedIndex = -1;
            PastWorkoutsList.SelectedIndex = -1;
        }


        private void PastWorkoutsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = PastWorkoutsList.SelectedItem as ListBoxItem;

            if (selectedItem != null)
            {
                // Update the DetailsContent with the selected workout details
                DetailsContent.Content = new StackPanel
                {
                    Children =
                    {
                        // "Details" header
                        new TextBlock
                        {
                            Text = "Details",
                            FontSize = 42,
                            Margin = new Thickness(0, 0, 0, 20),
                            HorizontalAlignment = HorizontalAlignment.Center
                        },
                            
                        // Date section
                        new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(0, 0, 0, 10),
                            Children =
                            {
                                new SymbolIcon
                                {
                                    Symbol = Symbol.Calendar,
                                    Margin = new Thickness(0, 0, 10, 0),
                                    Width = 50,
                                    Height = 50
                                },
                                new TextBlock
                                {
                                    Text = "07.11.2024",
                                    VerticalAlignment = VerticalAlignment.Center,
                                    FontSize = 24
                                }
                            }
                        },

                        // Time section
                        new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(0, 0, 0, 10),
                            Children =
                            {
                                new SymbolIcon
                                {
                                    Symbol = Symbol.Clock,
                                    Margin = new Thickness(0, 0, 10, 0),
                                    Width = 50,
                                    Height = 50
                                },
                                new TextBlock
                                {
                                    Text = "11:00 - 12:00",
                                    VerticalAlignment = VerticalAlignment.Center,
                                    FontSize = 24
                                }
                            }
                        },

                        // Trainer section
                        new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(0, 0, 0, 10),
                            Children =
                            {
                                new SymbolIcon
                                {
                                    Symbol = Symbol.Contact,
                                    Margin = new Thickness(0, 0, 10, 0),
                                    Width = 50,
                                    Height = 50
                                },
                                new TextBlock
                                {
                                    Text = "Ann Smith",
                                    VerticalAlignment = VerticalAlignment.Center,
                                    FontSize = 24
                                }
                            }
                        },

                        // Trainer description section
                        new StackPanel
                        {
                            Margin = new Thickness(0, 20, 0, 20),
                            Children =
                            {
                                new TextBlock
                                {
                                    Text = "Weight Loss Coach",
                                    FontWeight = Windows.UI.Text.FontWeights.SemiBold,
                                    FontSize = 16,
                                    Padding = new Thickness(0, 0, 0, 20)
                                },
                                new StackPanel
                                {
                                    Orientation = Orientation.Horizontal,
                                    Children =
                                    {
                                        new Image
                                        {
                                            Source = new BitmapImage(new Uri("ms-appx:///Assets/image.png")),
                                            Width = 70,
                                            Height = 70,
                                            Margin = new Thickness(0, 10, 10, 10)
                                        },
                                        new TextBlock
                                        {
                                            Text = "Ann Smith, Expert trainer, 15 years of experience",
                                            TextWrapping = TextWrapping.Wrap,
                                            FontSize = 16,
                                            HorizontalAlignment = HorizontalAlignment.Stretch,
                                            VerticalAlignment = VerticalAlignment.Top
                                        }
                                    }
                                }
                            }
                        },

                        // Rating section
                        new TextBlock
                        {
                            Text = "Leave your rating for the workout",
                            Margin = new Thickness(0, 0, 0, 10),
                            FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Stretch
                        },
                        new RatingControl
                        {
                            //AutomationPropertiesName = "Simple RatingControl",
                            IsClearEnabled = false,
                            IsReadOnly = false,
                                FontSize = 96,
                                Width = 110
                            }
                        }
                };

            }
        }

        private void UpcomingWorkoutsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = UpcomingWorkoutsList.SelectedItem as ListBoxItem;

            if (selectedItem != null)
            {
                // Update the DetailsContent with the selected workout details
                DetailsContent.Content = new StackPanel
                {
                    Children =
                    {
                        // "Details" header
                        new TextBlock
                        {
                            Text = "Details",
                            FontSize = 42,
                            Margin = new Thickness(0, 0, 0, 20),
                            HorizontalAlignment = HorizontalAlignment.Center
                        },

                        // Date section
                        new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(0, 0, 0, 10),
                            Children =
                            {
                                new SymbolIcon
                                {
                                    Symbol = Symbol.Calendar,
                                    Margin = new Thickness(0, 0, 10, 0),
                                    Width = 50,
                                    Height = 50
                                },
                                new TextBlock
                                {
                                    Text = "07.11.2024",
                                    VerticalAlignment = VerticalAlignment.Center,
                                    FontSize = 24
                                }
                            }
                        },

                        // Time section
                        new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(0, 0, 0, 10),
                            Children =
                            {
                                new SymbolIcon
                                {
                                    Symbol = Symbol.Clock,
                                    Margin = new Thickness(0, 0, 10, 0),
                                    Width = 50,
                                    Height = 50
                                },
                                new TextBlock
                                {
                                    Text = "11:00 - 12:00",
                                    VerticalAlignment = VerticalAlignment.Center,
                                    FontSize = 24
                                }
                            }
                        },

                        // Trainer section
                        new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(0, 0, 0, 10),
                            Children =
                            {
                                new SymbolIcon
                                {
                                    Symbol = Symbol.Contact,
                                    Margin = new Thickness(0, 0, 10, 0),
                                    Width = 50,
                                    Height = 50
                                },
                                new TextBlock
                                {
                                    Text = "Ann Smith",
                                    VerticalAlignment = VerticalAlignment.Center,
                                    FontSize = 24
                                }
                            }
                        },

                        // Trainer description section
                        new StackPanel
                        {
                            Margin = new Thickness(0, 20, 0, 20),
                            Children =
                            {
                                new TextBlock
                                {
                                    Text = "Weight Loss Coach",
                                    FontWeight = Windows.UI.Text.FontWeights.SemiBold,
                                    FontSize = 16,
                                    Padding = new Thickness(0, 0, 0, 20)
                                },
                                new StackPanel
                                {
                                    Orientation = Orientation.Horizontal,
                                    Children =
                                    {
                                        new Image
                                        {
                                            Source = new BitmapImage(new Uri("ms-appx:///Assets/image.png")),
                                            Width = 70,
                                            Height = 70,
                                            Margin = new Thickness(0, 10, 10, 10)
                                        },
                                        new TextBlock
                                        {
                                            Text = "Ann Smith, Expert trainer, 15 years of experience",
                                            TextWrapping = TextWrapping.Wrap,
                                            FontSize = 16,
                                            HorizontalAlignment = HorizontalAlignment.Stretch,
                                            VerticalAlignment = VerticalAlignment.Top
                                        }
                                    }
                                }
                            }
                        }

                    }

                };
            }

        }
    }

}
