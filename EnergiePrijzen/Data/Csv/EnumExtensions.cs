// Copyright (c) 2025 mvw684

using System.ComponentModel;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using Excel = ClosedXML.Excel;

namespace EnergiePrijzen.Data.Csv {
    internal static class EnumExtensions {

        /// <summary>
        /// This method is to fetch the string from the
        /// [Description("")] attribute of the ENUM value or if not present returns the enum values name.
        /// </summary>
        /// <param name="value">ENUM value for which description needs to be fetched.</param>
        /// <returns>
        /// string represented in the Description attribute or null
        /// if no description is provided.
        /// </returns>
        public static string GetHeaderValue<T>(this T value) where T : Enum {
            string name = value.ToString();
            FieldInfo? fieldInfo = value.GetType().GetField(name);

            var attributes =
                (DescriptionAttribute[])
                fieldInfo!.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 1 ? attributes[0].Description : name;
        }

        public static int ToInt<T>(this T value) where T : Enum {
            return Unsafe.As<T, int>(ref value);
        }

        public static Excel.IXLCell Cell<T>(this Excel.IXLRow row, T index) where T : Enum {
            return row.Cell(index.ToInt());

        }
    }
}
