using Client.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Client.UserControls
{
    /// <summary>
    /// Interaction logic for Movie.xaml
    /// </summary>
    public partial class MovieUserControl : UserControl
    {
        public static DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(Movie), typeof(MovieUserControl));

        public Movie Movie
        {
            get { return (Movie)GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        public MovieUserControl()
        {
            InitializeComponent();
        }
    }
}
