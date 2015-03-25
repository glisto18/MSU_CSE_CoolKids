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
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Threading.Tasks;
using BoeingSalesApp.Common;

namespace BoeingSalesApp
{
    public sealed partial class MeetingsView : Page
    {
        private DataAccess.Repository.MeetingRepository _meetingRepo;
        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
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

        public MeetingsView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            _meetingRepo = new DataAccess.Repository.MeetingRepository();
        }
        private async Task FetchMeetings()
        {
            try
            {
                var meetings = await _meetingRepo.GetAllAsync();
                GridView gridView = (GridView)this.FindName("DatabaseMeetings");
                gridView.ItemsSource = meetings;
            }
            catch (NullReferenceException e)
            {
                //Crashed once did this and then it didnt. ***Need to revisit***
            }
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            await FetchMeetings();
        }
       

        /*****************************************************************************/


        /*****************************************************************************
         * Data container to hold variables for selection
         * Allows user to visualize+select data before it is entered in database
         ****************************************************************************/
        public class Meetin
        {
            public Meetin() { }
            public Meetin(string strt, string end, string loc, string bdy, string ldy, string sub)
            {
                Strt = strt; End = end; Loc = loc; Bdy = bdy; Ldy = ldy; Sub = sub;
            }
            public string Strt { get; set; }
            public string End { get; set; }
            public string Loc { get; set; }
            public string Bdy { get; set; }
            public string Ldy { get; set; }
            public string Sub { get; set; }
            public override string ToString()
            {
                return "Subject: " + Sub + "\nStart Time: " + Strt + "\nEnd Time: " + End + "\nLocation: " + Loc + "\nDescription: " + Bdy;
            }
        }

        /**************************************************************************************
         * AllMeets allows for multiple meetings in gridview
         * onImport parses data from Outlook created file
         * For now it is a .txt file in the pictures library (will be changed for optimization)
         * ComboBox1 is the gridview which displays all items from AllMeets in a flyout grid
         ***************************************************************************************/ 
        public System.Collections.ObjectModel.ObservableCollection<Meetin> AllMeets = new System.Collections.ObjectModel.ObservableCollection<Meetin>();
        private async void onImport(object sender, RoutedEventArgs e)
        {
            AllMeets.Clear();
            string nextLine, strt="", end="", loc="", bdy="", bdyln="", ldy="", sub=""; int count = 0;
            //await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync("appdata.txt", CreationCollisionOption.FailIfExists);
            var meetings = await KnownFolders.PicturesLibrary.GetFileAsync("appdata.txt");
            using (StreamReader reader = new StreamReader(await meetings.OpenStreamForReadAsync()))
            {
                while ((nextLine = await reader.ReadLineAsync()) != null)
                {
                    if (count == 0)
                        strt = nextLine;
                    else if (count == 1)
                        end = nextLine;
                    else if (count == 2)
                        loc = nextLine;
                    else if (count == 3)
                    {
                        if (nextLine == "---ENDBODY---")
                        {
                            bdy = bdyln;
                            bdy = bdy.Remove(bdy.Length - 1);
                            bdyln = "";
                        }
                        else
                        {
                            bdyln += nextLine + '\n';
                            count--;
                        }
                    }
                    else if (count == 4)
                        ldy = nextLine;
                    else if (count == 5)
                    {
                        count = -1;
                        sub = nextLine;
                        AllMeets.Add(new Meetin(strt, end, loc, bdy, ldy, sub));
                    }
                    else { }
                    count++;
                }
                ComboBox1.DataContext = AllMeets;
            }
            showFlyout(sender, e);
        }
        /****************************************************************************************************
         * Called by Import Meetings Button flyout button: Import
         * User chooses among all imported meetings
         * Selected meetings are saved in "Meeting" database
         ****************************************************************************************************/
        private async void ImportSelected(object sender, RoutedEventArgs e)
        {
            foreach (Meetin selectedMeetin in ComboBox1.SelectedItems)
            {
                var newMeeting = new DataAccess.Entities.Meeting
                {
                    StartTime = DateTime.Parse(selectedMeetin.Strt),
                    EndTime = DateTime.Parse(selectedMeetin.End),
                    Location = selectedMeetin.Loc,
                    Body = selectedMeetin.Bdy,
                    AllDay = Convert.ToBoolean(selectedMeetin.Ldy),
                    Subject = selectedMeetin.Sub
                };

                //var meetingRepo = new DataAccess.Repository.MeetingRepository();
                await _meetingRepo.SaveAsync(newMeeting);
            }
            await FetchMeetings();
            SelectedMeetings.Hide();
        }
        /*********************************************************************************************
         * showFlyout is called twice: once by Import Meeting and once by Add Meeting
         * showFlyout displays flyout attached to sender
         ********************************************************************************************/
        private void showFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
        /****************************************************************************************
         * For now onCreate appends to a text file the user entered fields required by Outlook
         * The fields are in a flyout which closes when "Create" is tapped
         * Will need a way to clear the text file of unnesscary meetings/mistakes
         *************************************************************************************/
        private async void onCreate(object sender, RoutedEventArgs e)
        {
            var meetings = await KnownFolders.PicturesLibrary.GetFileAsync("appdatacreation.txt");
            var entMet = new List<string>
            {
                Cstrt.Text,
                Cend.Text,
                Cloc.Text,
                Cbdy.Text,
                "---ENDBODY---",
                "False",
                Csub.Text
            };
            await Windows.Storage.FileIO.AppendLinesAsync(meetings, entMet);

            var newMeeting = new DataAccess.Entities.Meeting
            {
                StartTime = DateTime.Parse(Cstrt.Text),
                EndTime = DateTime.Parse(Cend.Text),
                Location = Cloc.Text,
                Body = Cbdy.Text,
                AllDay = false,
                Subject = Csub.Text
            };
            await _meetingRepo.SaveAsync(newMeeting);
            await FetchMeetings();

            MeetingsAdd.Hide();
        }
        /*****************************************************************************************
         * onDelete removes user selected items from the database
         * For now it appends to a .txt file in the pictures folder for Outlook addin to parse
         * ^ writes relevant lines from user selected objects
         *****************************************************************************************/
        private async void onDelete(object sender, RoutedEventArgs e)
        {
            var deletings = await KnownFolders.PicturesLibrary.GetFileAsync("appdatadeletion.txt");
            var delMet = new List<string> { };
            foreach (DataAccess.Entities.Meeting selectdelete in DatabaseMeetings.SelectedItems)
            {
                delMet.Add(selectdelete.StartTime.ToString());
                await _meetingRepo.DeleteAsync(selectdelete);
            }
            await Windows.Storage.FileIO.AppendLinesAsync(deletings, delMet);
            await FetchMeetings();
        }
    }
}
