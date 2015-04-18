using BoeingSalesApp.Common;
using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BoeingSalesApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SurveyView : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private string _meetingName;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public SurveyView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _meetingName = (string)e.Parameter;
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void genpdf(String rating, String comment, String contact)
        {
            try
            {
                var generator = new Utility.PdfGenerator(rating, comment, contact);
                var meetingNameForFileName = Regex.Replace(_meetingName, @"[^a-zA-Z0-9]", "");
                var pdfName = string.Format("{0}_Survey_{1:yy-MM-dd_hh_mm}.pdf", meetingNameForFileName, DateTime.Now);
                generator.UpdateName(pdfName);
                await generator.gen();
            }
            catch
            {
            }
        }

        private void Submit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string rating = string.Empty;
            if ((ratingA as RadioButton).IsChecked.Value) 
            {
                rating = "A";
            }
            else if ((ratingB as RadioButton).IsChecked.Value)
            {
                rating = "B";
            }
            else if ((ratingC as RadioButton).IsChecked.Value)
            {
                rating = "C";
            }
            else if ((ratingD as RadioButton).IsChecked.Value)
            {
                rating = "D";
            }
            else if ((ratingF as RadioButton).IsChecked.Value)
            {
                rating = "F";
            }
            else
            {
                rating = "--";
            }

            String comment = (CommentBox as TextBox).Text;
            String contact = (ContactField as TextBox).Text;
            genpdf(rating, comment, contact);
            this.Frame.Navigate(typeof(TESTMeetingsView));
        }

        private void backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TESTMeetingsView));
        }

    }
}
