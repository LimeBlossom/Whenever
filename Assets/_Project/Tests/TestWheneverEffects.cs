using System.Linq;
using NUnit.Framework;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.Effects;
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
        
        var wheneverCriticalDamageTakenByEnemy = WheneverFilterFactory.CreateDealsDamageFilter(
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
}