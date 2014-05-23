using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SelectBuilder
{
    public class SelectStatement
    {
        private readonly Builder _builder;

        internal SelectStatement(Builder builder, string @object, string alias, int pageSize, bool distinct, bool isPaged)
        {
            PageSize = pageSize;
            Distinct = distinct;
            IsPaged = isPaged;
            PageNo = 1;
            _builder = builder;

            ObjectDef objectDef = builder.ObjectDefs.Single(item => item.FullName == @object);

            Join join = new Join(objectDef, alias);

            Joins = new List<Join>(new[] { @join });
            SelectColumns = new List<SelectColumn>();
            WhereColumns = new List<WhereColumn>();
        }

        public List<Join> Joins { get; set; }

        public List<SelectColumn> SelectColumns { get; set; }

        public List<WhereColumn> WhereColumns { get; set; }

        public int PageSize { get; set; }

        public bool Distinct { get; set; }
        
        public bool IsPaged { get; set; }

        public int PageNo { get; set; }

        public SelectStatement Join(string foreignKeyColumn, string alias)
        {
            /*
             * usage: .CreateSelect("core.Product", "p").Join("p.ProductTypeId", "pt")
             * 
             * joins the foreign key p.ProductTypeId to the referenced object on its primary key
             */
            GetJoinAndColumnDef(foreignKeyColumn, (parentJoin, parentColumnDef) =>
            {
                ObjectDef objectDef = parentColumnDef.ReferencedObject;
                ColumnDef columnDef = objectDef.PrimayKeyColumn;

                Join join = new Join(parentJoin, parentColumnDef, columnDef, alias);

                Joins.Add(join);
            });

            return this;
        }

        public SelectStatement Join(string parentColumn, SelectStatement selectStatement, string alias, string column)
        {
            GetJoinAndColumnDef(parentColumn, (parentJoin, parentColumnDef) =>
                GetSelectColumn(column, selectStatement, selectColumn =>
                {
                    Join join = new Join(parentJoin, parentColumnDef, selectColumn.ColumnDef, alias, selectStatement);
                    Joins.Add(@join);
                }));

            return this;
        }

        private void GetSelectColumn(string column, SelectStatement selectStatement, Action<SelectColumn> action)
        {
            string alias;
            string name;

            selectStatement.IsPaged = false;

            if (column.Contains("."))
            {
                alias = column.Split('.')[0];
                name = column.Split('.')[1];
            }
            else
            {
                alias = null;
                name = column;
            }

            action(selectStatement.SelectColumns.Single(item => item.Alias == (alias ?? item.Alias) && item.ColumnDef.Name == name));
        }

        public SelectStatement Join(string parentAlias, string primaryObject, string alias)
        {
            /*
             * usage: .CreateSelect("core.ProductType", "pt").Join("pt", "core.Product", "p")
             * 
             * joins the primary key of pt.ProductTypeId to the foreigh key of core.Product that references core.ProductType
             */
            Join parentJoin = Joins.Single(item => item.Alias == parentAlias);

            ObjectDef objectDef = _builder.ObjectDefs.Single(item => item.FullName == primaryObject);

            ColumnDef columnDef = _builder.ColumnDefs.Single(item => item.ObjectDef == objectDef && item.ReferencedObject == parentJoin.ObjectDef);

            Join join = new Join(parentJoin, parentJoin.ObjectDef.PrimayKeyColumn, columnDef, alias);

            Joins.Add(join);

            return this;
        }

        public SelectStatement Select(string column, string alias = null, ColumnType columnType = ColumnType.All, bool isVisible = true, int sortOrder = 0, Aggregates aggregate = Aggregates.None, string isNull = null)
        {
            GetJoinAndColumnDef(column, (join, columnDef) =>
            {
                if (String.IsNullOrEmpty(alias))
                {
                    alias = columnDef.Name;
                }

                ColumnDef aggregateColumnDef = null;

                if (aggregate != Aggregates.None)
                {
                    string type = "";

                    if (aggregate == Aggregates.Count)
                    {
                        type = "int";
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    aggregateColumnDef = new ColumnDef(null, alias, type, 0, true, false, null);
                }

                SelectColumn selectColumn = new SelectColumn(@join, columnDef, alias, columnType, null, isVisible, sortOrder, aggregate, aggregateColumnDef, isNull);

                SelectColumns.Add(selectColumn);
            });

            return this;
        }

        public SelectStatement SelectCustom(string statement, string type, string alias, ColumnType columnType = ColumnType.All, string[] dependentOnAliases = null, bool isVisible = true, int sortOrder = 0)
        {
            ColumnDef columnDef = new ColumnDef(null, alias, type, 0, false, false, null);

            SelectColumn selectColumn = new SelectColumn(null, columnDef, alias, columnType, dependentOnAliases, isVisible, sortOrder, Aggregates.None, null, null, statement);

            SelectColumns.Add(selectColumn);

            return this;
        }

        private void GetJoinAndColumnDef(string column, Action<Join, ColumnDef> action)
        {
            string alias = null;
            string name;

            if (column.Contains("."))
            {
                alias = column.Split('.')[0];
                name = column.Split('.')[1];
            }
            else
            {
                name = column;

                var allColumnDefs = _builder.ColumnDefs
                     .Join(Joins, j => j.ObjectDef, cd => cd.ObjectDef, (cd, j) => new { Join = j, ColumnDef = cd })
                     .Where(item => item.ColumnDef.Name == name)
                     .ToList();

                if (allColumnDefs.Count == 1)
                {
                    alias = allColumnDefs[0].Join.Alias;
                }
                else if (allColumnDefs.Count(item => item.ColumnDef.IsPrimaryKey) == 1)
                {
                    alias = allColumnDefs.Single(item => item.ColumnDef.IsPrimaryKey).Join.Alias;
                }
            }

            Join join = Joins.Single(item => item.Alias == alias);
            ColumnDef columnDef = null;

            if (join.SelectStatement != null)
            {
                SelectColumn selectColumn = join.SelectStatement.SelectColumns.Single(item => item.Alias == name);

                if (selectColumn.AggregateColumnDef != null)
                {
                    columnDef = selectColumn.AggregateColumnDef;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                columnDef = _builder.ColumnDefs.Single(item => item.ObjectDef == join.ObjectDef && item.Name == name);
            }

            action(join, columnDef);
        }

        public SelectStatement Where(string column, Operators @operator, string value1, string value2)
        {
            GetJoinAndColumnDef(column, (join, columnDef) =>
            {
                WhereColumn whereColumn = new WhereColumn(@join, columnDef, null, @operator, value1, value2);

                WhereColumns.Add(whereColumn);
            });

            return this;
        }

        public SelectStatement Where(SelectColumn selectColumn, Operators @operator, string value1, string value2)
        {
            WhereColumn whereColumn = new WhereColumn(selectColumn.Join, selectColumn.ColumnDef, selectColumn.Statement, @operator, value1, value2);

            WhereColumns.Add(whereColumn);

            return this;
        }

        public string Statement
        {
            get
            {
                FixOrderBy();

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendLine();

                if (Distinct)
                {
                    stringBuilder.Append("SELECT DISTINCT");
                    stringBuilder.AppendLine();
                }
                else
                {
                    stringBuilder.Append("SELECT      ");
                }

                bool first = true;

                if (SelectColumns.Count(item => item.IsVisible) == 0)
                {
                    stringBuilder.Append("*");
                }
                else
                {
                    foreach (SelectColumn selectColumn in SelectColumns.Where(item => item.IsVisible))
                    {
                        if (!first)
                        {
                            stringBuilder.AppendLine();
                            stringBuilder.Append("          , ");
                        }

                        if (selectColumn.Statement != null)
                        {
                            stringBuilder.Append(selectColumn.Statement);
                            stringBuilder.Append(" ");
                            stringBuilder.Append(selectColumn.Alias);

                        }
                        else
                        {

                            if (selectColumn.IsNull != null)
                            {
                                stringBuilder.Append("ISNULL(");
                            }

                            if (selectColumn.Aggregate != Aggregates.None)
                            {
                                stringBuilder.Append(selectColumn.Aggregate.ToString().ToUpper());
                                stringBuilder.Append("(");
                            }

                            stringBuilder.Append(selectColumn.Join.Alias.MakeSafe());
                            stringBuilder.Append(".");

                            if (selectColumn.ColumnDef.ObjectDef == null)
                            {
                                stringBuilder.Append(selectColumn.Alias);
                            }
                            else
                            {
                                stringBuilder.Append(selectColumn.ColumnDef.Name.MakeSafe());
                            }

                            if (selectColumn.Aggregate != Aggregates.None)
                            {
                                stringBuilder.Append(")");
                            }

                            if (selectColumn.IsNull != null)
                            {
                                stringBuilder.Append(", ");
                                stringBuilder.Append(selectColumn.IsNull);
                                stringBuilder.Append(")");
                            }

                            if (selectColumn.IsNull != null ||
                                (!String.IsNullOrWhiteSpace(selectColumn.Alias) && selectColumn.Alias != selectColumn.ColumnDef.Name))
                            {
                                stringBuilder.Append(" ");
                                stringBuilder.Append(selectColumn.Alias.MakeSafe());
                            }
                        }

                        first = false;
                    }

                }

                if (IsPaged)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.Append("          , COUNT(*) OVER(PARTITION BY '') TotalRecords");
                }

                first = true;

                foreach (Join join in GetRequiredJoins())
                {
                    if (first)
                    {
                        stringBuilder.AppendLine();
                        stringBuilder.Append("FROM        ");

                        stringBuilder.Append(join.ObjectDef.FullName);
                        stringBuilder.Append(" ");
                        stringBuilder.Append(join.Alias.MakeSafe());

                        first = false;
                    }
                    else
                    {
                        stringBuilder.AppendLine();

                        if (join.SelectStatement != null)
                        {
                            //ToDo : don't always need left join
                            stringBuilder.Append("LEFT JOIN   (");
                            stringBuilder.Append("            ");
                            stringBuilder.Append(join.SelectStatement.Statement.Replace(Environment.NewLine, Environment.NewLine + "            "));
                            stringBuilder.AppendLine();
                            stringBuilder.Append("            )");
                        }
                        else
                        {
                            if (join.ParentColumnDef.IsNullable)
                            {
                                stringBuilder.Append("LEFT JOIN   ");
                                stringBuilder.Append(join.ObjectDef.FullName);
                            }
                            else
                            {
                                stringBuilder.Append("JOIN        ");
                                stringBuilder.Append(join.ObjectDef.FullName);
                            }
                        }

                        stringBuilder.Append(" ");
                        stringBuilder.Append(join.Alias.MakeSafe());
                        stringBuilder.Append(" ON ");
                        stringBuilder.Append(join.ParentJoin.Alias.MakeSafe());
                        stringBuilder.Append(".");
                        stringBuilder.Append(join.ParentColumnDef.Name.MakeSafe());
                        stringBuilder.Append(" = ");
                        stringBuilder.Append(join.Alias.MakeSafe());
                        stringBuilder.Append(".");
                        stringBuilder.Append(join.ColumnDef.Name.MakeSafe());
                    }
                }

                first = true;

                int parameterId = 1;

                foreach (WhereColumn whereColumn in WhereColumns)
                {
                    stringBuilder.AppendLine();

                    stringBuilder.Append(first ? "WHERE       " : "AND         ");

                    if (whereColumn.Statement != null)
                    {
                        stringBuilder.Append(whereColumn.Statement);
                    }
                    else
                    {
                        stringBuilder.Append(whereColumn.Join.Alias);
                        stringBuilder.Append(".");
                        stringBuilder.Append(whereColumn.ColumnDef.Name);
                        stringBuilder.Append(" ");
                    }

                    switch (whereColumn.Operator)
                    {
                        case Operators.IsEqualTo:
                            stringBuilder.Append("= @p");
                            stringBuilder.Append(parameterId);
                            break;
                        case Operators.Contains:
                            stringBuilder.Append("LIKE '%' + @p");
                            stringBuilder.Append(parameterId);
                            stringBuilder.Append(" + '%'");
                            break;
                        case Operators.StartsWith:
                            stringBuilder.Append("LIKE @p");
                            stringBuilder.Append(parameterId);
                            stringBuilder.Append(" + '%'");
                            break;
                        case Operators.EndsWidth:
                            stringBuilder.Append("LIKE '%' + @p");
                            stringBuilder.Append(parameterId);
                            break;
                        case Operators.IsLessThan:
                            stringBuilder.Append("< @p");
                            stringBuilder.Append(parameterId);
                            break;
                        case Operators.IsMoreThan:
                            stringBuilder.Append("> @p");
                            stringBuilder.Append(parameterId);
                            break;
                        case Operators.IsLessThanOrEqualTo:
                            stringBuilder.Append("<= @p");
                            stringBuilder.Append(parameterId);
                            break;
                        case Operators.IsMoreThanOrEqualTo:
                            stringBuilder.Append(">= @p");
                            stringBuilder.Append(parameterId);
                            break;
                        case Operators.IsTrue:
                            stringBuilder.Append("= 1");
                            break;
                        case Operators.IsFalse:
                            stringBuilder.Append("= 0");
                            break;
                        case Operators.IsNull:
                            stringBuilder.Append("IS NULL");
                            break;
                        case Operators.IsNotNull:
                            stringBuilder.Append("IS NOT NULL");
                            break;
                    }

                    first = false;
                    if (IsParamRequired(whereColumn))
                    {
                        parameterId++;
                    }
                }


                first = true;

                if (SelectColumns.Any(item => item.Aggregate != Aggregates.None))
                {
                    foreach (SelectColumn selectColumn in SelectColumns.Where(item => item.Aggregate == Aggregates.None))
                    {
                        stringBuilder.AppendLine();

                        stringBuilder.Append(first ? "GROUP BY    " : "          , ");

                        stringBuilder.Append(selectColumn.Join.Alias.MakeSafe());
                        stringBuilder.Append(".");
                        stringBuilder.Append(selectColumn.ColumnDef.Name.MakeSafe());

                        first = false;
                    }
                }

                first = true;

                if (SelectColumns
                    .Any(item => item.SortOrder != 0))
                {
                    foreach (SelectColumn selectColumn in SelectColumns
                        .Where(item => item.SortOrder != 0)
                        .OrderBy(item => Math.Abs(item.SortOrder)))
                    {

                        stringBuilder.AppendLine();

                        stringBuilder.Append(first ? "ORDER BY    " : "          , ");

                        if (selectColumn.Statement != null)
                        {
                            stringBuilder.Append(selectColumn.Statement);
                        }
                        else
                        {
                            stringBuilder.Append(selectColumn.Join.Alias.MakeSafe());
                            stringBuilder.Append(".");
                            stringBuilder.Append(selectColumn.ColumnDef.Name.MakeSafe());
                        }

                        if (selectColumn.SortOrder < 0)
                        {
                            stringBuilder.Append(" DESC");
                        }

                        first = false;
                    }

                }
                else if (IsPaged)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.Append("ORDER BY    ");
                    stringBuilder.Append(SelectColumns[0].Join.Alias.MakeSafe());
                    stringBuilder.Append(".");
                    stringBuilder.Append(SelectColumns[0].ColumnDef.Name.MakeSafe());
                }

                if (IsPaged)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.Append("OFFSET ");
                    stringBuilder.Append(PageSize * (PageNo - 1));
                    stringBuilder.Append(" ROWS FETCH NEXT ");
                    stringBuilder.Append(PageSize);
                    stringBuilder.Append(" ROWS ONLY");
                }

                return stringBuilder.ToString();
            }
        }



        private string GetSqlValue(string type, string value)
        {
            if (new[] { "varchar", "nvarchar", "char", "mchar" }.Contains(type))
            {
                return String.Format("'{0}'", value);
            }

            return value;
        }

        private IEnumerable<Join> GetRequiredJoins()
        {
            if (!SelectColumns.Any())
            {
                return Joins;
            }

            return Joins.Where(IsJoinRequired);
        }

        private bool IsJoinRequired(Join join)
        {
            //are any of the select query columns for this join Visible or Ordered
            bool selectColumns = SelectColumns
                .Any(item => (item.Join == join || (item.DependentOnAliases != null && item.DependentOnAliases.Contains(join.Alias)))
                    && (item.IsVisible || item.SortOrder != 0));

            bool whereColumns = WhereColumns.Any(item => item.Join == join);


            //are any of the sub joins required
            bool subJoinsRequired = Joins
                .Where(item => item.ParentJoin == join)
                .Any(IsJoinRequired);

            return selectColumns || whereColumns || subJoinsRequired;
        }

        public SelectStatement OrderBy(string alias)
        {
            SelectColumn selectColumn = SelectColumns.Single(item => item.Alias == alias);

            if (Math.Abs(selectColumn.SortOrder) == 1)
            {
                selectColumn.SortOrder = -selectColumn.SortOrder;
            }
            else
            {
                selectColumn.SortOrder = 1;

                int so = 2;
                foreach (SelectColumn sc in SelectColumns
                    .Where(item => item.Alias != alias && item.SortOrder != 0)
                    .OrderBy(item => Math.Abs(item.SortOrder)))
                {
                    sc.SortOrder = so++ * (sc.SortOrder > 0 ? 1 : -1);
                }
            }

            return this;
        }

        private void FixOrderBy()
        {
            int so = 1;
            foreach (SelectColumn sc in SelectColumns
                .Where(item => item.SortOrder != 0)
                .OrderBy(item => Math.Abs(item.SortOrder)))
            {
                sc.SortOrder = so++ * (sc.SortOrder > 0 ? 1 : -1);
            }
        }

        public DataSet Execute()
        {
            DataSet dataSet = new DataSet();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_executesql", connection) { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.Add(new SqlParameter("@statement", SqlDbType.NVarChar, -1) { Value = Statement });

                    int parameterId = 1;
                    string parameters = "";

                    foreach (WhereColumn wc in WhereColumns.Where(IsParamRequired))
                    {
                        parameters += parameters == "" ? "" : ", ";
                        parameters += "@p" + parameterId++ + @" " + wc.ColumnDef.Type;
                        switch (wc.ColumnDef.Type)
                        {
                            case "nvarchar":
                                parameters += String.Format(" ({0})", wc.ColumnDef.Length);
                                break;
                        }
                    }

                    command.Parameters.Add(new SqlParameter("@params", SqlDbType.NVarChar, -1) { Value = parameters });

                    parameterId = 1;
                    foreach (WhereColumn wc in WhereColumns.Where(IsParamRequired))
                    {
                        command.Parameters.Add(new SqlParameter("@p" + parameterId++, SqlDbType.NVarChar, -1) { Value = wc.Value1 });
                    }

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        dataAdapter.Fill(dataSet);
                    }
                }
            }

            return dataSet;
        }

        private bool IsParamRequired(WhereColumn whereColumn)
        {
            switch (whereColumn.Operator)
            {
                case Operators.IsFalse:
                case Operators.IsTrue:
                case Operators.IsNull:
                case Operators.IsNotNull:
                    return false;
            }

            return true;
        }
    }
}