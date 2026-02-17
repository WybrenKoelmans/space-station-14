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
    [Dependency] private readonly CartridgeLoaderSystem _cartridgeLoader = default!;
    [Dependency] private readonly PaperSystem _paper = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;

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
        }
        else if (message.Payload is FinesPrintMessage printMsg)
        {
            var fine = printMsg.Fine;
            var printed = Spawn("Paper", Transform(GetEntity(args.LoaderUid)).Coordinates);
            
            var msg = new FormattedMessage();
            msg.AddMarkupOrThrow($"[center][bold]Security Fine[/bold][/center]\n");
            msg.AddMarkupOrThrow($"[bold]Target:[/bold] {fine.Target}\n");
            msg.AddMarkupOrThrow($"[bold]Reason:[/bold] {fine.Reason}\n");
            msg.AddMarkupOrThrow($"[bold]Amount:[/bold] {fine.Amount} credits\n");
            
            _paper.SetContent(printed, msg.ToMarkup());
            _audio.PlayPvs(new SoundPathSpecifier("/Audio/Machines/printer.ogg"), GetEntity(args.LoaderUid));
            
            _hands.PickupOrDrop(args.Actor, printed); 
        }
    }

    private void UpdateUiState(Entity<FinesCartridgeComponent> ent, EntityUid loaderUid)
    {
        var state = new FinesUiState(ent.Comp.Fines);
        _cartridgeLoader.UpdateCartridgeUiState(loaderUid, state);
    }
}
