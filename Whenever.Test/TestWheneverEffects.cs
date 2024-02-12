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
    
    public class UnitTestExample
    {
        [Test]
        public void MyTest()
        {
            Debug.Log("This runs inside");
        
            Assert.IsTrue(true);
            Assert.AreEqual("someThing", "someThing");
        }

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
        public void WhenPlayerTakesDamage__TakesDamage()
        {
            var (player, enemy, turnManager, baseWorld) = GetEnemyAndPlayerTurnContext();
        
            /* Code that would be called by a turn manager */
        
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.FIRE, 1, player),
                Initiators.From(enemy));


            var playerData = baseWorld.CombatantData(player);
            var enemyData = baseWorld.CombatantData(enemy);
        
            Assert.AreEqual(9,playerData.CurrentHealth());
            Assert.AreEqual(10, enemyData.CurrentHealth());
        }

        [Test]
        public void WhenPlayer_DealsDamage_HealsSelf()
        {
            var (player, enemy, turnManager, baseWorld) = GetEnemyAndPlayerTurnContext();
        
            // whenever the player deals damage, heal the player
            var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
                DamageType.PHYSICAL,
                WheneverCombatantTypeFilter.Player);
            var healPlayer = new WheneverType(wheneverPlayerDealsPhysical, EffectFactory.HealInitiator(3));
            turnManager.AddWhenever(healPlayer);
        
            // player takes damage
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 4, player),
                Initiators.From(enemy));
        
            Assert.AreEqual(6, baseWorld.CombatantData(player).CurrentHealth());
            Assert.AreEqual(10, baseWorld.CombatantData(enemy).CurrentHealth());
        
            // player deals damage
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 1, enemy),
                Initiators.From(player));
        
            Assert.AreEqual(9, baseWorld.CombatantData(player).CurrentHealth());
            Assert.AreEqual(9, baseWorld.CombatantData(enemy).CurrentHealth());
        }
        [Test]
        public void WhenPlayer_DealsDamage_HealsSelf_ToMaxHealth()
        {
            var (player, enemy, turnManager, baseWorld) = GetEnemyAndPlayerTurnContext();
        
            // whenever the player deals damage, heal the player
            var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
                DamageType.PHYSICAL,
                WheneverCombatantTypeFilter.Player);
            var healPlayer = new WheneverType(wheneverPlayerDealsPhysical, EffectFactory.HealInitiator(3));
            turnManager.AddWhenever(healPlayer);
        
            // player takes damage
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 1, player),
                Initiators.From(enemy));
        
            Assert.AreEqual(9, baseWorld.CombatantData(player).CurrentHealth());
            Assert.AreEqual(10, baseWorld.CombatantData(enemy).CurrentHealth());
        
            // player deals damage
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 1, enemy),
                Initiators.From(player));
        
            Assert.AreEqual(10, baseWorld.CombatantData(player).CurrentHealth());
            Assert.AreEqual(9, baseWorld.CombatantData(enemy).CurrentHealth());
        }
    
        [Test]
        public void When_FireDamageTaken_RandomBoulderThrown()
        {
            var (player, enemy, turnManager, baseWorld) = GetEnemyAndPlayerTurnContext();
        
            // whenever anyone takes fire damage, throw a random boulder
            var wheneverFireDamageTaken = WheneverFilterFactory.CreateDealtDamageFilter(
                DamageType.FIRE,
                WheneverCombatantTypeFilter.Any);
            var randomMeteor = new WheneverType(wheneverFireDamageTaken, EffectFactory.RandomBoulder(2));
            turnManager.AddWhenever(randomMeteor);
        
            // player deals fire damage, triggering a random meteor
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.FIRE, 1, enemy),
                Initiators.From(player));
        
            Assert.AreEqual(8, baseWorld.CombatantData(player).CurrentHealth());
            Assert.AreEqual(9, baseWorld.CombatantData(enemy).CurrentHealth());
        }

        [Test]
        public void When_PhysicalDamageOnEnemy_TriggerPhysicalDamageOnEnemy_OnlyAppliesOnce()
        {
            var (player, enemy, turnManager, baseWorld) = GetEnemyAndPlayerTurnContext();
        
            var wheneverPhysicalDamageTakenByEnemy = WheneverFilterFactory.CreateDealtDamageFilter(
                DamageType.PHYSICAL,
                WheneverCombatantTypeFilter.Enemy);
            var appliesPhysicalDamage = new WheneverType(wheneverPhysicalDamageTakenByEnemy, EffectFactory.DamageTarget(DamageType.PHYSICAL, 1));
            turnManager.AddWhenever(appliesPhysicalDamage);
        
            var playerData = baseWorld.CombatantData(player);
            var enemyData = baseWorld.CombatantData(enemy);
        
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 1, enemy),
                Initiators.From(player));
        
            Assert.AreEqual(10, playerData.CurrentHealth());
            Assert.AreEqual(8, enemyData.CurrentHealth());
        
            Assert.AreEqual(10, playerData.CurrentHealth());
            Assert.AreEqual(8, enemyData.CurrentHealth());
        }
    
        [Test]
        public void When_PlayerDealsPhysicalDamage_AdjacentEnemiesTakePhysicalDamage(){
            GlobalCombatWorldDemo GenerateWorld()
            {
                var playerData = new Combatant(10, CombatantType.Player, new (0, 0));
                var enemies = new Vector2[]
                {
                    new(3, 3),
                    new(3, 4),
                    new(4, 3),
                    new(2, 3),

                    new(1, 1),
                    new(2, 2),
                    new(4, 2),
                }.Select(x => new Combatant(10, CombatantType.Enemy, x)).ToArray();
        
                return new GlobalCombatWorldDemo(new[] {playerData}.Concat(enemies).ToList(), 197271);   
            }
            var baseWorld = GenerateWorld();
            var turnManager = new WheneverManager<IInspectableWorldDemo, ICommandableWorldDemo>(baseWorld, baseWorld);
            float GetHealthOf(float x, float y) => baseWorld.CombatantData(baseWorld.GetAtLocation(new (x, y))).CurrentHealth();
        
            var playerId = baseWorld.GetAtLocation(new(0, 0));
        
            var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
                DamageType.PHYSICAL,
                WheneverCombatantTypeFilter.Player);
            var appliesPhysicalDamage = new WheneverType(wheneverPlayerDealsPhysical, EffectFactory.DamageAdjacentTargets(DamageType.PHYSICAL, 1));
            turnManager.AddWhenever(appliesPhysicalDamage);
        
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 1, baseWorld.GetAtLocation(new (3, 3))),
                Initiators.From(playerId));
        
            Assert.AreEqual(9, GetHealthOf(3, 3));
            Assert.AreEqual(9, GetHealthOf(3, 4));
            Assert.AreEqual(9, GetHealthOf(4, 3));
            Assert.AreEqual(9, GetHealthOf(2, 3));
        
            Assert.AreEqual(10, GetHealthOf(1, 1));
            Assert.AreEqual(10, GetHealthOf(2, 2));
            Assert.AreEqual(10, GetHealthOf(4, 2));
        
            Assert.AreEqual(10, GetHealthOf(0, 0));
        
        }

        [Test]
        public void When_PlayerDealsPhysicalDamage_AdjacentEnemiesTakePhysicalDamage_AndTriggersHealPlayer(){
            GlobalCombatWorldDemo GenerateWorld()
            {
                var playerData = new Combatant(10, CombatantType.Player, new (0, 0));
                var enemies = new Vector2[]
                {
                    new(3, 3),
                    new(3, 4),
                    new(4, 3),
                    new(2, 3),

                    new(1, 1),
                    new(2, 2),
                    new(4, 2),
                }.Select(x => new Combatant(10, CombatantType.Enemy, x)).ToArray();
        
                return new GlobalCombatWorldDemo(new[] {playerData}.Concat(enemies).ToList(), 197271);   
            }
            var baseWorld = GenerateWorld();
            var turnManager = new WheneverManager<IInspectableWorldDemo, ICommandableWorldDemo>(baseWorld, baseWorld);
            float GetHealthOf(float x, float y) => baseWorld.CombatantData(baseWorld.GetAtLocation(new (x, y))).CurrentHealth();
            float GetHealthOfId(CombatantId id) => baseWorld.CombatantData(id).CurrentHealth();
        
            var playerId = baseWorld.GetAtLocation(new(0, 0));
        
            var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
                DamageType.PHYSICAL,
                WheneverCombatantTypeFilter.Player);
            var appliesDamageToAdjacents = EffectFactory.DamageAdjacentTargets(DamageType.PHYSICAL, 1);
            turnManager.AddWhenever(new WheneverType(wheneverPlayerDealsPhysical, appliesDamageToAdjacents));
        
            var healsInitiator = EffectFactory.HealInitiator(1);
            turnManager.AddWhenever(new WheneverType(wheneverPlayerDealsPhysical, healsInitiator));
        
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 8, playerId),
                Initiators.From(baseWorld.GetAtLocation(new (3, 3))));
            Assert.AreEqual(2, GetHealthOfId(playerId));
        
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 1, baseWorld.GetAtLocation(new (3, 3))),
                Initiators.From(playerId));
        
            // the healing whenever trigger occurs after the damage adjacents, so the player deals damage to 4 total:
            // the original enemy, and the enemies hit by the adjacent factor. healing them for (3 adjacent + 1 direct) = 4
            Assert.AreEqual(6, GetHealthOfId(playerId));
        
            Assert.AreEqual(9, GetHealthOf(3, 3));
            Assert.AreEqual(9, GetHealthOf(3, 4));
            Assert.AreEqual(9, GetHealthOf(4, 3));
            Assert.AreEqual(9, GetHealthOf(2, 3));
        
            Assert.AreEqual(10, GetHealthOf(1, 1));
            Assert.AreEqual(10, GetHealthOf(2, 2));
            Assert.AreEqual(10, GetHealthOf(4, 2));
        }
        [Test]
        public void When_PlayerDealsPhysicalDamage_AdjacentEnemiesTakePhysicalDamage_AndTriggersHealPlayer__Inverted_Is_Worse(){
            GlobalCombatWorldDemo GenerateWorld()
            {
                var playerData = new Combatant(10, CombatantType.Player, new (0, 0));
                var enemies = new Vector2[]
                {
                    new(3, 3),
                    new(3, 4),
                    new(4, 3),
                    new(2, 3),

                    new(1, 1),
                    new(2, 2),
                    new(4, 2),
                }.Select(x => new Combatant(10, CombatantType.Enemy, x)).ToArray();
        
                return new GlobalCombatWorldDemo(new[] {playerData}.Concat(enemies).ToList(), 197271);   
            }
            var baseWorld = GenerateWorld();
            var turnManager = new WheneverManager<IInspectableWorldDemo, ICommandableWorldDemo>(baseWorld, baseWorld);
            float GetHealthOf(float x, float y) => baseWorld.CombatantData(baseWorld.GetAtLocation(new (x, y))).CurrentHealth();
            float GetHealthOfId(CombatantId id) => baseWorld.CombatantData(id).CurrentHealth();
        
            var playerId = baseWorld.GetAtLocation(new(0, 0));
        
            var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
                DamageType.PHYSICAL,
                WheneverCombatantTypeFilter.Player);
            var healsInitiator = EffectFactory.HealInitiator(1);
            turnManager.AddWhenever(new WheneverType(wheneverPlayerDealsPhysical, healsInitiator));
        
            var appliesDamageToAdjacents = EffectFactory.DamageAdjacentTargets(DamageType.PHYSICAL, 1);
            turnManager.AddWhenever(new WheneverType(wheneverPlayerDealsPhysical, appliesDamageToAdjacents));
        
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 8, playerId),
                Initiators.From(baseWorld.GetAtLocation(new (3, 3))));
            Assert.AreEqual(2, GetHealthOfId(playerId));
        
            turnManager.InitiateCommand(
                CmdFactory.Damage(DamageType.PHYSICAL, 1, baseWorld.GetAtLocation(new (3, 3))),
                Initiators.From(playerId));
        
            // the healing whenever trigger occurs before the damage adjacents, so the player deals damage to 1 total at this point:
            // the original enemy alone
            Assert.AreEqual(3, GetHealthOfId(playerId));
        
            Assert.AreEqual(9, GetHealthOf(3, 3));
            Assert.AreEqual(9, GetHealthOf(3, 4));
            Assert.AreEqual(9, GetHealthOf(4, 3));
            Assert.AreEqual(9, GetHealthOf(2, 3));
        
            Assert.AreEqual(10, GetHealthOf(1, 1));
            Assert.AreEqual(10, GetHealthOf(2, 2));
            Assert.AreEqual(10, GetHealthOf(4, 2));
        }
    }
}