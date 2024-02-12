using System.Linq;
using NUnit.Framework;
using Whenever.Core;
using Whenever.Core.CommandInitiators;
using Whenever.Core.Commands;
using Whenever.Core.Effects;
using Whenever.Core.WheneverFilter;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt;
using Whenever.HealthExt.World;

namespace Whenever.Test
{
    using WheneverType = Whenever.Core.Whenever<IInspectWorldHealth, ICommandWorldHealth>;

    
    public class TestBasicHealthWorldWhenevers
    {
        private record TestContext
        {
            public CombatantId player;
            public CombatantId enemy;
            public IManageWorld<IInspectWorldHealth, ICommandWorldHealth> turnManager;
            public IInspectWorldHealth inspector;
            public HealthWorld world;
            
            public void AddWhenever(
                IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth> filter,
                IEffect<IInspectWorldHealth, ICommandWorldHealth> effect,
                string expectedDescription)
            {
                var newWhenever = new WheneverType(filter, effect);
                this.AddWhenever(newWhenever, expectedDescription);
            }
            public void AddWhenever(
                WheneverType whenever,
                string expectedDescription)
            {
                Assert.AreEqual(expectedDescription, whenever.Describe());
                turnManager.AddWhenever(whenever);
            }
        }
        
        private TestContext GetEnemyAndPlayerTurnContext()
        {
            var player = new HealthCombatant(10);
            var enemy = new HealthCombatant(10);
            var combatants = new[] {player, enemy};
            var turnManager = new HealthWorld(combatants.ToList());
            var wheneverManager =
                new WheneverManager<IInspectWorldHealth, ICommandWorldHealth>(turnManager, turnManager);

            return new TestContext
            {
                player = player.id,
                enemy = enemy.id,
                turnManager = wheneverManager,
                inspector = turnManager,
                world = turnManager
            };
        }
        
        [Test]
        public void WhenPlayerTakesDamage__TakesDamage()
        {
            var ctx = GetEnemyAndPlayerTurnContext();
            var dmg = HealthExt.Commands.Factory.Damage(ctx.player, 2);
            var initiated = new InitiatedCommand<ICommandWorldHealth>(dmg, InitiatorFactory.FromNone("test"));
            Assert.AreEqual(10, ctx.inspector.GetHealth(ctx.player));
            ctx.turnManager.InitiateCommandBatch(new[] { initiated });
            Assert.AreEqual(8, ctx.inspector.GetHealth(ctx.player));
        }

        [Test]
        public void WheneverPlayerDealsDamage_ApplyDotToTarget__AppliedDot()
        {
            var ctx = GetEnemyAndPlayerTurnContext();
            
            ctx.AddWhenever(
                HealthExt.Filters.Factory.CreateDamageOccursFilter(2), 
                HealthExt.Effects.Factory.DotTarget(1, 3), 
                "whenever at least 2 damage occurs; apply 1 damage per turn for 3 turns to the target");
            
            Assert.AreEqual(10, ctx.inspector.GetHealth(ctx.enemy));
            
            var dmg = HealthExt.Commands.Factory.Damage(ctx.enemy, 2);
            var initiated = new InitiatedCommand<ICommandWorldHealth>(dmg, InitiatorFactory.From(ctx.player));
            ctx.turnManager.InitiateCommandBatch(new[] { initiated });
            
            Assert.AreEqual(8, ctx.inspector.GetHealth(ctx.enemy));

            ctx.turnManager.InitiateCommandBatch(ctx.world.ApplyAllStatusEffects());
            Assert.AreEqual(7, ctx.inspector.GetHealth(ctx.enemy));
            
            ctx.turnManager.InitiateCommandBatch(ctx.world.ApplyAllStatusEffects());
            Assert.AreEqual(6, ctx.inspector.GetHealth(ctx.enemy));
            
            ctx.turnManager.InitiateCommandBatch(ctx.world.ApplyAllStatusEffects());
            Assert.AreEqual(5, ctx.inspector.GetHealth(ctx.enemy));
            
            ctx.turnManager.InitiateCommandBatch(ctx.world.ApplyAllStatusEffects());
            Assert.AreEqual(5, ctx.inspector.GetHealth(ctx.enemy));
        }
        
