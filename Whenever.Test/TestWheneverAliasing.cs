using System.Linq;
using CoreFac;
using NUnit.Framework;
using UnityEngine;
using Whenever.DmgTypeEtcExt.Experimental;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.Effects;
using Whenever.DmgTypeEtcExt.Experimental.Filters;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.Test
{
    using WheneverType = Whenever<IInspectableWorldDemo, ICommandableWorldDemo>;
    
    public class TestWheneverAliasing
    {
        private (CombatantId player, CombatantId enemy, IManageWorld<IInspectableWorldDemo, ICommandableWorldDemo> turnManager, GlobalCombatWorldDemo baseWorld) GetEnemyAndPlayerTurnContext()
        {
            var player = new Combatant(10, CombatantType.Player);
            var enemy = new Combatant(10, CombatantType.Enemy);
            var combatants = new[] {player, enemy};
            var turnManager = new GlobalCombatWorldDemo(combatants.ToList(), 197271);
            var wheneverManager =
                new WheneverManager<IInspectableWorldDemo, ICommandableWorldDemo>(turnManager, turnManager);

            return (
                turnManager.GetOfType(CombatantType.Player).Single(),
                turnManager.GetOfType(CombatantType.Enemy).Single(), 
                wheneverManager,
                turnManager);
        }

        [Test]
        public void WhenPlayer_PlaysCard_BakesEnemy_AsCardTarget()
        {
            var (player, enemy, turnManager, baseWorld) = GetEnemyAndPlayerTurnContext();
        
            // whenever the card target takes physical damage, heal the initiator
            var wheneverCardTargetTakesPhysicalDamage = WheneverFilterFactory.Compose(
                    WheneverFilterFactory.DamageIs(DamageType.PHYSICAL),
                    WheneverFilterFactory.TargetIs(CombatantAlias.FromId("#cardTarget"))
                    );
            var healCaster = EffectFactory.Heal(StandardAliases.Initiator, 3);
            
            var aliaser = new SimpleCombatantAliaser();
            var targetedAliaser = aliaser.WithOverrides(
                ("#cardTarget", enemy),
                ("#cardCaster", player)
            );
            var whenever = new WheneverType(wheneverCardTargetTakesPhysicalDamage, healCaster);
            whenever = whenever.BakeCombatantAlias(targetedAliaser);
            
            turnManager.AddWhenever(whenever);
            
            // player takes damage
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 4, player),
                Initiators.From(enemy));
        
            Assert.AreEqual(6, baseWorld.CombatantData(player).CurrentHealth());
            Assert.AreEqual(10, baseWorld.CombatantData(enemy).CurrentHealth());
        
            // player deals damage, as the caster of the card
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 1, enemy),
                Initiators.From(player),
                aliaser: aliaser);
        
            Assert.AreEqual(9, baseWorld.CombatantData(player).CurrentHealth());
            Assert.AreEqual(9, baseWorld.CombatantData(enemy).CurrentHealth());
            
            // enemy deals damage, overrides aliasing with baked alias
            targetedAliaser = aliaser.WithOverrides(
                ("#cardTarget", player),
                ("#cardCaster", enemy)
            );
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 1, player),
                Initiators.From(enemy),
                aliaser: targetedAliaser);
            
            Assert.AreEqual(8, baseWorld.CombatantData(player).CurrentHealth());
            Assert.AreEqual(9, baseWorld.CombatantData(enemy).CurrentHealth());
        }
    }
}