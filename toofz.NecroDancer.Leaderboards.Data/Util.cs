namespace toofz.NecroDancer.Leaderboards
{
    static class Util
    {
        // https://stackoverflow.com/questions/10869062/removing-a-default-constraint-after-adding-a-new-column-in-code-first
        public static string DropDefaultConstraint(string table, string column)
        {
            return $@"DECLARE @name sysname

SELECT @name = dc.name
FROM sys.columns c
JOIN sys.default_constraints dc ON dc.object_id = c.default_object_id
WHERE c.object_id = OBJECT_ID('{table}')
AND c.name = '{column}'

IF @name IS NOT NULL
    EXECUTE ('ALTER TABLE {table} DROP CONSTRAINT ' + @name)";
        }
    }
}
