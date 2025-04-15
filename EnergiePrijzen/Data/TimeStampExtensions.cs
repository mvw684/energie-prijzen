// Copyright (c) 2025 mvw684

using System;

namespace EnergiePrijzen.Data {
    public static class TimeStampExtensions {

        public static DateTime ToStamp(this DateTime dateTime) {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, 0, DateTimeKind.Local);
        }
    }
}
