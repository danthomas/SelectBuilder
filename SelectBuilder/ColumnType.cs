using System;

namespace SelectBuilder
{
    [Flags]
    public enum ColumnType
    {
        Identifier = 1,
        Select = 2,
        Where = 4,
        OrderBy = 8,
        SelectWhereOrderBy = Select | Where | OrderBy
    }
}
