using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows ;
using System.Windows.Threading;

namespace Wpfcurd
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Wire up the event handler for unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
           
        }

        

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            // Display an error message to the user
            MessageBox.Show("An unhandled exception occurred: " + ex?.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            // Optionally, you can log the exception or perform other actions

            // Terminate the application
            Environment.Exit(1);
        }

    }

}









