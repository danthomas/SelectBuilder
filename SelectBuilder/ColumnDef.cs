using System;

namespace SelectBuilder
{
    public class ColumnDef
    {
        public ObjectDef ObjectDef { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public short Length { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public ObjectDef ReferencedObject { get; set; }

        public string FullName { get { return String.Format("{0}.{1}.{2}", ObjectDef.Schema.Name, ObjectDef.Name, Name); } }

        public ColumnDef(ObjectDef objectDef, string name, string type, short length, bool isNullable, bool isPrimaryKey, ObjectDef referencedObject)
        {
            ObjectDef = objectDef;
            Name = name;
            Type = type;
            Length = length;
            IsNullable = isNullable;
            IsPrimaryKey = isPrimaryKey;
            ReferencedObject = referencedObject;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}