        [Test]
        public void WheneverPlayerDealsDamage_DealsDamageToTarget_OneWhenever__DealsDamage_Once()
        {
            var ctx = GetEnemyAndPlayerTurnContext();
            ctx.AddWhenever(
                HealthExt.Filters.Factory.CreateDamageOccursFilter(1),
                HealthExt.Effects.Factory.DamageTarget(1), 
                "whenever at least 1 damage occurs; deal 1 damage to the target");
            
            Assert.AreEqual(10, ctx.inspector.GetHealth(ctx.enemy));
            
            var dmg = HealthExt.Commands.Factory.Damage(ctx.enemy, 2);
            var initiated = new InitiatedCommand<ICommandWorldHealth>(dmg, InitiatorFactory.From(ctx.player));
            ctx.turnManager.InitiateCommand(initiated);
            
            Assert.AreEqual(7, ctx.inspector.GetHealth(ctx.enemy));
        }
        [Test]
        public void WheneverPlayerDealsDamage_DealsDamageToTarget__DealsDamage_OncePerCommand_PerWhenever_InStages()
        {
            var ctx = GetEnemyAndPlayerTurnContext();
            var filter = HealthExt.Filters.Factory.CreateDamageOccursFilter(1);
            WheneverType damageOccursDealsDamageToTarget = new(filter, HealthExt.Effects.Factory.DamageTarget(1));
            ctx.AddWhenever(damageOccursDealsDamageToTarget, "whenever at least 1 damage occurs; deal 1 damage to the target");
            ctx.AddWhenever(damageOccursDealsDamageToTarget, "whenever at least 1 damage occurs; deal 1 damage to the target");
            
            Assert.AreEqual(10, ctx.inspector.GetHealth(ctx.enemy));
            
            var dmg = HealthExt.Commands.Factory.Damage(ctx.enemy, 2);
            var initiated = new InitiatedCommand<ICommandWorldHealth>(dmg, InitiatorFactory.From(ctx.player));
            ctx.turnManager.InitiateCommand(initiated);
            
            // <2dmg>
            // + <1dmg> / <*dmg> =  + <1dmg>
            // = <1dmg>, <2dmg>
            // + <1dmg> / <*dmg> =  + <1dmg>, <1dmg>
            // = <1dmg>, <1dmg>, <1dmg>, <2dmg>
            // = 5
            Assert.AreEqual(5, ctx.inspector.GetHealth(ctx.enemy));
        }

        [Test]
        public void WheneverPlayerDealsDamage_AndTargetHasAtLeastHealth__DealsMoreDamage()
        {
            var ctx = GetEnemyAndPlayerTurnContext();
            ctx.AddWhenever(
                HealthExt.Filters.Factory.Compose(
                    HealthExt.Filters.Factory.TargetHasAtLeastHealth(5),
                    HealthExt.Filters.Factory.CreateDamageOccursFilter(1)
                ),
                HealthExt.Effects.Factory.DamageTarget(2), 
                "whenever target has at least 5 health and at least 1 damage occurs; deal 2 damage to the target");
            
            Assert.AreEqual(10, ctx.inspector.GetHealth(ctx.enemy));
            
            var dmg = HealthExt.Commands.Factory.Damage(ctx.enemy, 2);
            var initiated = new InitiatedCommand<ICommandWorldHealth>(dmg, InitiatorFactory.From(ctx.player));
            ctx.turnManager.InitiateCommand(initiated);
            
            Assert.AreEqual(6, ctx.inspector.GetHealth(ctx.enemy));
        }
    }
    
}