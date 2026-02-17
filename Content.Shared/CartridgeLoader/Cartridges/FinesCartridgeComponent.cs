using Robust.Shared.Serialization;
using Robust.Shared.GameStates;

namespace Content.Shared.CartridgeLoader.Cartridges;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FinesCartridgeComponent : Component
{
    [DataField, AutoNetworkedField]
    public List<Fine> Fines = new();
}

[DataDefinition, Serializable, NetSerializable]
public partial struct Fine
{
    [DataField]
    public string Target;

    [DataField]
    public string Reason;

    [DataField]
    public int Amount;

    public Fine(string target, string reason, int amount)
    {
        Target = target;
        Reason = reason;
        Amount = amount;
    }
}
