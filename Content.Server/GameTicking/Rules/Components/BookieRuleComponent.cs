using Content.Shared.Random;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// Stores data for <see cref="BookieRuleSystem"/>.
/// </summary>
[RegisterComponent, Access(typeof(BookieRuleSystem))]
public sealed partial class BookieRuleComponent : Component;
