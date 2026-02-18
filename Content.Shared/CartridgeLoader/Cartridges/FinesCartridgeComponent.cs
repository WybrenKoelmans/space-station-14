using Robust.Shared.Serialization;
using Robust.Shared.GameStates;
using Content.Shared.Radio;
using Robust.Shared.Prototypes;

namespace Content.Shared.CartridgeLoader.Cartridges;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FinesCartridgeComponent : Component
{
    [DataField, AutoNetworkedField]
    public List<Fine> Fines = new();

    /// <summary>
    /// The next time the cartridge can print.
    /// </summary>
    [DataField]
    public TimeSpan NextPrintAllowedAfter;

    /// <summary>
    /// The delay between printing actions.
    /// </summary>
    [DataField]
    public TimeSpan PrintDelay = TimeSpan.FromSeconds(5);
    /// <summary>
    /// Channel to send radio announcements on.
    /// </summary>
    [DataField]
    public ProtoId<RadioChannelPrototype> SecurityChannel = "Security";}

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
