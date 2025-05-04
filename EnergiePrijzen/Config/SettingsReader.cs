// Copyright (c) 2025 mvw684

using System;
using System.IO;
using System.Text;

namespace EnergiePrijzen.Config {
    internal static class SettingsReader {

        private static readonly DirectoryInfo settingsDirectory = 
            new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EnergiePrijzen"));

        private const string configFileName = "Settings.json";

        internal static DirectoryInfo SettingsDirectory => settingsDirectory;

        static SettingsReader() {
            if (!settingsDirectory.Exists) {
                settingsDirectory.Create();
            }
        }

        public static void Save(this Settings settings) {
            var file = new FileInfo(Path.Combine(settingsDirectory.FullName, configFileName));
            Save(settings, file);
        }

        private static void Save(this Settings settings, FileInfo file) {
            string data = settings.ToJson();
            File.WriteAllText(file.FullName, data, Encoding.UTF8);
        }

        private static Settings Load(FileInfo file) {
            if(file.Exists) {
                string data = File.ReadAllText(file.FullName, Encoding.UTF8);
                if(Settings.FromJson(data, out Settings? settings)) {
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
