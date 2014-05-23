using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SelectBuilder
{
    public class Builder
    {
        private readonly IDataSource _dataSource;

        public Builder(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public void LoadDefinitions()
        {
            using (DataSet dataSet = _dataSource.GetDefinitions())
            {
                SchemaDefs = new List<SchemaDef>();
                ObjectDefs = new List<ObjectDef>();
                ColumnDefs = new List<ColumnDef>();

                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    SchemaDef schemaDef = new SchemaDef(
                        dataRow.Field<int>("SchemaId"),
                        dataRow.Field<string>("Name"));

                    SchemaDefs.Add(schemaDef);
                }

                foreach (DataRow dataRow in dataSet.Tables[1].Rows)
                {
                    ObjectDef objectDef = new ObjectDef(
                        dataRow.Field<int>("ObjectId"),
                        SchemaDefs.Single(item => item.SchemaId == dataRow.Field<int>("SchemaId")),
                        dataRow.Field<string>("Name"));

                    ObjectDefs.Add(objectDef);
                }

                foreach (DataRow dataRow in dataSet.Tables[2].Rows)
                {
                    ObjectDef objectDef = ObjectDefs.Single(item => item.TableId == dataRow.Field<int>("ObjectId"));
                    ObjectDef referencedObjectDef = dataRow.Field<int?>("ReferencedObjectId") == null ? null : ObjectDefs.Single(item => item.TableId == dataRow.Field<int?>("ReferencedObjectId"));

                    ColumnDef columnDef = new ColumnDef(
                        objectDef,
                        dataRow.Field<string>("Name"),
                        dataRow.Field<string>("Type"),
                        dataRow.Field<short>("Length"),
                        dataRow.Field<bool>("IsNullable"),
                        dataRow.Field<bool>("IsPrimaryKey"),
                        referencedObjectDef);

                    ColumnDefs.Add(columnDef);

                    if (dataRow.Field<bool>("IsPrimaryKey"))
                    {
                        objectDef.PrimayKeyColumn = columnDef;
                    }
                }
            }
        }

        public List<ColumnDef> ColumnDefs { get; set; }

        public List<ObjectDef> ObjectDefs { get; set; }

        public List<SchemaDef> SchemaDefs { get; set; }

        public SelectStatement CreateSelect(string table, string alias, int pageSize = 20, bool distinct = false, bool isPaged = false)
        {
            return new SelectStatement(this, table, alias, pageSize, distinct, isPaged);
        }
    }
}
