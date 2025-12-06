namespace AspCoreFirstApp.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string TableName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string EntityKey { get; set; } = null!;
    public string? Changes { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}
