using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace toofz.NecroDancer.Leaderboards
{
    internal static class EntityFrameworkExtensions
    {
        #region https://romiller.com/2014/04/08/ef6-1-mapping-between-types-tables/

        public static MappingFragment GetMappingFragment<TEntity>(this DbContext db)
            where TEntity : class
        {
            var metadata = ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var type = typeof(TEntity);
            var entityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .Single(e => objectItemCollection.GetClrType(e) == type);

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata
                .GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                .Single()
                .EntitySetMappings
                .Single(s => s.EntitySet == entitySet);

            return mapping
                .EntityTypeMappings
                .Single()
                .Fragments
                .Single();
        }

        public static string GetTableName(this MappingFragment mappingFragment)
        {
            // Find the storage entity set (table) that the entity is mapped
            var table = mappingFragment.StoreEntitySet;

            // Return the table name from the storage entity set
            return (string)table.MetadataProperties["Table"].Value ?? table.Name;
        }

        public static IEnumerable<ScalarPropertyMapping> GetScalarPropertyMappings(this MappingFragment mappingFragment)
        {
            return mappingFragment
                .PropertyMappings
                .OfType<ScalarPropertyMapping>()
                .ToList();
        }

        public static IEnumerable<string> GetColumnNames(this MappingFragment mappingFragment)
        {
            return GetScalarPropertyMappings(mappingFragment)
                .Select(m => m.Column.Name)
                .ToList();
        }

        public static IEnumerable<string> GetPrimaryKeyColumnNames(this MappingFragment mappingFragment)
        {
            return mappingFragment
                .StoreEntitySet
                .ElementType
                .KeyProperties // What's the difference between KeyMembers and KeyProperties?
                .Select(c => c.Name)
                .ToList();
        }

        #endregion
    }
}
