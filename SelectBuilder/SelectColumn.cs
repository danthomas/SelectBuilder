using System.Collections;

namespace SelectBuilder
{
    public class SelectColumn
    {
        public Join Join { get; set; }
        public ColumnDef ColumnDef { get; set; }
        public ColumnType ColumnType { get; set; }
        public string Alias { get; set; }
        public string[] DependentOnAliases { get; set; }
        public bool IsVisible { get; set; }
        public int OrderByIndex { get; set; }
        public Aggregates Aggregate { get; set; }
        public ColumnDef AggregateColumnDef { get; set; }
        public string IsNull { get; set; }
        public string Statement { get; set; }
        public SelectStatement OptionsSelectStatement { get; set; }
        public bool IsWhere { get { return (ColumnType & ColumnType.Where) == ColumnType.Where; }}
        public bool IsSelect { get { return (ColumnType & ColumnType.Select) == ColumnType.Select; } }
        public bool IsOrderBy { get { return (ColumnType & ColumnType.OrderBy) == ColumnType.OrderBy; } }

        public SelectColumn(Join @join, ColumnDef columnDef, string alias, ColumnType columnType, string[] dependentOnAliases, bool isVisible, int orderByIndex, Aggregates aggregate, ColumnDef aggregateColumnDef, string isNull, string statement = null, SelectStatement optionsSelectStatement = null)
        {
            Join = @join;
            ColumnDef = columnDef;
            ColumnType = columnType;
            Alias = alias;
            DependentOnAliases = dependentOnAliases;
            IsVisible = isVisible;
            OrderByIndex = orderByIndex;
            Aggregate = aggregate;
            AggregateColumnDef = aggregateColumnDef;
            IsNull = isNull;
            Statement = statement;
            OptionsSelectStatement = optionsSelectStatement;
        }

        public override string ToString()
        {
            return Alias;
        }
    }
}