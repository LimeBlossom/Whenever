using Whenever.Core;
using Whenever.Core.WorldInterface;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Commands
{
    public static class CmdFactory
    {
        public static IWorldCommand<ICommandableWorldDemo> Damage(DamageType type, int amount, CombatantId target)
        { 
            return new DamageCommand(target, new DamagePackage(type, amount));
        }
    }
}