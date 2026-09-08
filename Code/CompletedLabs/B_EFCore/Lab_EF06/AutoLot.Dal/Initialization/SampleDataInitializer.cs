// Copyright Information
// ==================================
// AutoLot - AutoLot.Dal - SampleDataInitializer.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/05
// ==================================

namespace AutoLot.Dal.Initialization;

public static class SampleDataInitializer
{
    internal static void ClearData(
        ApplicationDbContext context)
    {
        var entities =
            new[]
            {
                typeof(CarDriver).FullName,
                typeof(Driver).FullName,
                typeof(Radio).FullName,
                typeof(Car).FullName,
                typeof(Make).FullName,
                typeof(SeriLogEntry).FullName
            };
        IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                foreach (var entityName in entities)
                {
                    var entity = context.Model.FindEntityType(entityName);
                    var tableName = entity.GetTableName();
                    var schemaName = entity.GetSchema();
#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
                    context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");
                    context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 1);");
#pragma warning restore EF1002 // Risk of vulnerability to SQL injection.
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        });
    }

    internal static void SeedData(
        ApplicationDbContext context)
    {
        IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                ProcessInsert(
                    context,
                    context.Makes,
                    SampleData.Makes);
                ProcessInsert(
                    context,
                    context.Drivers,
                    SampleData.Drivers);
                ProcessInsert(
                    context,
                    context.Cars,
                    SampleData.Inventory);
                ProcessInsert(
                    context,
                    context.Radios,
                    SampleData.Radios);
                ProcessInsert(
                    context,
                    context.CarDrivers,
                    SampleData.CarsAndDrivers);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        });

        static void ProcessInsert<TEntity>(
            ApplicationDbContext context,
            DbSet<TEntity> table,
            List<TEntity> records) where TEntity : BaseEntity
        {
            if (table.Any())
            {
                return;
            }

            var metaData = context.Model.FindEntityType(typeof(TEntity).FullName);
            var identityInsertSql = $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()}";
            try
            {
#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
                context.Database.ExecuteSqlRaw($"{identityInsertSql} ON");
                table.AddRange(records);
                context.SaveChanges();
            }
            finally
            {
                // Ensure IDENTITY_INSERT is always turned off, even on failure.
                context.Database.ExecuteSqlRaw($"{identityInsertSql} OFF");
#pragma warning restore EF1002 // Risk of vulnerability to SQL injection.
            }
        }
    }

    public static void ClearAndReseedDatabase(
        ApplicationDbContext context)
    {
        ClearData(context);
        SeedData(context);
    }
}