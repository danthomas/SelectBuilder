using System;

namespace SelectBuilder
{
    [Flags]
    public enum ColumnType
    {
        None,
        Select = 1,
        Where = 2,
        OrderBy = 4,
        All = Select | Where | OrderBy
    }
}
