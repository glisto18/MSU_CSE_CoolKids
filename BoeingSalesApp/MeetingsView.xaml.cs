﻿using System;
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

         //function to be called by "import meetings" button
         private void onImport()
         {
             DateTime strt, endx; string location, body, subject;
             try
             {
                 Outlook.Application outlookapp = new Outlook.Application();
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
             catch (System.Exception)
             {
                 this.Frame.Navigate(typeof(FailPage));
             }
         }
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
    }
}
