// Copyright (c) 2025 mvw684

using System;

namespace EnergiePrijzen {
    public static class Tracer {


        public class TraceMessageEventArgs : EventArgs {
            required public string Message {
                get; init;
            }
        }
        public delegate void TraceEvent(object sender, TraceMessageEventArgs args);
        public static event TraceEvent? Tracers;

        static Tracer() {
            Tracers += (sender, args) => {
                System.Diagnostics.Trace.WriteLine(args.Message);
            };
        }


        public static void Trace(string message) {
            Tracers?.Invoke(AppDomain.CurrentDomain, new TraceMessageEventArgs() { Message = message });
        }
    }
}
