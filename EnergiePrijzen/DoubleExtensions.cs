// Copyright (c) 2025 mvw684

using System;
using System.Globalization;


namespace EnergiePrijzen {
    internal static class DoubleExtensions {
        private static readonly CultureInfo dutch = new CultureInfo("nl-NL");

        public static bool TryParseDutch(this string valueAsString, out double value) {
            if (double.TryParse(valueAsString, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, dutch, out value)) {
                return true;
            } else {
                throw new FormatException($"Invalid double format: {value}");
            }
        }
    }
}
