using Lymph.Managers.DiscordRPC.RPC.Payload;

namespace Lymph.Managers.DiscordRPC.RPC.Commands
{
    internal interface ICommand
    {
        IPayload PreparePayload(long nonce);
    }
}
