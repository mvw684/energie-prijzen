// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics;
using System.Windows.Forms;

using EnergiePrijzen.Config;
using EnergiePrijzen.UI;

namespace EnergiePrijzen {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            var settings = SettingsReader.Read();
            ApplicationConfiguration.Initialize();
            Application.Run(new DataSelectie(settings));
            settings.Save();
        }

        private static void OnUnhandledException(
            object sender,
            UnhandledExceptionEventArgs e
        ) {
            if(Debugger.IsAttached) {
                Debugger.Break();
            }
            var exception = e.ExceptionObject as Exception;
            if (exception is not null) {
                ReportExceptionAndCause(exception);
            } else {
                Trace.WriteLine("Unknown exception");
            }
        }

        private static void ReportExceptionAndCause(Exception exception) {
            ReportException("Caught", exception);
            var cause = exception.GetBaseException();
            if((cause is not null) && (cause != exception)) {
                ReportException("Caused by", cause);
            }
        }

        private static void ReportException(string cause, Exception exception) {
            Trace.WriteLine(cause + " " + exception.GetType().Name + ": " + exception.Message);
            Trace.WriteLine(exception.StackTrace);
        }

    }
}