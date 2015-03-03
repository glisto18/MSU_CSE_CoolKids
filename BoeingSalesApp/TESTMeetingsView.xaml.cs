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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BoeingSalesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TESTMeetingsView : Page
    {
        public TESTMeetingsView()
        {
            this.InitializeComponent();
        }
        private void onBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
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
        }
    }
}
