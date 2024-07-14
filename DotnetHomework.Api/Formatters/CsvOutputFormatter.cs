using System.Collections;
using System.Reflection;
using System.Text;
using DotnetHomework.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace DotnetHomework.Api.Formatters
{
    /// <summary>
    /// Content negotiation CSV output formatter
    /// </summary>
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add("text/csv");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var sb = new StringBuilder();
            var response = context.HttpContext.Response;
            var data = context.Object;

            if (data == null || (typeof(DocumentDTO) != data.GetType()))
            {
                sb.AppendLine("Not supported object type for CSV output. Use application/JSON or text/JSON content type instead.");
            }

            else
            {
                string csv = ObjectToCsv(data as DocumentDTO);
                sb.Append(csv);
            }
            await response.WriteAsync(sb.ToString());
        }

        /// <summary>
        /// Converts an object to a CSV string.
        /// </summary>
        /// <typeparam name="T">The type of the object to convert.</typeparam>
        /// <param name="obj">The object to convert to CSV.</param>
        /// <returns>A CSV string representation of the object.</returns>
        string ObjectToCsv<T>(T obj)
        {
            StringBuilder csvStringBuilder = new StringBuilder();
            List<string> headers = new List<string>();
            List<string> values = new List<string>();

            BuildCsv(obj, headers, values, "");

            csvStringBuilder.AppendLine(string.Join(",", headers));
            csvStringBuilder.AppendLine(string.Join(",", values));

            return csvStringBuilder.ToString();
        }

        /// <summary>
        /// Recursively builds the CSV headers and values for the given object.
        /// </summary>
        /// <param name="obj">The object to process.</param>
        /// <param name="headers">The list of headers to build.</param>
        /// <param name="values">The list of values to build.</param>
        /// <param name="parentName">The name of the parent property.</param>
        void BuildCsv(object obj, List<string> headers, List<string> values, string parentName)
        {
            if (obj == null)
                return;

            Type type = obj.GetType();

            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Skip properties with parameters (e.g., indexers)
                if (property.GetIndexParameters().Length > 0)
                    continue;

                string propertyName = string.IsNullOrEmpty(parentName) ? property.Name : $"{parentName}.{property.Name}";

                if (IsSimple(property.PropertyType))
                {
                    headers.Add(propertyName);
                    values.Add(property.GetValue(obj)?.ToString() ?? string.Empty);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                {
                    var enumerable = property.GetValue(obj) as IEnumerable;
                    int index = 0;
                    foreach (var item in enumerable)
                    {
                        string itemPropertyName = $"{propertyName}[{index}]";
                        headers.Add(itemPropertyName);
                        values.Add(item?.ToString() ?? string.Empty);
                        index++;
                    }
                }
                else
                {
                    var propertyValue = property.GetValue(obj);
                    if (propertyValue != null)
                    {
                        BuildCsv(propertyValue, headers, values, propertyName);
                    }
                    else
                    {
                        AddEmptyProperties(property.PropertyType, headers, values, propertyName);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified type is a simple type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified type is a simple type; otherwise, <c>false</c>.</returns>
        bool IsSimple(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);
        }


        /// <summary>
        /// Adds empty properties for the given type to the headers and values lists.
        /// </summary>
        /// <param name="type">The type of the properties to add.</param>
        /// <param name="headers">The list of headers to add to.</param>
        /// <param name="values">The list of values to add to.</param>
        /// <param name="parentName">The name of the parent property.</param>
        void AddEmptyProperties(Type type, List<string> headers, List<string> values, string parentName)
        {
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.GetIndexParameters().Length > 0)
                    continue;

                string propertyName = string.IsNullOrEmpty(parentName) ? property.Name : $"{parentName}.{property.Name}";
                headers.Add(propertyName);
                values.Add(string.Empty);
            }
        }
    }
}