namespace SelectBuilder
{
    public class WhereColumn
    {
        public Join Join { get; set; }
        public ColumnDef ColumnDef { get; set; }
        public string Statement { get; set; }
        public Operators Operator { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }

        public WhereColumn(Join @join, ColumnDef columnDef, string statement, Operators @operator, string value1, string value2)
        {
            Join = @join;
            ColumnDef = columnDef;
            Statement = statement;
            Operator = @operator;
            Value1 = value1;
            Value2 = value2;
        }
    }
}