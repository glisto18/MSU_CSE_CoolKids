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
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;

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


        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            lblStatus.Text = "Clicked";
        }




         //function to be called by "import meetings" button
         private void onImport(object sender, RoutedEventArgs e)
         {/*
             DateTime strt, endx; string location, body, subject;
             //try
             {
                 //Guid g = Guid.Parse("00063001-0000-0000-C000-000000000046");
                 Outlook.Application outlookapp = new Outlook.Application(); //try http://www.dimastr.com/redemption/home.htm
                 //Outlook.NameSpace mapiNamespace = outlookapp.GetNamespace("MAPI");
                 //Outlook.MAPIFolder calender = mapiNamespace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);
                 Outlook.MAPIFolder calender = outlookapp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);
                 Outlook.Items outlookCalendarItems = calender.Items;
                 foreach(Outlook.AppointmentItem meeting in outlookCalendarItems)
                 {
                     //display meetings somehow
                     strt = meeting.Start;
                     endx = meeting.End;
                     location = meeting.Location;
                     body = meeting.Body;
                     subject = meeting.Subject;
                     //add to database somehow, but only selected how do i tell if they are selected?

                 }
             }
            //catch (System.Exception)
             {
                 //this.Frame.Navigate(typeof(FailPage));
             }
             try
             {
                 // create an application instance of Outlook
                 Outlook.Application oApp = new Outlook.Application();
             }
             catch (System.Exception ex)
             {
                 try
                 {
                     // get Outlook in another way
                    // Outlook.Application oApp = Marshal.GetActiveObject("Outlook.Application");
                 }
                 catch (System.Exception ex2)
                 {
                     // try some other way to get the object
                     //Outlook.Application oApp = Activator.CreateInstance(Type.GetTypeFromProgID("Outlook.Application"));
                 }
             }*/
         }
        //save appoointment based on select fields (there are others) to default folder, need xaml text fields
         private void addAppointment(DateTime strt, DateTime endx, string location, string body, string subject)
         {
             try
             {
                 Outlook.Application outlookapp = new Outlook.Application();
                 Outlook.AppointmentItem newAppointment = outlookapp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olAppointmentItem);
                 newAppointment.Start = strt; //DateTime.Parse("6/11/2007 12:00 AM");
                 newAppointment.End = endx;
                 newAppointment.Location = location;
                 newAppointment.Body = body;
                 newAppointment.Subject = subject;
                 newAppointment.Save();
                 newAppointment.Display(true);
             }
             catch (System.Exception)
             {
                 this.Frame.Navigate(typeof(FailPage));
             }
         }
        //Display available appointments, slected appointemnt is connected to index number, delete based on that value "ind"
        private void deleteAppointment(int ind)
         {
             Outlook.Application outlookapp = new Outlook.Application();
             Outlook.MAPIFolder calender = outlookapp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);
             Outlook.Items calendarItems = calender.Items; //.Delete()
         }
    }
}
/*
namespace OutlookAddIn1
{
    class Sample
    {
        Outlook.Application GetApplicationObject()
        {

            Outlook.Application application = null;

            // Check whether there is an Outlook process running.
            if (Process.GetProcessesByName("OUTLOOK").Count() > 0)
            {

                // If so, use the GetActiveObject method to obtain the process and cast it to an Application object.
                application = Marshal.GetActiveObject("Outlook.Application") as Outlook.Application;
            }
            else
            {

                // If not, create a new instance of Outlook and log on to the default profile.
                application = new Outlook.Application();
                Outlook.NameSpace nameSpace = application.GetNamespace("MAPI");
                nameSpace.Logon("", "", Missing.Value, Missing.Value);
                nameSpace = null;
            }

            // Return the Outlook Application object.
            return application;
        }

    }
}
*/