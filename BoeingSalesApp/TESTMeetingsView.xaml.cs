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
    public sealed partial class TESTMeetingsView : Page
    {
        private DataAccess.Repository.MeetingRepository _meetingRepo;
        private DataAccess.Repository.SalesBagRepository _salesbagRepo;
        private NavigationHelper navigationHelper;
        public TESTMeetingsView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            _meetingRepo = new DataAccess.Repository.MeetingRepository();
            _salesbagRepo = new DataAccess.Repository.SalesBagRepository();
        }
        /************************************************************************
         * FetchSalesbag will be caled multiple times
         * Called to refresh the view of the page
         * Shows all meetings from the backend
         *********************************************************************/
        private async Task FetchMeetings()
        {
            try
            {
                var meetings = await _meetingRepo.GetAllAsync();
                DatabaseMeetings.ItemsSource = meetings;
            }
            catch (NullReferenceException e) { }
        }
        //gets all salesbags from backend: to do
        private async Task FetchSalesbag()
        {
            try
            {
                var salesbags = await _salesbagRepo.GetAllAsync();
                GridView gridView = (GridView)this.FindName("DatabaseSalesBag");
                gridView.ItemsSource = salesbags;
            }
            catch (NullReferenceException e) { /**/ }
        }
        //onAddButton: for salesbag connection
        private async void onAddButton(object sender, RoutedEventArgs e) { await FetchSalesbag(); showFlyout(sender, e); }

        //Whenever MeetingsView page is navigated to FetchMeetings is called
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            await FetchMeetings();
        }
        //Function to handle returning to page that called MeetingsView
        private void onBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        /**************************************************************************
         * Function to Launch a meeting and enter "presentation mode"
         * Only if 1 item is selected and there is a salesbag connected
         ***************************************************************************/
        private void onLaunchMeet(object sender, RoutedEventArgs e)
        {
            if (DatabaseMeetings.SelectedItems.Count == 1)
            {
                DataAccess.Entities.Meeting ms = (DataAccess.Entities.Meeting)DatabaseMeetings.SelectedItem;
                if (ms.SalesBag!=Guid.Empty)
                {
                    this.Frame.Navigate(typeof(PresPg), ms);
                }
            }
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
            string nextLine, strt="", end="", loc="", bdy="", bdyln="", ldy="", sub=""; int count = 0, z, o;
            DateTime x; var y = await _meetingRepo.GetAllAsync(); var stayMet = new List<string> { };
            try { await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync("appdata.txt", CreationCollisionOption.FailIfExists); }
            catch { }
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

                        /*****************************************************************************
                         * Two foreach loops:
                         * 1st - Only adds meetings not already saved in database
                         * 2nd - Only allows meetings not already in "AllMeets" to be selected from
                         ***************************************************************************/
                        x = DateTime.Parse(strt);
                        z = 0; o = 0;
                        foreach (var obj in y)
                        {
                            if (x == obj.StartTime)
                                z = 1;
                        }
                        foreach (var obj2 in AllMeets)
                        {
                            if (strt == obj2.Strt)
                                o = 1;
                        }
                        if (z == 0)
                        {
                            AllMeets.Add(new Meetin(strt, end, loc, bdy, ldy, sub));
                            stayMet.Add(strt);
                            stayMet.Add(end);
                            stayMet.Add(loc);
                            stayMet.Add(bdy);
                            stayMet.Add("---ENDBODY---");
                            stayMet.Add(ldy);
                            stayMet.Add(sub);
                        }

                    }
                    else { }
                    count++;
                }
                ComboBox1.DataContext = AllMeets;
            }
            //await Windows.Storage.FileIO.WriteLinesAsync(meetings, stayMet);
            using (Windows.Storage.Streams.DataWriter dw = new Windows.Storage.Streams.DataWriter(await meetings.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)))
            {
                foreach (var hu in stayMet)
                {
                    dw.WriteString(hu);
                }
                dw.Dispose();
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
                await _meetingRepo.SaveAsync(newMeeting);
            }
            await FetchMeetings();
            SelectedMeetings.Hide();
        }
        /*********************************************************************************************
         * showFlyout is called twice: once by Import Meeting and once by Add Meeting
         * showFlyout displays one of the two xaml flyouts
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
            try { await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync("appdatacreation.txt", CreationCollisionOption.FailIfExists); }
            catch { }
            var meetings = await KnownFolders.PicturesLibrary.GetFileAsync("appdatacreation.txt");
            string dtstrt = strtDate.Date.ToString().Split(' ')[0] + " " + strtTime.Time.ToString(), dtend = strtDate.Date.ToString().Split(' ')[0] + " " + endTime.Time.ToString();
            var entMet = new List<string>
            {
                dtstrt,
                dtend,
                Cloc.Text,
                Cbdy.Text,
                "---ENDBODY---",
                "False",
                Csub.Text
            };
            await Windows.Storage.FileIO.AppendLinesAsync(meetings, entMet);
            
            var newMeeting = new DataAccess.Entities.Meeting
            {
                StartTime = DateTime.Parse(dtstrt),
                EndTime = DateTime.Parse(dtend),
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
         * recently added delete note if the meeting has one attached
         *****************************************************************************************/
        private async void onDelete(object sender, RoutedEventArgs e)
        {
            try { await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync("appdatadeletion.txt", CreationCollisionOption.FailIfExists); }
            catch { }
            var deletings = await KnownFolders.PicturesLibrary.GetFileAsync("appdatadeletion.txt");
            var delMet = new List<string> { };
            foreach (DataAccess.Entities.Meeting selectdelete in DatabaseMeetings.SelectedItems)
            {
                delMet.Add(selectdelete.StartTime.ToString());
                if(selectdelete.Note!=null)
                {
                    try
                    {
                        var notefile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(selectdelete.Note);
                        await notefile.DeleteAsync();
                    }
                    catch { }
                }
                await _meetingRepo.DeleteAsync(selectdelete);
            }
            await Windows.Storage.FileIO.AppendLinesAsync(deletings, delMet);
            await FetchMeetings();
        }
        /********************************************************************************************
         * Connects one or more meeting to a salesbag
         * Deletes old meeting from database, then saves new one with salesbag guid and name fields
         *******************************************************************************************/
        private async void onConnect(object sender, RoutedEventArgs e)
        {
            DataAccess.Entities.SalesBag salesbagto = (DataAccess.Entities.SalesBag)DatabaseSalesBag.SelectedItem;
            foreach (DataAccess.Entities.Meeting selectCon in DatabaseMeetings.SelectedItems)
            {
                var newMeeting = selectCon;
                newMeeting.SalesBag = salesbagto.ID;
                newMeeting.Name = salesbagto.Name;
                await _meetingRepo.DeleteAsync(selectCon);
                await _meetingRepo.SaveAsync(newMeeting);
            }
            await FetchMeetings();
            ConMeetings.Hide();
        }
        /*************************************************************
         * If only one meeting is selected
         * If the meeting as an associated note
         * launch the note in notepad
         ***************************************************************/
        private async void onNote(object sender, RoutedEventArgs e)
        {
            if(DatabaseMeetings.SelectedItems.Count==1)
            {
                DataAccess.Entities.Meeting meat = (DataAccess.Entities.Meeting)DatabaseMeetings.SelectedItem;
                if (meat.Note != null)
                {
                    var note = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(meat.Note);
                    await Windows.System.Launcher.LaunchFileAsync(note);
                }
            }
        }
        /*******************************************************************
         * Called every time a meetings object is selected
         * if one is selected salesbag, launch, and note are viewable
         * more than one, delete is viewable
         *********************************************************************/
        private void hideornot(object sender, RoutedEventArgs e)
        {
            if (DatabaseMeetings.SelectedItems.Count == 1)
            {
                DataAccess.Entities.Meeting ms = (DataAccess.Entities.Meeting)DatabaseMeetings.SelectedItem;
                if(ms.Note!=null)
                    noteView.Visibility = Windows.UI.Xaml.Visibility.Visible;
                SalesbagConnect.Visibility = Windows.UI.Xaml.Visibility.Visible;
                launchBut.Visibility = Windows.UI.Xaml.Visibility.Visible;
                delBut.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else if(DatabaseMeetings.SelectedItems.Count > 0)
            {
                noteView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                launchBut.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                noteView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                delBut.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                SalesbagConnect.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                launchBut.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }
    }
}
