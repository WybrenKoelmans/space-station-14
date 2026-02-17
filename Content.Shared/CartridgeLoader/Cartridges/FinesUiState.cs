using Robust.Shared.Serialization;
using Robust.Shared.GameObjects;
using System.Collections.Generic;

namespace Content.Shared.CartridgeLoader.Cartridges;

[Serializable, NetSerializable]
public sealed class FinesUiState : BoundUserInterfaceState
{
    public List<Fine> Fines;

    public FinesUiState(List<Fine> fines)
    {
        Fines = fines;
    }
}
