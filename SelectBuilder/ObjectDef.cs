using System;

namespace SelectBuilder
{
    public class ObjectDef
    {
        public int TableId { get; set; }
        public SchemaDef Schema { get; set; }
        public string Name { get; set; }
        public ColumnDef PrimayKeyColumn { get; set; }

        public string FullName { get { return String.Format("{0}.{1}", Schema.Name.MakeSafe(), Name.MakeSafe()); } }

        public ObjectDef(int tableId, SchemaDef schema, string name)
        {
            TableId = tableId;
            Schema = schema;
            Name = name;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}