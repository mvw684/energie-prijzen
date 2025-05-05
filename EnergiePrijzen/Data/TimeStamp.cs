// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics.CodeAnalysis;

namespace EnergiePrijzen.Data {
    public readonly struct TimeStamp : IComparable<TimeStamp>, IEquatable<TimeStamp> {
        private readonly DateTime start;

        private static readonly TimeSpan duration = TimeSpan.FromMinutes(60);
        private static readonly DateTime zero = new DateTime(2020, 1, 1);
        
        public TimeStamp(DateTime start) {
            var asStamp = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0, 0, start.Kind);
            if (asStamp.Kind == DateTimeKind.Local) {
                this.start = asStamp;
            } else if (asStamp.Kind == DateTimeKind.Utc) {
                this.start = asStamp.ToLocalTime();
            } else {
                throw new ArgumentException(asStamp.Kind + "unexpected", nameof(start));
            }
        }

        public readonly DateTime Start {
            get => start;
        }

        public DateTime End => start + duration;

        public static TimeSpan Duration {
            get => duration;
        }

        public override string ToString() => start.ToString("yyyy-MM-dd HH") + " / " + start.ToLocalTime().ToString("yyyy-MM-dd HH");

        public override int GetHashCode() => (int)((start - zero).TotalMinutes);

        public override bool Equals([NotNullWhen(true)] object? obj) => (obj is not null) && (obj is TimeStamp stamp) && (stamp.Start == start);

        public int CompareTo(TimeStamp other) => start.CompareTo(other.start);

        public bool Equals(TimeStamp other) => start.Equals(other.start);

        public static bool operator ==(TimeStamp left, TimeStamp right) => left.Equals(right);

        public static bool operator !=(TimeStamp left, TimeStamp right) => !left.Equals(right);

        public static bool operator <(TimeStamp left, TimeStamp right) => left.CompareTo(right) < 0;

        public static bool operator >(TimeStamp left, TimeStamp right) => left.CompareTo(right) > 0;

        public static bool operator <=(TimeStamp left, TimeStamp right) => left.CompareTo(right) <= 0;
        public static bool operator >=(TimeStamp left, TimeStamp right) => left.CompareTo(right) >= 0;
        public static TimeStamp operator +(TimeStamp left, TimeSpan right) => new TimeStamp(left.start + right);
    }
}
