using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using AspCoreFirstApp.Models;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AddAuditLogs(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AddAuditLogs(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AddAuditLogs(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            var tableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name;
            var key = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "";

            string action = entry.State.ToString(); // Added, Modified, Deleted

            // collecte des changements
            Dictionary<string, object?> changes = new();
            foreach (var prop in entry.Properties)
            {
                if (entry.State == EntityState.Added)
                    changes[prop.Metadata.Name] = prop.CurrentValue;
                else if (entry.State == EntityState.Deleted)
                    changes[prop.Metadata.Name] = prop.OriginalValue;
                else if (entry.State == EntityState.Modified && prop.IsModified)
                    changes[prop.Metadata.Name] = new { Original = prop.OriginalValue, Current = prop.CurrentValue };
            }

            var audit = new AuditLog
            {
                TableName = tableName,
                Action = action,
                EntityKey = key ?? "",
                Changes = JsonSerializer.Serialize(changes),
                Date = DateTime.UtcNow
            };

            // On ne veut pas déclencher d'autres logs en ajoutant l'audit :
            context.Set<AuditLog>().Add(audit);
        }
    }
}
