using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace DechFlow.Renders
{
    public class ConsoleTable
    {
        private readonly IList<object> _columns;
        private readonly IList<object[]> _rows;

        private Type[] ColumnTypes { get; set; }

        private static HashSet<Type> _numericTypes = new HashSet<Type>
        {
            typeof(int), typeof(double), typeof(decimal),
            typeof(long), typeof(short), typeof(sbyte),
            typeof(byte), typeof(ulong), typeof(ushort),
            typeof(uint), typeof(float)
        };

        private ConsoleTable()
        {
            _rows = new List<object[]>();
            _columns = new List<object>();
        }

        private void AddColumn(IEnumerable<string> names)
        {
            foreach (var name in names)
                _columns.Add(name);
        }

        private void AddRow(params object[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (!_columns.Any())
                throw new Exception("Please set the columns first");

            if (_columns.Count != values.Length)
                throw new Exception(
                    $"The number columns in the row ({_columns.Count}) does not match the values ({values.Length})");

            _rows.Add(values);
        }
        
        public static ConsoleTable FromDynamic(IEnumerable<ExpandoObject> values)
        {
            var sampleData = values.First();
            
            var table = new ConsoleTable
            {
                ColumnTypes = GetColumnsType(sampleData).ToArray()
            };

            var columns = GetColumns(sampleData);

            table.AddColumn(columns);

            var valuesToAdd = values
                .Select(value => columns
                    .Select(column => GetColumnValue(value, column)));
            
            foreach (var propertyValues in valuesToAdd)
            {
                table.AddRow(propertyValues.ToArray());
            }

            return table;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var columnLengths = ColumnLengths();

            var columnAlignment = Enumerable.Range(0, _columns.Count)
                .Select(GetNumberAlignment)
                .ToList();

            var format = Enumerable.Range(0, _columns.Count)
                .Select(i => " | {" + i + "," + columnAlignment[i] + columnLengths[i] + "}")
                .Aggregate((s, a) => s + a) + " |";

            var maxRowLength = Math.Max(0, _rows.Any() ? _rows.Max(row => string.Format(format, row).Length) : 0);
            var columnHeaders = string.Format(format, _columns.ToArray());
            
            var longestLine = Math.Max(maxRowLength, columnHeaders.Length);
            var results = _rows.Select(row => string.Format(format, row)).ToList();

            var divider = " " + string.Join("", Enumerable.Repeat("-", longestLine - 1)) + " ";

            builder.AppendLine(divider);
            builder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                builder.AppendLine(divider);
                builder.AppendLine(row);
            }

            builder.AppendLine(divider);
            
            return builder.ToString();
        }
        
        private string GetNumberAlignment(int i)
        {
            if (ColumnTypes != null && _numericTypes.Contains(ColumnTypes[i]))
            {
                return "";
            }

            return "-";
        }

        private List<int> ColumnLengths()
        {
            var columnLengths = _columns
                .Select((t, i) => _rows.Select(x => x[i])
                    .Union(new[] {_columns[i]})
                    .Where(x => x != null)
                    .Select(x => x.ToString().Length).Max())
                .ToList();
            return columnLengths;
        }
        
        private static IEnumerable<string> GetColumns(IDictionary<string,object> expandoObjectAsDict)
        {
            return expandoObjectAsDict.Keys.ToArray();
        }

        private static object GetColumnValue(IDictionary<string,object> expandoObjectAsDict, string column)
        {
            return  expandoObjectAsDict[column];
        }

        private static IEnumerable<Type> GetColumnsType(IDictionary<string,object> expandoObjectAsDict)
        {
            return expandoObjectAsDict.Keys.Select(s=>expandoObjectAsDict[s].GetType()).ToArray();
        }
    }
}