using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexMarkup.Classes;
using System.Reflection;
using System.Data;
using System.ComponentModel;

namespace RegexMarkup.Classes
{
    public static class ListConverter
    {

        /// <summary>
        /// Convert our IList to a DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            Type elementType = typeof(T);
            using (DataTable t = new DataTable())
            {
                PropertyInfo[] _props = elementType.GetProperties();
                foreach (PropertyInfo propInfo in _props)
                {
                    Type _pi = propInfo.PropertyType;
                    Type ColType = Nullable.GetUnderlyingType(_pi) ?? _pi;
                    t.Columns.Add(getColName(propInfo), ColType);
                }
                foreach (T item in list)
                {
                    DataRow row = t.NewRow();
                    foreach (PropertyInfo propInfo in _props)
                    {
                        row[getColName(propInfo)] = propInfo.GetValue(item, null) ?? DBNull.Value;
                    }
                    t.Rows.Add(row);
                }
                return t;
            }
        }

        /// <summary>
        /// Convert our IList to a DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>DataSet</returns>
        public static DataSet ToDataSet<T>(this IEnumerable<T> list)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Tables.Add(list.ToDataTable());
                return ds;
            }
        }

        private static string getColName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(DisplayNameAttribute), true);
            if (atts.Length == 0)
                return property.Name;
            return (atts[0] as DisplayNameAttribute).DisplayName;
        }
    }
}
