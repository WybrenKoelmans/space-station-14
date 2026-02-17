using Content.Client.UserInterface.Fragments;
using Content.Shared.CartridgeLoader;
using Content.Shared.CartridgeLoader.Cartridges;
using Robust.Client.UserInterface;
using Robust.Shared.GameObjects;

namespace Content.Client.CartridgeLoader.Cartridges;

public sealed partial class FinesUi : UIFragment
{
    private FinesUiFragment? _fragment;

    public override Control GetUIFragmentRoot()
    {
        return _fragment!;
    }

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        _fragment = new FinesUiFragment();

        _fragment.OnAddFine += fine =>
        {
            userInterface.SendMessage(new CartridgeUiMessage(new FinesUiMessageEvent(new FinesAddMessage(fine))));
        };

        _fragment.OnPrintFine += fine =>
        {
            userInterface.SendMessage(new CartridgeUiMessage(new FinesUiMessageEvent(new FinesPrintMessage(fine))));
        };
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is FinesUiState cast)
        {
            _fragment?.UpdateState(cast.Fines);
        }
    }
}
