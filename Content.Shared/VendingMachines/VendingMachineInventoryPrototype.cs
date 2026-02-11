using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.VendingMachines
{
    [Prototype]
    public sealed partial class VendingMachineInventoryPrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField]
        public List<VendingMachineInventoryEntryForPrototype> StartingInventory { get; private set; } = new();

        [DataField]
        public List<VendingMachineInventoryEntryForPrototype>? EmaggedInventory { get; private set; }

        [DataField]
        public List<VendingMachineInventoryEntryForPrototype>? ContrabandInventory { get; private set; }
    }

}
