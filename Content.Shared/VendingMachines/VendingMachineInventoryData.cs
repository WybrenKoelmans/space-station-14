using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Shared.VendingMachines;

/// <summary>
/// Data definition for a vending machine inventory entry.
/// Can be implicitly deserialized from a uint (amount) for backward compatibility.
/// </summary>
[DataDefinition]
[Serializable, NetSerializable]
public partial struct VendingMachineInventoryData
{
    [DataField("amount")]
    public uint Amount;

    [DataField("price")]
    public int Price;
}

[TypeSerializer]
public sealed class VendingMachineInventoryDataSerializer :
    ITypeSerializer<VendingMachineInventoryData, MappingDataNode>,
    ITypeSerializer<VendingMachineInventoryData, ValueDataNode>
{
    public ValidationNode Validate(ISerializationManager serializationManager, ValueDataNode node,
        IDependencyCollection dependencies, ISerializationContext? context = null)
    {
        return serializationManager.ValidateNode<uint>(node, context);
    }

    public VendingMachineInventoryData Read(ISerializationManager serializationManager,
        ValueDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<VendingMachineInventoryData>? instanceProvider = null)
    {
        var amount = serializationManager.Read<uint>(node, context);
        return new VendingMachineInventoryData { Amount = amount, Price = 5 };
    }

    public ValidationNode Validate(ISerializationManager serializationManager, MappingDataNode node,
        IDependencyCollection dependencies, ISerializationContext? context = null)
    {
        var mappingResults = new Dictionary<ValidationNode, ValidationNode>();

        if (node.TryGet("amount", out var amountNode))
            mappingResults.Add(new ValidatedValueNode(new ValueDataNode("amount")), serializationManager.ValidateNode<uint>(amountNode, context));

        if (node.TryGet("price", out var priceNode))
            mappingResults.Add(new ValidatedValueNode(new ValueDataNode("price")), serializationManager.ValidateNode<int>(priceNode, context));

        return new ValidatedMappingNode(mappingResults);
    }

    public VendingMachineInventoryData Read(ISerializationManager serializationManager,
        MappingDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<VendingMachineInventoryData>? instanceProvider = null)
    {
        var amount = 1u;
        var price = 5;

        if (node.TryGet("amount", out var amountNode))
            amount = serializationManager.Read<uint>(amountNode, context);

        if (node.TryGet("price", out var priceNode))
            price = serializationManager.Read<int>(priceNode, context);

        return new VendingMachineInventoryData { Amount = amount, Price = price };
    }

    public DataNode Write(ISerializationManager serializationManager, VendingMachineInventoryData value,
        IDependencyCollection dependencies, bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        // If price is 0, write as scalar to keep YAMLs clean? 
        // Existing YAML generation tools might prefer consistent output.
        // But for game saving/blueprints, maybe mapping is fine.
        // Let's stick to full mapping for consistency when writing, 
        // OR scalar if price is 0 for backward copat? (though we rarely write prototypes back).

        var mapping = new MappingDataNode();
        mapping.Add("amount", serializationManager.WriteValue(value.Amount, alwaysWrite, context));
        if (value.Price != 5 || alwaysWrite)
        {
            mapping.Add("price", serializationManager.WriteValue(value.Price, alwaysWrite, context));
        }

        return mapping;
    }

    public VendingMachineInventoryData Copy(ISerializationManager serializationManager, VendingMachineInventoryData source,
        VendingMachineInventoryData target, bool skipHook, ISerializationContext? context = null)
    {
        return source;
    }
}
