// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace EnergiePrijzen.Config {
    internal static class SettingsReader {

        private static DirectoryInfo settingsDirectory = 
            new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EnergiePrijzen"));

        private const string configFileName = "Settings.json";

        static SettingsReader() {
            if (!settingsDirectory.Exists) {
                settingsDirectory.Create();
            }
            Process.Start("explorer.exe", settingsDirectory.FullName);
        }

        public static void Save(this Settings settings) {
            var file = new FileInfo(Path.Combine(settingsDirectory.FullName, configFileName));
            Save(settings, file);
        }

        private static void Save(this Settings settings, FileInfo file) {
            var data = settings.ToJson();
            File.WriteAllText(file.FullName, data, Encoding.UTF8);
        }

        private static Settings Load(FileInfo file) {
            if(file.Exists) {
                var data = File.ReadAllText(file.FullName, Encoding.UTF8);
                if(Settings.FromJson(data, out var settings)) {
                    return settings;
                }
            }
            return new Settings();
        }

        public static Settings Read() {
            var file = new FileInfo(Path.Combine(settingsDirectory.FullName, configFileName));
            return Load(file);
        }
    }
}
