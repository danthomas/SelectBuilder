namespace SelectBuilder
{
    public class Join
    {
        public Join ParentJoin { get; set; }
        public ColumnDef ParentColumnDef { get; set; }
        public ColumnDef ColumnDef { get; set; }
        public ObjectDef ObjectDef { get; set; }
        public string Alias { get; set; }
        public SelectStatement SelectStatement { get; set; }

        public Join(ObjectDef objectDef, string alias)
        {
            ObjectDef = objectDef;
            Alias = alias;
        }

        public Join(Join parentJoin, ColumnDef parentColumnDef, ColumnDef columnDef, string alias)
            : this(parentJoin, parentColumnDef, columnDef, alias, null)
        {
        }

        public Join(Join parentJoin, ColumnDef parentColumnDef, ColumnDef columnDef, string alias, SelectStatement selectStatement)
        {
            ParentJoin = parentJoin;
            ParentColumnDef = parentColumnDef;
            ColumnDef = columnDef;
            ObjectDef = columnDef.ObjectDef;
            Alias = alias;
            SelectStatement = selectStatement;
        }
    }
}