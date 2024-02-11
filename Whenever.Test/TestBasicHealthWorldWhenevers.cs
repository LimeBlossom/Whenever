using System.Linq;
using NUnit.Framework;
using Whenever.Core;
using Whenever.Core.CommandInitiators;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt;
using Whenever.HealthExt.World;

namespace Whenever.Test
{
    using WheneverType = Whenever.Core.Whenever<IInspectWorldHealth, ICommandWorldHealth>;

    
    public class TestBasicHealthWorldWhenevers
    {
        private (CombatantId player, CombatantId enemy, IManageWorld<IInspectWorldHealth, ICommandWorldHealth> manager, IInspectWorldHealth inspector, HealthWorld world) GetEnemyAndPlayerTurnContext()
        {
            var player = new HealthCombatant(10);
            var enemy = new HealthCombatant(10);
            var combatants = new[] {player, enemy};
            var turnManager = new HealthWorld(combatants.ToList());
            var wheneverManager =
                new WheneverManager<IInspectWorldHealth, ICommandWorldHealth>(turnManager, turnManager);
            return (
                player.id,
                enemy.id,
                wheneverManager,
                turnManager,
                turnManager);
        }
        
        [Test]
        public void WhenPlayerTakesDamage__TakesDamage()
        {
            var (player, _, turnManager, inspector, _) = GetEnemyAndPlayerTurnContext();
            var dmg = HealthExt.Commands.Factory.Damage(player, 2);
            var initiated = new InitiatedCommand<ICommandWorldHealth>(dmg, InitiatorFactory.FromNone("test"));
            Assert.AreEqual(10, inspector.GetHealth(player));
            turnManager.InitiateCommandBatch(new[] { initiated });
            Assert.AreEqual(8, inspector.GetHealth(player));
        }

        [Test]
        public void WheneverPlayerDealsDamage_ApplyDotToTarget__AppliedDot()
        {
            var (player, enemy, turnManager, inspector, world) = GetEnemyAndPlayerTurnContext();
            var filter = HealthExt.Filters.Factory.CreateDamageOccursFilter(2);
            WheneverType damageOccursAppliesDotToTarget = new(filter, HealthExt.Effects.Factory.DotTarget(1, 3));
            turnManager.AddWhenever(damageOccursAppliesDotToTarget);
            
            Assert.AreEqual(10, inspector.GetHealth(enemy));
            
            var dmg = HealthExt.Commands.Factory.Damage(enemy, 2);
            var initiated = new InitiatedCommand<ICommandWorldHealth>(dmg, InitiatorFactory.From(player));
            turnManager.InitiateCommandBatch(new[] { initiated });
            
            Assert.AreEqual(8, inspector.GetHealth(enemy));

            turnManager.InitiateCommandBatch(world.ApplyAllStatusEffects());
            Assert.AreEqual(7, inspector.GetHealth(enemy));
            
            turnManager.InitiateCommandBatch(world.ApplyAllStatusEffects());
            Assert.AreEqual(6, inspector.GetHealth(enemy));
            
            turnManager.InitiateCommandBatch(world.ApplyAllStatusEffects());
            Assert.AreEqual(5, inspector.GetHealth(enemy));
            
            turnManager.InitiateCommandBatch(world.ApplyAllStatusEffects());
            Assert.AreEqual(5, inspector.GetHealth(enemy));
        }
    }
}