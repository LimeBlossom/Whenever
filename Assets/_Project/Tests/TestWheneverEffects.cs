using System.Linq;
using _Project.WheneverAbstractions;
using NUnit.Framework;
using UnityEngine;

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
        var turnManager = new GlobalCombatWorld(combatants.ToList());

        return (
            turnManager.GetPlayers().Single(),
            turnManager.GetEnemies().Single(), 
            turnManager);
    }    


    [Test]
    public void WhenPlayerTakesDamage__TakesDamage()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        /* Code that would be applied by an attack action */
        var damageCommand = new DamageCommand()
        {
            damagePackage = new(DamageType.FIRE, 1),
            Target = player
        };

        var initiator = new CombatantCommandInitiator()
        {
            Initiator = enemy
        };
        

        /* Code that would be called by a turn manager */
        turnManager.StartEnemyTurn();
        turnManager.InitiateCommand(damageCommand,initiator);
        turnManager.StartPlayerTurn();


        var playerData = turnManager.GetCombatantData(player);
        var enemyData = turnManager.GetCombatantData(enemy);
        
        Assert.AreEqual(9,playerData.GetCurrentHealth());
        Assert.AreEqual(10, enemyData.GetCurrentHealth());
    }

    [Test]
    public void WhenPlayer_HasBurnOnBurnDamageHasBurnEffect__AppliesBurnDamage()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();

        var wheneverFilter = new WheneverDamageFilter()
        {
            trigger = DamageType.FIRE,
            targetType = Target.Player
        };
        Whenever fireDamageAppliesBurn = new(wheneverFilter, new BurnAction());
        turnManager.AddWhenever(fireDamageAppliesBurn);

        /* Code that would be applied by an attack action */
        var damageCommand = CommandFactory.Damage(DamageType.FIRE, 1, player);
        var initiator = CommandInitiatorFactory.From(enemy);
        
        turnManager.StartEnemyTurn();
        turnManager.InitiateCommand(damageCommand, initiator);
        var playerData = turnManager.GetCombatantData(player);
        Assert.AreEqual(9, playerData.GetCurrentHealth());

        /* Code that would be called by a turn manager */
        turnManager.StartPlayerTurn();
        Assert.AreEqual(8, playerData.GetCurrentHealth());
        turnManager.StartPlayerTurn();
        Assert.AreEqual(7, playerData.GetCurrentHealth());
        turnManager.StartPlayerTurn();
        Assert.AreEqual(6, playerData.GetCurrentHealth());
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        
        var enemyData = turnManager.GetCombatantData(enemy);
        Assert.AreEqual(6, playerData.GetCurrentHealth());
        Assert.AreEqual(10, enemyData.GetCurrentHealth());
    }

    [Test]
    public void When_PlayerDealsDamage__SpawnMeteorOnEnemy()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
    }
}