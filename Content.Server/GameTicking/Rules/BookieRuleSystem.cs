using Content.Server.Antag;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Roles;
using Content.Shared.Humanoid;
using Content.Shared.Roles.Components;

namespace Content.Server.GameTicking.Rules;

public sealed class BookieRuleSystem : GameRuleSystem<BookieRuleComponent>
{
    [Dependency] private readonly AntagSelectionSystem _antag = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BookieRuleComponent, AfterAntagEntitySelectedEvent>(AfterAntagSelected);

        SubscribeLocalEvent<BookieRoleComponent, GetBriefingEvent>(OnGetBriefing);
    }

    // Greeting upon bookie activation
    private void AfterAntagSelected(Entity<BookieRuleComponent> mindId, ref AfterAntagEntitySelectedEvent args)
    {
        var ent = args.EntityUid;
        _antag.SendBriefing(ent, MakeBriefing(ent), null, null);
    }

    // Character screen briefing
    private void OnGetBriefing(Entity<BookieRoleComponent> role, ref GetBriefingEvent args)
    {
        var ent = args.Mind.Comp.OwnedEntity;

        if (ent is null)
            return;
        args.Append(MakeBriefing(ent.Value));
    }

    private string MakeBriefing(EntityUid ent)
    {
        var isHuman = HasComp<HumanoidProfileComponent>(ent);
        var briefing = isHuman
            ? Loc.GetString("bookie-role-greeting-human")
            : Loc.GetString("bookie-role-greeting-animal");

        if (isHuman)
            briefing += "\n \n" + Loc.GetString("bookie-role-greeting-equipment") + "\n";

        return briefing;
    }
}
