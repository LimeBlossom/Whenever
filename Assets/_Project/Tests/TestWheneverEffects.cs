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

    private (Combatant player, Combatant enemy, GlobalTurnManager turnManager) GetEnemyAndPlayerTurnContext()
    {
        var player = new Combatant(10, CombatantType.Player);
        var enemy = new Combatant(10, CombatantType.Enemy);
        var combatants = new Combatant[] {player, enemy};
        var wheneverManager = new WheneverManager();
        var turnManager = new GlobalTurnManager(wheneverManager, combatants.ToList());

        return (player, enemy, turnManager);
    }ApplyEffect    


    [Test]
    public void WhenPlayerTakesDamage__TakesDamage()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        /* Code that would be applied by an attack action */
        DamagePackage damagePackage = new();
        damagePackage.damageType = DamageType.FIRE;
        damagePackage.damageAmount = 1;
        damagePackage.attacker = enemy;
        damagePackage.target = player;
        

        /* Code that would be called by a turn manager */
        turnManager.StartEnemyTurn();
        turnManager.ApplyDamage(damagePackage);
        turnManager.StartPlayerTurn();
        
        Assert.AreEqual(9, player.health.GetCurrentHealth());
        Assert.AreEqual(10, enemy.health.GetCurrentHealth());
    }

    [Test]
    public void WhenPlayer_HasBurnOnBurnDamageHasBurnEffect__AppliesBurnDamage()
    {
        var (player, enemy, turnManager) = GetEnemyAndPlayerTurnContext();
        
        Whenever fireDamageAppliesBurn = new();
        fireDamageAppliesBurn.SetTrigger(DamageType.FIRE);
        fireDamageAppliesBurn.SetTarget(Target.Player);
        BurnAction burnAction = new();
        fireDamageAppliesBurn.effect += burnAction.Effect;
        turnManager.AddWhenever(fireDamageAppliesBurn);

        /* Code that would be applied by an attack action */
        DamagePackage damagePackage = new();
        damagePackage.damageType = DamageType.FIRE;
        damagePackage.damageAmount = 1;
        damagePackage.attacker = enemy;
        damagePackage.target = player;
        
        turnManager.StartEnemyTurn();
        turnManager.ApplyDamage(damagePackage);
        Assert.AreEqual(9, player.health.GetCurrentHealth());

        /* Code that would be called by a turn manager */
        turnManager.StartPlayerTurn();
        Assert.AreEqual(8, player.health.GetCurrentHealth());
        turnManager.StartPlayerTurn();
        Assert.AreEqual(7, player.health.GetCurrentHealth());
        turnManager.StartPlayerTurn();
        Assert.AreEqual(6, player.health.GetCurrentHealth());
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        turnManager.StartPlayerTurn();
        
        Assert.AreEqual(6, player.health.GetCurrentHealth());
        Assert.AreEqual(10, enemy.health.GetCurrentHealth());
    }
}