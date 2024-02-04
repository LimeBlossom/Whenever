using System.Linq;
using NUnit.Framework;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.Effects;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;
using WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter;

public class UnitTestExample
{
    [Test]
    public void MyTest()
    {
        Debug.Log("This runs inside");
        
        Assert.IsTrue(true);
        Assert.AreEqual("someThing", "someThing");
    }

    private (CombatantId player, CombatantId enemy, GlobalCombatWorld turnManager) GetEnemyAndPlayerTurnContext()
    {
        var player = new Combatant(10, CombatantType.Player);
        var enemy = new Combatant(10, CombatantType.Enemy);
        var combatants = new[] {player, enemy};
        var turnManager = new GlobalCombatWorld(combatants.ToList(), 197271);

        return (
            turnManager.GetOfType(CombatantType.Player).Single(),
            turnManager.GetOfType(CombatantType.Enemy).Single(), 
            turnManager);
    }


    [Test]
    public void WhenPlayerTakesDamage__TakesDamage()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        /* Code that would be called by a turn manager */
        turnManager.StartEnemyTurn();
        
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.FIRE, 1, player),
            InitiatorFactory.From(enemy));
        turnManager.StartPlayerTurn();


        var playerData = turnManager.CombatantData(player);
        var enemyData = turnManager.CombatantData(enemy);
        
        Assert.AreEqual(9,playerData.CurrentHealth());
        Assert.AreEqual(10, enemyData.CurrentHealth());
    }

    [Test]
    public void WhenPlayer_HasBurnOnBurnDamageHasBurnEffect__AppliesBurnDamage()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();

        var wheneverFilter = WheneverFilterFactory.CreateDealtDamageFilter(
                DamageType.FIRE,
                WheneverCombatantTypeFilter.Player);
        Whenever fireDamageAppliesBurn = new(wheneverFilter, EffectFactory.BurnTarget());
        turnManager.InitiateCommand(
            CmdFactory.Whenever(fireDamageAppliesBurn),
            InitiatorFactory.FromNone());

        /* Code that would be applied by an attack action */
        
        turnManager.StartEnemyTurn();
        
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.FIRE, 1, player),
            InitiatorFactory.From(enemy));
        var playerData = turnManager.CombatantData(player);
        Assert.AreEqual(9, playerData.CurrentHealth());

        /* Code that would be called by a turn manager */
        turnManager.StartPlayerTurn();
        Assert.AreEqual(8, playerData.CurrentHealth());
        turnManager.StartPlayerTurn();
        Assert.AreEqual(7, playerData.CurrentHealth());
        turnManager.StartPlayerTurn();
        Assert.AreEqual(6, playerData.CurrentHealth());
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        
        var enemyData = turnManager.CombatantData(enemy);
        Assert.AreEqual(6, playerData.CurrentHealth());
        Assert.AreEqual(10, enemyData.CurrentHealth());
    }

    [Test]
    public void WhenPlayer_DealsDamage_HealsSelf()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        // whenever the player deals damage, heal the player
        var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
            DamageType.PHYSICAL,
            WheneverCombatantTypeFilter.Player);
        var healPlayer = new Whenever(wheneverPlayerDealsPhysical, EffectFactory.HealInitiator(3));
        turnManager.InitiateCommand(CmdFactory.Whenever(healPlayer), InitiatorFactory.FromNone());
        
        // player takes damage
        turnManager.StartEnemyTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 4, player),
            InitiatorFactory.From(enemy));
        
        Assert.AreEqual(6, turnManager.CombatantData(player).CurrentHealth());
        Assert.AreEqual(10, turnManager.CombatantData(enemy).CurrentHealth());
        
        // player deals damage
        turnManager.StartPlayerTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 1, enemy),
            InitiatorFactory.From(player));
        
        Assert.AreEqual(9, turnManager.CombatantData(player).CurrentHealth());
        Assert.AreEqual(9, turnManager.CombatantData(enemy).CurrentHealth());
    }
    [Test]
    public void WhenPlayer_DealsDamage_HealsSelf_ToMaxHealth()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        // whenever the player deals damage, heal the player
        var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
            DamageType.PHYSICAL,
            WheneverCombatantTypeFilter.Player);
        var healPlayer = new Whenever(wheneverPlayerDealsPhysical, EffectFactory.HealInitiator(3));
        turnManager.InitiateCommand(CmdFactory.Whenever(healPlayer), InitiatorFactory.FromNone());
        
        // player takes damage
        turnManager.StartEnemyTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 1, player),
            InitiatorFactory.From(enemy));
        
        Assert.AreEqual(9, turnManager.CombatantData(player).CurrentHealth());
        Assert.AreEqual(10, turnManager.CombatantData(enemy).CurrentHealth());
        
        // player deals damage
        turnManager.StartPlayerTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 1, enemy),
            InitiatorFactory.From(player));
        
        Assert.AreEqual(10, turnManager.CombatantData(player).CurrentHealth());
        Assert.AreEqual(9, turnManager.CombatantData(enemy).CurrentHealth());
    }
    
    [Test]
    public void When_FireDamageTaken_RandomBoulderThrown()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        // whenever anyone takes fire damage, throw a random boulder
        var wheneverFireDamageTaken = WheneverFilterFactory.CreateDealtDamageFilter(
            DamageType.FIRE,
            WheneverCombatantTypeFilter.Any);
        var randomMeteor = new Whenever(wheneverFireDamageTaken, EffectFactory.RandomBoulder(2));
        turnManager.InitiateCommand(CmdFactory.Whenever(randomMeteor), InitiatorFactory.FromNone());
        
        // player deals fire damage, triggering a random meteor
        turnManager.StartPlayerTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.FIRE, 1, enemy),
            InitiatorFactory.From(player));
        
        Assert.AreEqual(8, turnManager.CombatantData(player).CurrentHealth());
        Assert.AreEqual(9, turnManager.CombatantData(enemy).CurrentHealth());
    }

    [Test]
    public void When_PlayerDealsPhysicalCritAndBleed_ThenHealsFromBleed__HealsProperly()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        var wheneverPhysicalDamageTakenByEnemy = WheneverFilterFactory.CreateDealtDamageFilter(
            DamageType.PHYSICAL,
            WheneverCombatantTypeFilter.Enemy);
        var appliesCriticalDamage = new Whenever(wheneverPhysicalDamageTakenByEnemy, EffectFactory.CriticalDamage());
        turnManager.InitiateCommand(CmdFactory.Whenever(appliesCriticalDamage), InitiatorFactory.FromNone());
        
        var wheneverCriticalDamageTakenByEnemy = WheneverFilterFactory.CreateDealtDamageFilter(
            DamageType.CRITICAL,
            WheneverCombatantTypeFilter.Enemy);
        var appliesBleedStatus = new Whenever(wheneverCriticalDamageTakenByEnemy, EffectFactory.BleedTarget(1, 3));
        turnManager.InitiateCommand(CmdFactory.Whenever(appliesBleedStatus), InitiatorFactory.FromNone());
        
        var wheneverPlayerDealsBleed = WheneverFilterFactory.CreateDealsDamageFilter(
            DamageType.BLEED,
            WheneverCombatantTypeFilter.Player);
        var appliesHeal = new Whenever(wheneverPlayerDealsBleed, EffectFactory.HealInitiator(1));
        turnManager.InitiateCommand(CmdFactory.Whenever(appliesHeal), InitiatorFactory.FromNone());
        
        var playerData = turnManager.CombatantData(player);
        var enemyData = turnManager.CombatantData(enemy);
        turnManager.StartEnemyTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 5, player),
            InitiatorFactory.From(enemy));
        
        Assert.AreEqual(5, playerData.CurrentHealth());
        Assert.AreEqual(10, enemyData.CurrentHealth());
        
        turnManager.StartPlayerTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 1, enemy),
            InitiatorFactory.From(player));
        
        Assert.AreEqual(5, playerData.CurrentHealth());
        Assert.AreEqual(8, enemyData.CurrentHealth());
        
        turnManager.StartEnemyTurn();
        turnManager.StartPlayerTurn();
        Assert.AreEqual(6, playerData.CurrentHealth());
        Assert.AreEqual(7, enemyData.CurrentHealth());

        turnManager.StartEnemyTurn();
        turnManager.StartPlayerTurn();
        Assert.AreEqual(7, playerData.CurrentHealth());
        Assert.AreEqual(6, enemyData.CurrentHealth());
        
        turnManager.StartEnemyTurn();
        turnManager.StartPlayerTurn();
        Assert.AreEqual(8, playerData.CurrentHealth());
        Assert.AreEqual(5, enemyData.CurrentHealth());
        
        turnManager.StartEnemyTurn();
        turnManager.StartPlayerTurn();
        Assert.AreEqual(8, playerData.CurrentHealth());
        Assert.AreEqual(5, enemyData.CurrentHealth());
    }

    [Test]
    public void When_PlayerAppliesBleedToEnemy_TakesPhysicalDamage()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        var wheneverPhysicalDamageTakenByEnemy = WheneverFilterFactory.CreateDealtDamageFilter(
            DamageType.PHYSICAL,
            WheneverCombatantTypeFilter.Enemy);
        var appliesBleedStatus = new Whenever(wheneverPhysicalDamageTakenByEnemy, EffectFactory.BleedTarget(1, 3));
        turnManager.InitiateCommand(CmdFactory.Whenever(appliesBleedStatus), InitiatorFactory.FromNone());
        
        var wheneverBleedInflictedOnEnemy = WheneverFilterFactory.CreateStatusEffectInflictedFilter(
            typeof(BleedStatus),
            WheneverCombatantTypeFilter.Enemy);
        var appliesPhysicalDamageToInitiator = new Whenever(wheneverBleedInflictedOnEnemy, EffectFactory.DamageInitiator(DamageType.PHYSICAL, 1));
        turnManager.InitiateCommand(CmdFactory.Whenever(appliesPhysicalDamageToInitiator), InitiatorFactory.FromNone());
        
        var playerData = turnManager.CombatantData(player);
        var enemyData = turnManager.CombatantData(enemy);
        
        turnManager.StartPlayerTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 1, enemy),
            InitiatorFactory.From(player));
        Assert.AreEqual(9, playerData.CurrentHealth());
        Assert.AreEqual(9, enemyData.CurrentHealth());
        
        turnManager.StartEnemyTurn();
        turnManager.StartPlayerTurn();
        Assert.AreEqual(9, playerData.CurrentHealth());
        Assert.AreEqual(8, enemyData.CurrentHealth());
        
        turnManager.StartEnemyTurn();
        turnManager.StartPlayerTurn();
        Assert.AreEqual(9, playerData.CurrentHealth());
        Assert.AreEqual(7, enemyData.CurrentHealth());
        
        turnManager.StartEnemyTurn();
        turnManager.StartPlayerTurn();
        Assert.AreEqual(9, playerData.CurrentHealth());
        Assert.AreEqual(6, enemyData.CurrentHealth());
        
        turnManager.StartEnemyTurn();
        turnManager.StartPlayerTurn();
        Assert.AreEqual(9, playerData.CurrentHealth());
        Assert.AreEqual(6, enemyData.CurrentHealth());
    }
    
    [Test]
    public void When_PhysicalDamageOnEnemy_TriggerPhysicalDamageOnEnemy_OnlyAppliesOnce()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        var wheneverPhysicalDamageTakenByEnemy = WheneverFilterFactory.CreateDealtDamageFilter(
            DamageType.PHYSICAL,
            WheneverCombatantTypeFilter.Enemy);
        var appliesPhysicalDamage = new Whenever(wheneverPhysicalDamageTakenByEnemy, EffectFactory.DamageTarget(DamageType.PHYSICAL, 1));
        turnManager.InitiateCommand(CmdFactory.Whenever(appliesPhysicalDamage), InitiatorFactory.FromNone());
        
        var playerData = turnManager.CombatantData(player);
        var enemyData = turnManager.CombatantData(enemy);
        
        turnManager.StartPlayerTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 1, enemy),
            InitiatorFactory.From(player));
        
        Assert.AreEqual(10, playerData.CurrentHealth());
        Assert.AreEqual(8, enemyData.CurrentHealth());
        
        turnManager.StartEnemyTurn();
        turnManager.StartPlayerTurn();
        Assert.AreEqual(10, playerData.CurrentHealth());
        Assert.AreEqual(8, enemyData.CurrentHealth());
    }
    
    [Test]
    public void When_PlayerDealsPhysicalDamage_AdjacentEnemiesTakePhysicalDamage(){
        GlobalCombatWorld GenerateWorld()
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
        
            return new GlobalCombatWorld(new[] {playerData}.Concat(enemies).ToList(), 197271);   
        }
        var turnManager = GenerateWorld();
        float GetHealthOf(float x, float y) => turnManager.CombatantData(turnManager.GetAtLocation(new (x, y))).CurrentHealth();
        
        var playerId = turnManager.GetAtLocation(new(0, 0));
        
        var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
            DamageType.PHYSICAL,
            WheneverCombatantTypeFilter.Player);
        var appliesPhysicalDamage = new Whenever(wheneverPlayerDealsPhysical, EffectFactory.DamageAdjacentTargets(DamageType.PHYSICAL, 1));
        turnManager.InitiateCommand(CmdFactory.Whenever(appliesPhysicalDamage), InitiatorFactory.FromNone());
        
        turnManager.StartPlayerTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 1, turnManager.GetAtLocation(new (3, 3))),
            InitiatorFactory.From(playerId));
        
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
        GlobalCombatWorld GenerateWorld()
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
        
            return new GlobalCombatWorld(new[] {playerData}.Concat(enemies).ToList(), 197271);   
        }
        var turnManager = GenerateWorld();
        float GetHealthOf(float x, float y) => turnManager.CombatantData(turnManager.GetAtLocation(new (x, y))).CurrentHealth();
        float GetHealthOfId(CombatantId id) => turnManager.CombatantData(id).CurrentHealth();
        
        var playerId = turnManager.GetAtLocation(new(0, 0));
        
        var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
            DamageType.PHYSICAL,
            WheneverCombatantTypeFilter.Player);
        var appliesDamageToAdjacents = EffectFactory.DamageAdjacentTargets(DamageType.PHYSICAL, 1);
        turnManager.InitiateCommand(CmdFactory.Whenever(new Whenever(wheneverPlayerDealsPhysical, appliesDamageToAdjacents)), InitiatorFactory.FromNone());
        
        var healsInitiator = EffectFactory.HealInitiator(1);
        turnManager.InitiateCommand(CmdFactory.Whenever(new Whenever(wheneverPlayerDealsPhysical, healsInitiator)), InitiatorFactory.FromNone());
        
        turnManager.StartEnemyTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 8, playerId),
            InitiatorFactory.From(turnManager.GetAtLocation(new (3, 3))));
        Assert.AreEqual(2, GetHealthOfId(playerId));
        
        turnManager.StartPlayerTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 1, turnManager.GetAtLocation(new (3, 3))),
            InitiatorFactory.From(playerId));
        
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
        GlobalCombatWorld GenerateWorld()
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
        
            return new GlobalCombatWorld(new[] {playerData}.Concat(enemies).ToList(), 197271);   
        }
        var turnManager = GenerateWorld();
        float GetHealthOf(float x, float y) => turnManager.CombatantData(turnManager.GetAtLocation(new (x, y))).CurrentHealth();
        float GetHealthOfId(CombatantId id) => turnManager.CombatantData(id).CurrentHealth();
        
        var playerId = turnManager.GetAtLocation(new(0, 0));
        
        var wheneverPlayerDealsPhysical = WheneverFilterFactory.CreateDealsDamageFilter(
            DamageType.PHYSICAL,
            WheneverCombatantTypeFilter.Player);
        var healsInitiator = EffectFactory.HealInitiator(1);
        turnManager.InitiateCommand(CmdFactory.Whenever(new Whenever(wheneverPlayerDealsPhysical, healsInitiator)), InitiatorFactory.FromNone());
        
        var appliesDamageToAdjacents = EffectFactory.DamageAdjacentTargets(DamageType.PHYSICAL, 1);
        turnManager.InitiateCommand(CmdFactory.Whenever(new Whenever(wheneverPlayerDealsPhysical, appliesDamageToAdjacents)), InitiatorFactory.FromNone());
        
        turnManager.StartEnemyTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 8, playerId),
            InitiatorFactory.From(turnManager.GetAtLocation(new (3, 3))));
        Assert.AreEqual(2, GetHealthOfId(playerId));
        
        turnManager.StartPlayerTurn();
        turnManager.InitiateCommand(
            CmdFactory.Damage(DamageType.PHYSICAL, 1, turnManager.GetAtLocation(new (3, 3))),
            InitiatorFactory.From(playerId));
        
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