using Robust.Shared.Timing;
using Content.Server.Radio.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Radio;
using Content.Shared.CartridgeLoader;
using Content.Shared.CartridgeLoader.Cartridges;
using Content.Shared.Paper;
using Content.Shared.Hands.EntitySystems;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Utility;
using Robust.Shared.GameObjects;

namespace Content.Server.CartridgeLoader.Cartridges;

public sealed class FinesCartridgeSystem : EntitySystem
{
    private static readonly SoundPathSpecifier PrinterSound = new("/Audio/Machines/printer.ogg");

    [Dependency] private readonly CartridgeLoaderSystem _cartridgeLoader = default!;
    [Dependency] private readonly PaperSystem _paper = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly RadioSystem _radio = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FinesCartridgeComponent, CartridgeMessageEvent>(OnUiMessage);
        SubscribeLocalEvent<FinesCartridgeComponent, CartridgeUiReadyEvent>(OnUiReady);
    }

    private void OnUiReady(Entity<FinesCartridgeComponent> ent, ref CartridgeUiReadyEvent args)
    {
        UpdateUiState(ent, args.Loader);
    }

    private void OnUiMessage(Entity<FinesCartridgeComponent> ent, ref CartridgeMessageEvent args)
    {
        if (args is not FinesUiMessageEvent message)
            return;

        if (message.Payload is FinesAddMessage addMsg)
        {
            ent.Comp.Fines.Add(addMsg.Fine);
            UpdateUiState(ent, GetEntity(args.LoaderUid));

            var officer = Name(args.Actor);

            _radio.SendRadioMessage(ent,
                Loc.GetString("fines-cartridge-announcement",
                    ("target", addMsg.Fine.Target),
                    ("amount", addMsg.Fine.Amount),
                    ("reason", addMsg.Fine.Reason),
                    ("officer", officer)
                ),
                ent.Comp.SecurityChannel,
                ent);
        }
        else if (message.Payload is FinesPrintMessage printMsg)
        {
            if (_timing.CurTime < ent.Comp.NextPrintAllowedAfter)
                return;

            ent.Comp.NextPrintAllowedAfter = _timing.CurTime + ent.Comp.PrintDelay;
            var fine = printMsg.Fine;
            var printed = Spawn("Paper", Transform(GetEntity(args.LoaderUid)).Coordinates);

            var msg = new FormattedMessage();
            msg.AddMarkupOrThrow(Loc.GetString("fines-cartridge-name") + "\n");
            msg.AddMarkupOrThrow(Loc.GetString("fines-cartridge-id", ("target", fine.Target)) + "\n");
            msg.AddMarkupOrThrow(Loc.GetString("fines-cartridge-reason", ("reason", fine.Reason)) + "\n");
            msg.AddMarkupOrThrow(Loc.GetString("fines-cartridge-amount", ("amount", fine.Amount)) + "\n");

            _paper.SetContent(printed, msg.ToMarkup());
            _audio.PlayPvs(PrinterSound, GetEntity(args.LoaderUid));

            _hands.PickupOrDrop(args.Actor, printed);
        }
    }

    private void UpdateUiState(Entity<FinesCartridgeComponent> ent, EntityUid loaderUid)
    {
        var state = new FinesUiState(ent.Comp.Fines);
        _cartridgeLoader.UpdateCartridgeUiState(loaderUid, state);
    }
}
