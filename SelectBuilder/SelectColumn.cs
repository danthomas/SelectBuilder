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
        public int SortOrder { get; internal set; }
        public Aggregates Aggregate { get; set; }
        public ColumnDef AggregateColumnDef { get; set; }
        public string IsNull { get; set; }
        public string Statement { get; set; }

        public SelectColumn(Join @join, ColumnDef columnDef, string alias, ColumnType columnType, string[] dependentOnAliases, bool isVisible, int sortOrder, Aggregates aggregate, ColumnDef aggregateColumnDef, string isNull, string statement = null)
        {
            Join = @join;
            ColumnDef = columnDef;
            ColumnType = columnType;
            Alias = alias;
            DependentOnAliases = dependentOnAliases;
            IsVisible = isVisible;
            SortOrder = sortOrder;
            Aggregate = aggregate;
            AggregateColumnDef = aggregateColumnDef;
            IsNull = isNull;
            Statement = statement;
        }
    }
}