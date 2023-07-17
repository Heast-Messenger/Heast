using Client.Attributes;

namespace Client.Model;

public enum ConnectionStatus
{
    [StringValue("Pending")] Pending,
    [StringValue("Failed")] Failed,
    [StringValue("Successful")] Successful
}