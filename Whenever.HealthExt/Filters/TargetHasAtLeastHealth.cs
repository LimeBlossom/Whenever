
using System;
using Serialization;
using UnityEngine;

[PolymorphicSerializable("TargetHasAtLeastHealth"), Serializable]
public record TargetHasAtLeastHealth : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
{
    [SerializeField]
    private float atLeast;

    public TargetHasAtLeastHealth(float atLeast)
    {
        this.atLeast = atLeast;
    }

    public bool TriggersOn(InitiatedCommand<ICommandWorldHealth> initiatedCommand, IInspectWorldHealth world)
    {
        if (initiatedCommand.command is IGenericTargetedWorldCommand<ICommandWorldHealth> targetedCommand)
        {
            return world.GetHealth(targetedCommand.Target) >= atLeast;
        }
            
        return false;
    }

    public string Describe()
    {
        return $"target has at least {atLeast} health";
    }
}