
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Strateq.Core.Database.DbModel;
using System;
using System.Collections.Generic;

namespace Strateq.Core.Database.Helper
{
    public class AuditHelper
    {
        public static AuditLogEntry AddAuditLogAdded(EntityEntry entry, string source = "")
        {
            AuditLogEntry auditEntry = new();
            var tableName = entry.Metadata.GetTableName();

            if (tableName == "RequestLog" || tableName == "SystemLog") return null;

            AuditLog auditmodel = new AuditLog()
            {
                AffectedTable = tableName
            };

            auditmodel.Operation = Operation.Added;
            foreach (var property in entry.Properties)
            {
                var columnName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditmodel.AffectedId = int.Parse(property.CurrentValue.ToString());
                }
                var modifiedObj = entry.CurrentValues[columnName];
                var modifiedValue = modifiedObj?.ToString() ?? "";
                AuditLogDetail newDetail = new AuditLogDetail()
                {
                    ColumnName = columnName,
                    OriginalValue = string.Empty,
                    NewValue = modifiedValue,
                };
                auditEntry.Details.Add(newDetail);
            }

            auditEntry.Auditlog = auditmodel;
            return auditEntry;
        }

        public static AuditLogEntry AddAuditLogUpdate(EntityEntry entry)
        {
            AuditLogEntry auditEntry = new();
            var tableName = entry.Metadata.GetTableName();

            if (tableName == "RequestLog" || tableName == "SystemLog") return null;

            AuditLog auditmodel = new AuditLog()
            {
                AffectedTable = tableName
            };

            if (entry.State == EntityState.Modified)
            {
                auditmodel.Operation = Operation.Modified;
                foreach (var property in entry.Properties)
                {
                    var ori = entry.GetDatabaseValues();
                    if (ori != null)
                    {
                        var columnName = property.Metadata.Name;
                        if (property.Metadata.IsPrimaryKey())
                        {
                            auditmodel.AffectedId = int.Parse(property.CurrentValue.ToString());
                        }
                        var originalObj = ori[columnName];
                        var modifiedObj = entry.CurrentValues[columnName];
                        var originalValue = originalObj?.ToString() ?? "";
                        var modifiedValue = modifiedObj?.ToString() ?? "";
                        AuditLogDetail newDetail = new AuditLogDetail()
                        {
                            ColumnName = columnName,
                            OriginalValue = originalValue,
                            NewValue = modifiedValue,
                        };
                        if (originalValue != modifiedValue)
                        {
                            auditEntry.Details.Add(newDetail);
                        }
                    }
                }
            }
            else if (entry.State == EntityState.Deleted)
            {
                auditmodel.Operation = Operation.Deleted;
                foreach (var property in entry.Properties)
                {
                    var columnName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditmodel.AffectedId = int.Parse(property.CurrentValue.ToString());
                    }
                    var originalObj = entry.OriginalValues[columnName];
                    var originalValue = originalObj?.ToString() ?? "";
                    AuditLogDetail newDetail = new AuditLogDetail()
                    {
                        ColumnName = columnName,
                        OriginalValue = originalValue,
                        NewValue = string.Empty,
                    };
                    auditEntry.Details.Add(newDetail);
                }
            }

            auditEntry.Auditlog = auditmodel;
            return auditEntry;
        }
    }

    public class AuditLogEntry
    {
        public AuditLog Auditlog { get; set; }

        public List<AuditLogDetail> Details { get; set; } = new();
    }

    public struct Operation
    {
        public static string Unchanged = "Unchanged";
        public static string Added = "Added";
        public static string Modified = "Modified";
        public static string Deleted = "Deleted";
    }
}


