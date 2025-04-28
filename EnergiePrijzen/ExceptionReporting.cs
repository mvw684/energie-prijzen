// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace EnergiePrijzen {
    internal class ExceptionReporting : IDisposable {

        public class ExceptionMessageEventArgs : EventArgs {
            required public string Message {
                get; init;
            }

            required public Exception Exception {
                get; init;
            }
        }

        public delegate void ExceptionReportEvent(object sender, ExceptionMessageEventArgs args);

        public static event ExceptionReportEvent? ExceptionReporters;

        public ExceptionReporting() {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.ThreadException += OnThreadException;
            ExceptionReporters += TraceException;

        }

        public void Dispose() {
            ExceptionReporters -= TraceException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.ThreadException += OnThreadException;
        }

        private void OnThreadException(object sender, ThreadExceptionEventArgs e) {
            var exception = e.Exception;
            if (Debugger.IsAttached) {
                Debugger.Break();
            }
            if (exception is not null) {
                ReportExceptionAndCause(exception);
            } else {
                Trace.WriteLine("Unknown exception");
            }
        }

        private void OnUnhandledException(
            object sender,
            UnhandledExceptionEventArgs e
        ) {
            var exception = e.ExceptionObject as Exception;
            if (Debugger.IsAttached) {
                Debugger.Break();
            }
            if (exception is not null) {
                ReportExceptionAndCause(exception);
            } else {
                Trace.WriteLine("Unknown exception");
            }
        }

        private void ReportExceptionAndCause(Exception exception) {
            ReportException("Caught", exception);
            var cause = exception.GetBaseException();
            if ((cause is not null) && (cause != exception)) {
                ReportException("Caused by", cause);
            }
        }

        private void ReportException(string cause, Exception exception) {
            ExceptionReporters?.Invoke(
                this, 
                new ExceptionMessageEventArgs {
                    Message = cause,
                    Exception = exception
                }
            );
        }

        private void TraceException(object sender, ExceptionMessageEventArgs args) {
            var exception = args.Exception;
            var cause = args.Message;
            Trace.WriteLine(cause + " " + exception.GetType().Name + ": " + exception.Message);
            Trace.WriteLine(exception.StackTrace);
        }
    }
}
