using Core.Model;
using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public partial class AccountRequestS2CPacket : AbstractPacket
{
    public AccountRequestS2CPacket(Account account)
    {
        Name = account.Name;
        Email = account.Email;
        Password = account.Password;
        Token = account.Token;
    }

    public string Name { get; }
    public string Email { get; }
    public string Password { get; }
    public byte[] Token { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientAuthListener clientAuthListener)
        {
            clientAuthListener.OnAccountRequest(this);
        }
    }
}