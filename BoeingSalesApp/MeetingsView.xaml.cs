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
//using Microsoft.Office.Interop.Outlook

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BoeingSalesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MeetingsView : Page
    {
        public MeetingsView()
        {
            this.InitializeComponent();
        }

        private void onBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void tap_to_launch(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(launchMeeting));
        }
        /* 
         * If returned false load page or dialogue  box to say outlook is not installed
        public static bool IsOutlookInstalled()
        {
            try
            {
                Type type = Type.GetTypeFromCLSID(new Guid("0006F03A-0000-0000-C000-000000000046")); //Outlook.Application
                if (type == null) return false;
                object obj = Activator.CreateInstance(type);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                return true;
            }
            catch (COMException)
            {
                return false;
            }
        }
         * function to be called by "import meetings" button
         * private void onImport()
         * {
         *      if(IsOutlookInstalled()) this.Frame.Navigate(typeof(SuccessPage));
         *      else this.Frame.Navigate(typeof(FailPage));
         *      Microsoft.Office.Interop.Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
         *      Microsoft.Office.Interop.Outlook.NameSpace mapiNamespace= oApp.GetNamespace("MAPI");
         *      Microsoft.Office.Interop.Outlook.MAPIFolder CalendarFolder= mapiNamespace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderCalendar);
         *      Microsoft.Office.Interop.Outlook.Items outlookCalendarItems = CalendarFolder.Items;
         *      
         * // foreach (Outlook.AppointmentItem item in outlookCalendarItems)
         * 
         * }
         * */
    }
}
