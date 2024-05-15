using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Threading;
using System.Threading.Tasks;

public class DbInterceptor : SaveChangesInterceptor
{
    private readonly string[] auditingProperties = { "CreatedDate", "CreatedBy", "LastUpdatedDate", "LastUpdatedBy", "ActiveFlag" };

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                SetAuditingProperties(entry.CurrentValues, entry.Metadata);
            }
            else if (entry.State == EntityState.Modified)
            {
                SetModifiedAuditingProperties(entry.CurrentValues, entry.Metadata);
            }
        }

        return base.SavingChanges(eventData, result);
    }

    private void SetAuditingProperties(Microsoft.EntityFrameworkCore.ChangeTracking.PropertyValues propertyValues, IEntityType entityType)
    {
        foreach (var propertyName in auditingProperties)
        {
            if (entityType.FindProperty(propertyName) != null)
            {
                if (propertyName == "CreatedDate" || propertyName == "LastUpdatedDate")
                {
                    propertyValues[propertyName] = DateTime.UtcNow;
                }
                else if (propertyName == "CreatedBy" || propertyName == "LastUpdatedBy")
                {
                    propertyValues[propertyName] = "sa";
                }
                else if (propertyName == "ActiveFlag")
                {
                    propertyValues[propertyName] = true;
                }
            }
        }
    }

    private void SetModifiedAuditingProperties(Microsoft.EntityFrameworkCore.ChangeTracking.PropertyValues propertyValues, IEntityType entityType)
    {
        foreach (var propertyName in auditingProperties)
        {
            if (entityType.FindProperty(propertyName) != null)
            {
                if (propertyName == "LastUpdatedDate")
                {
                    propertyValues[propertyName] = DateTime.UtcNow;
                }
                else if (propertyName == "LastUpdatedBy")
                {
                    propertyValues[propertyName] = "sa";
                }
            }
        }
    }
}