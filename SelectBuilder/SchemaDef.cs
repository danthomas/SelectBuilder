namespace SelectBuilder
{
    public class SchemaDef
    {
        public int SchemaId { get; set; }
        public string Name { get; set; }

        public string FullName { get { return Name.MakeSafe(); } }

        public SchemaDef(int schemaId, string name)
        {
            SchemaId = schemaId;
            Name = name;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}