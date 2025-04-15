// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace EnergiePrijzen.Config {
    public class Settings {

        private static readonly JsonSerializerOptions serializerOptions =
            new JsonSerializerOptions {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                AllowTrailingCommas = true,
                IndentCharacter = ' ',
                IndentSize = 2
            };

        public int Jaar {
            get; set;
        } = 2024;

        public string JeroenPrijzen {
            get; set;
        } = @"D:\OneDrive\Documents\Administratie\Huis\Electriciteits prijzen en keuzes\Prijzen";

        
        public string SolarEdge {
            get; set;
        } = @"D:\OneDrive\Documents\Administratie\Huis\Electriciteits prijzen en keuzes\SolarEdge";

        public string SlimmeMeter {
            get;set;
        } = @"D:\OneDrive\Documents\Administratie\Huis\Electriciteits prijzen en keuzes\SlimmeMeterPortal";

        public string ToJson() {
            return JsonSerializer.Serialize(this, serializerOptions);
        }

        public static bool FromJson(string json, [NotNullWhen(true)] out Settings? settings) {
            try {
                settings = JsonSerializer.Deserialize<Settings>(json);
                return settings != null;
            } catch (Exception) {
                if(Debugger.IsAttached) {
                    Debugger.Break();
                }
                settings = null;
                return false;
            }
        }

    }
}
