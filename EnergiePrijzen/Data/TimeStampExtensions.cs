// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics.CodeAnalysis;

namespace EnergiePrijzen.Data {
    public static class TimeStampExtensions {

        internal const string DateFormat = "yyyy-MM-dd";

        private readonly static string[] dateFormats = [TimeStampExtensions.DateFormat];


        public static bool TryParseDate(this string dateString, [NotNullWhen(true)] out TimeStamp? timeStamp) {
            if (DateTime.TryParseExact(dateString, dateFormats, null, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime dateTime)) {
                timeStamp = new TimeStamp(dateTime);
                return true;
            } else {
                timeStamp = null;
                return false;
            }
        }

        public static bool TryParseDateTime(this string dateString, string[] formats, [NotNullWhen(true)] out DateTime? dateTime) {
            if (DateTime.TryParseExact(dateString, formats, null, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime temp)) {
                dateTime = temp;
                return true;
            } else {
                dateTime = null;
                return false;
            }
        }

        public static bool TryParseDateTime(this string dateString, string[] formats, [NotNullWhen(true)] out TimeStamp? timeStamp) {
            if (TryParseDateTime(dateString, formats, out DateTime? dateTime)) {
                timeStamp = new TimeStamp(dateTime.Value);
                return true;
            } else {
                timeStamp = null;
                return false;
            }
        }

        internal static bool Aggregate<TData>(this TimeStampedDataList<TData> aggragetables) where TData : class, IAggregatableData<TData> {
            try {
                foreach (TData item in aggragetables) {
                    item.Aggregate();
                }
            } catch (Exception e) {
                Tracer.Trace("Failed to aggregate " + aggragetables.GetType().Name + ": " + e.Message);
                return false;
            }
            return true;
        }
    }
}
