// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics.CodeAnalysis;

namespace EnergiePrijzen.Data {
    public static class TimeStampExtensions {

        internal const string DateFormat = "yyyy-MM-dd";

        private readonly static string[] dateFormats = [TimeStampExtensions.DateFormat];


        public static DateTime ToStamp(this DateTime dateTime) {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, 0, DateTimeKind.Local);
        }

        public static bool TryParseDate(this string dateString, [NotNullWhen(true)] out TimeStamp? timeStamp) {
            if(DateTime.TryParseExact(dateString, dateFormats, null, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime dateTime)) {
                timeStamp = new TimeStamp(dateTime);
                return true;
            } else {
                timeStamp = null;
                return false;
            }
        }

        public static bool TryParseDateTime(this string dateString, string[] formats, [NotNullWhen(true)] out TimeStamp? timeStamp) {
            if (DateTime.TryParseExact(dateString, formats, null, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime dateTime)) {
                timeStamp = new TimeStamp(dateTime);
                return true;
            } else {
                timeStamp = null;
                return false;
            }
        }
    }
}
