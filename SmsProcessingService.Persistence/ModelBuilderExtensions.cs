using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;
using Npgsql.NameTranslation;

namespace SmsProcessingService.Persistence
{
    public static class ModelBuilderExtensions
    {
        private static readonly Regex _keysRegex = new("^(PK|FK|IX)_", RegexOptions.Compiled);

        public static void UseSnakeCaseNames(this ModelBuilder modelBuilder)
        {
            var mapper = new NpgsqlSnakeCaseNameTranslator();

            foreach (var table in modelBuilder.Model.GetEntityTypes())
            {
                ConvertToSnake(mapper, table);

                foreach (var property in table.GetProperties()) ConvertToSnake(mapper, property);

                foreach (var primaryKey in table.GetKeys()) ConvertToSnake(mapper, primaryKey);

                foreach (var foreignKey in table.GetForeignKeys()) ConvertToSnake(mapper, foreignKey);

                foreach (var indexKey in table.GetIndexes()) ConvertToSnake(mapper, indexKey);
            }
        }

        private static void ConvertToSnake(INpgsqlNameTranslator mapper, object entity)
        {
            switch (entity)
            {
                case IMutableEntityType table:
                    table.SetTableName(ConvertGeneralToSnake(mapper, table.GetTableName()));
                    break;
                case IMutableProperty property:
                    property.SetColumnName(ConvertGeneralToSnake(mapper, property.GetColumnName()));
                    break;
                case IMutableKey primaryKey:
                    primaryKey.SetName(ConvertKeyToSnake(mapper, primaryKey.GetName()));
                    break;
                case IMutableForeignKey foreignKey:
                    foreignKey.SetConstraintName(ConvertKeyToSnake(mapper, foreignKey.GetConstraintName()));
                    break;
                case IMutableIndex indexKey:
                    indexKey.SetDatabaseName(ConvertKeyToSnake(mapper, indexKey.GetDatabaseName()));
                    break;
                default:
                    throw new NotImplementedException("Unexpected type was provided to snake case converter");
            }
        }

        private static string ConvertKeyToSnake(INpgsqlNameTranslator mapper, string keyName)
        {
            return ConvertGeneralToSnake(mapper, _keysRegex.Replace(keyName, match => match.Value.ToLower()));
        }

        private static string ConvertGeneralToSnake(INpgsqlNameTranslator mapper, string entityName)
        {
            return mapper.TranslateMemberName(entityName);
        }
    }
}
