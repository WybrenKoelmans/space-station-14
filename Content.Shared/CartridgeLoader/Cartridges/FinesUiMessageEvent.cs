using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.Cartridges;

[Serializable, NetSerializable]
public sealed class FinesUiMessageEvent : CartridgeMessageEvent
{
    public readonly IFinesUiMessagePayload Payload;
    public FinesUiMessageEvent(IFinesUiMessagePayload payload)
    {
        Payload = payload;
    }
}

public interface IFinesUiMessagePayload
{
}

[Serializable, NetSerializable]
public sealed class FinesAddMessage : IFinesUiMessagePayload
{
    public Fine Fine;

    public FinesAddMessage(Fine fine)
    {
        Fine = fine;
    }
}

[Serializable, NetSerializable]
public sealed class FinesPrintMessage : IFinesUiMessagePayload
{
    public Fine Fine;

    public FinesPrintMessage(Fine fine)
    {
        Fine = fine;
    }
}
