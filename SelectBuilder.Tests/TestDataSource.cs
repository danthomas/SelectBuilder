using System;
using System.Data;

namespace SelectBuilder.Tests
{
    public class TestDataSource : IDataSource
    {
        private readonly DataSet _dataSet;
        private readonly DataTable _schemaTable;
        private readonly DataTable _objectTable;
        private readonly DataTable _columnTable;

        private int _schemaId;
        private int _objectId;

        public TestDataSource()
        {
            _dataSet = new DataSet();

            _schemaTable = new DataTable("Schemas");
            _dataSet.Tables.Add(_schemaTable);

            _schemaTable.Columns.Add("SchemaId", typeof(int));
            _schemaTable.Columns.Add("Name", typeof(string));

            _objectTable = new DataTable("Objects");
            _dataSet.Tables.Add(_objectTable);

            _objectTable.Columns.Add("ObjectId", typeof(int));
            _objectTable.Columns.Add("SchemaId", typeof(int));
            _objectTable.Columns.Add("Name", typeof(string));

            _columnTable = new DataTable("Columns");
            _dataSet.Tables.Add(_columnTable);

            _columnTable.Columns.Add("ObjectId", typeof(int));
            _columnTable.Columns.Add("Name", typeof(string));
            _columnTable.Columns.Add("Type", typeof(string));
            _columnTable.Columns.Add("Length", typeof(short));
            _columnTable.Columns.Add("IsNullable", typeof(bool));
            _columnTable.Columns.Add("IsPrimaryKey", typeof(bool));
            _columnTable.Columns.Add("ReferencedObjectId", typeof(int));
        }

        public DataSet GetDefinitions()
        {
            return _dataSet;
        }

        public void AddSchema(string name)
        {
            _schemaTable.Rows.Add(++_schemaId, name);
        }

        public void AddObject(string schema, string name)
        {
            int schemaId = (int)_schemaTable.Select(String.Format("name = '{0}'", schema))[0][0];
            _objectTable.Rows.Add(++_objectId, schemaId, name);
        }

        public void AddColumn(string schema, string @object, string name, string type, int length, bool isNullable, bool isPrimaryKey/*, string referencedSchema*/, string referencedObject)
        {
            int objectId = (int)_objectTable.Select(String.Format("name = '{0}'", @object))[0][0];

            int? referencedObjectId = null;

            if (referencedObject != null)
            {
                referencedObjectId = (int)_objectTable.Select(String.Format("name = '{0}'", referencedObject))[0][0];
            }
            _columnTable.Rows.Add(objectId, name, type, length, isNullable, isPrimaryKey, referencedObjectId);
        }
    }
}