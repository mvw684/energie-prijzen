// Copyright (c) 2025 mvw684

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace EnergiePrijzen {
    internal class ExceptionReporting : IDisposable {

        public ExceptionReporting() {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.ThreadException += OnThreadException;
        }

        public void Dispose() {
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            Application.ThreadException -= OnThreadException;
        }

        private void OnThreadException(object sender, ThreadExceptionEventArgs e) {
            var exception = e.Exception;
            if (Debugger.IsAttached) {
                Debugger.Break();
            }
            if (exception is not null) {
                ReportExceptionAndCause(exception);
            } else {
                Tracer.Trace("Unknown exception");
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
                Tracer.Trace("Unknown exception");
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
            Tracer.Trace(cause + " " + exception.GetType().Name + ": " + exception.Message);
            var stack = exception.StackTrace;
            if (stack is null) {
                stack = "<no stack>";
            }
            Tracer.Trace(stack);
        }
    }
}
