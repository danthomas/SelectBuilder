using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SelectBuilder
{
    public class DataSource : IDataSource
    {
        public string ConnectionStringName { get; set; }

        public DataSource(string connectionStringName)
        {
            ConnectionStringName = connectionStringName;
        }

        public DataSet GetDefinitions()
        {
            DataSet dataSet = new DataSet();

            const string sql = @"
select		s.schema_id SchemaId
			, s.name Name
from		sys.schemas s
where		s.principal_id = 1

select		t.object_id ObjectId
			, t.schema_id SchemaId
			, t.name Name
from		sys.tables t
join		sys.schemas s on t.schema_id = s.schema_id 
where		s.principal_id = 1

select		c.object_id ObjectId
			, c.name Name
			, t2.name Type
			, c.max_length Length
			, c.is_nullable IsNullable
			, cast(case when pk.name is null then 0 else 1 end as bit) IsPrimaryKey
			, fk.referenced_object_id ReferencedObjectId
from		sys.columns c
join		sys.tables t1 on c.object_id = t1.object_id	
join		sys.types t2 on c.user_type_id = t2.user_type_id
left join	(
			select	c.object_id
					, c.name
			from	sys.key_constraints kc
			join    sys.index_columns ic ON kc.parent_object_id = ic.object_id  and kc.unique_index_id = ic.index_id
			join    sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
			where	kc.type = 'PK' ) pk on c.object_id = pk.object_id and c.name = pk.name
left join	(
			select	parent_object_id object_id,
					c.name,
					referenced_object_id
					, cref.name referenced_object_name
			from	sys.foreign_key_columns fkc
			join	sys.columns c on fkc.parent_column_id = c.column_id and fkc.parent_object_id = c.object_id
			join	sys.columns cref on fkc.referenced_column_id = cref.column_id and fkc.referenced_object_id = cref.object_id) fk on c.object_id = fk.object_id and c.name = fk.name";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString))
            {
                connection.Open();

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection))
                {
                    dataAdapter.Fill(dataSet);
                }
            }

            return dataSet;
        }
    }
}
