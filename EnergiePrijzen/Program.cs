// Copyright (c) 2025 mvw684

using System;
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
            
            // TODO: remove once experimentation is completed
            new Experiment().Run();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            using (new ExceptionReporting()) {
                Settings settings = SettingsReader.Read();
                ApplicationConfiguration.Initialize();
                Application.Run(new DataSelectie(settings));
            }
        }
    }
}