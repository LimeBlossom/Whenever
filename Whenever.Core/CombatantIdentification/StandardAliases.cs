
public static class StandardAliases
{
    // TODO: maybe we don't want to embed a description meant to be displayed to the user in the aliases 
    public static CombatantAlias Initiator => CombatantAlias.FromId("#initiator", "the initiator");
    public static CombatantAlias Target => CombatantAlias.FromId("#target", "the target");
}