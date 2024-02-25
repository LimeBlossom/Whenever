using System;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Filters
{
    public record CombatantsAreSame : IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        private readonly CombatantAlias variableAlias;
        private readonly CombatantAlias expectedAlias;

        public CombatantsAreSame(CombatantAlias variableAlias, CombatantAlias expectedAlias)
        {
            this.variableAlias = variableAlias;
            this.expectedAlias = expectedAlias;
        }

        public bool TriggersOn(
            InitiatedCommand<ICommandableWorldDemo> initiatedCommand,
            IAliasCombatantIds aliaser,
            IInspectableWorldDemo world)
        {
            var variableId = aliaser.GetIdForAlias(variableAlias);
            var expectedId = aliaser.GetIdForAlias(expectedAlias);
            return variableId == expectedId;
        }
        
        public string Describe(IDescriptionContext context)
        {
            return $"{context.NameOf(variableAlias)} is {context.NameOf(expectedAlias)}";
        }
    }
}