using Core.Network.Packets.S2C;

namespace Core.Model;

public class Account
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required byte[] Token { get; init; }

    public static implicit operator Account?(AccountRequestS2CPacket packet)
    {
        if (packet.HasErrors())
        {
            return null;
        }

        return new Account
        {
            Name = packet.Name,
            Email = packet.Email,
            Password = packet.Password,
            Token = packet.Token
        };
    }
}