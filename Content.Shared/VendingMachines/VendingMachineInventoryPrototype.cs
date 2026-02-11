using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.VendingMachines
{
    [Prototype]
    public sealed partial class VendingMachineInventoryPrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField("startingInventory", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<VendingMachineInventoryData, EntityPrototype>))]
        public Dictionary<string, VendingMachineInventoryData> StartingInventory { get; private set; } = new();

        [DataField("emaggedInventory", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<VendingMachineInventoryData, EntityPrototype>))]
        public Dictionary<string, VendingMachineInventoryData>? EmaggedInventory { get; private set; }

        [DataField("contrabandInventory", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<VendingMachineInventoryData, EntityPrototype>))]
        public Dictionary<string, VendingMachineInventoryData>? ContrabandInventory { get; private set; }
    }

}
