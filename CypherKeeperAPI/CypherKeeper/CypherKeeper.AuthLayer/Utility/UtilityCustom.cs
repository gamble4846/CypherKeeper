using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using FastMember;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace CypherKeeper.AuthLayer.Utility
{
    public static class UtilityCustom
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static T ConvertReaderToObject<T>(this SqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();
            try
            {
                for (int i = 0; i < rd.FieldCount; i++)
                {
                    if (!rd.IsDBNull(i))
                    {
                        string fieldName = rd.GetName(i);

                        if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                        {
                            accessor[t, fieldName] = rd.GetValue(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex;
            }


            return t;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                var type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static dynamic GetFullQueryRow(SqlDataReader rd)
        {
            dynamic RowData = new System.Dynamic.ExpandoObject();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                var currentValue = rd.GetValue(i);
                var currentColumn = rd.GetName(i);
                var currentType = rd.GetFieldType(i);

                if (string.IsNullOrEmpty(currentValue.ToString()))
                {
                    ((IDictionary<string, object>)RowData).Add(currentColumn, null);
                }
                else
                {
                    ((IDictionary<string, object>)RowData).Add(currentColumn, currentValue);
                }
            }

            return RowData;
        }

        public static async Task<dynamic> RestCall(string ApiLink)
        {
            dynamic data = await _httpClient.GetStringAsync(ApiLink);

            JObject json = JObject.Parse(data);
            var emptyArrays = json.Properties()
                .Where(p => p.Value.Type == JTokenType.Array && !p.Value.HasValues)
                .ToList();

            foreach (var property in emptyArrays)
            {
                property.Remove();
            }

            var CleanedJSON = json.ToString();

            dynamic ToReturn = JsonConvert.DeserializeObject<dynamic>(CleanedJSON);
            return ToReturn;
        }

        public static DataTable GetDataTableFromCommand(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            catch (Exception)
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                throw;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

            return dt;
        }

        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);

                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }

            return item;
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
                 .FirstOrDefault();
        }

        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }

        public static T DataTableToObject<T>(this DataTable dt) where T : new()
        {
            var dataRows = dt.AsEnumerable().ToList();
            if (dataRows.Count > 0)
            {
                return dataRows[0].ToObject<T>();
            }
            else
            {
                return default;
            }
        }

        public static List<T> DataTableToObjectList<T>(this DataTable dt) where T : new()
        {
            var dataRows = dt.AsEnumerable().ToList();
            var ListObject = new List<T>();
            foreach (var dataRow in dataRows)
            {
                ListObject.Add(dataRow.ToObject<T>());
            }
            return ListObject;
        }
    }
}

