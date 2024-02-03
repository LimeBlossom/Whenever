using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    /* Certain combinations are quite thematic, such as vampirism.
     * "Whenever physical damage is caused, heal for damage."
     */
    [SerializeField] private Combatant player;
    [SerializeField] private Combatant enemy;

    void Start()
    {
        player.health.Reduce(5);
        //PlayerFireAttack();
        PlayerPhysicalAttack();

        /* Code that would be called by a turn manager */
        enemy.StartTurn();
        player.StartTurn();
        enemy.StartTurn();
        player.StartTurn();
        enemy.StartTurn();
    }

    private void PlayerFireAttack()
    {
        /* Code that would be applied by card or default state */
        Whenever fireDamageAppliesBurn = new();
        fireDamageAppliesBurn.SetTrigger(DamageType.FIRE);
        fireDamageAppliesBurn.SetTarget(Target.TARGET);
        fireDamageAppliesBurn.effectName = "Burn";
        BurnAction burnAction = new();
        fireDamageAppliesBurn.effect += burnAction.Effect;
        player.wheneverManager.AddWhenever(fireDamageAppliesBurn);

        /* Code that would be applied by an attack action */
        DamagePackage damagePackage = new();
        damagePackage.damageType = DamageType.FIRE;
        damagePackage.damageAmount = 1;
        damagePackage.attacker = player;
        damagePackage.target = enemy;
        enemy.damageable.TakeDamage(ref damagePackage);
    }

    private void PlayerPhysicalAttack()
    {
        /* Code that would be applied by card or default state */
        Whenever physicalDamageChanceAppliesCritical = new();
        physicalDamageChanceAppliesCritical.SetTrigger(DamageType.PHYSICAL);
        physicalDamageChanceAppliesCritical.SetTarget(Target.TARGET);
        physicalDamageChanceAppliesCritical.effectName = "Critical";
        CriticalAction critAction = new();
        physicalDamageChanceAppliesCritical.effect += critAction.Effect;
        player.wheneverManager.AddWhenever(physicalDamageChanceAppliesCritical);

        Whenever criticalDamageAppliesBleed = new();
        criticalDamageAppliesBleed.SetTrigger(DamageType.CRITICAL);
        criticalDamageAppliesBleed.SetTarget(Target.TARGET);
        criticalDamageAppliesBleed.effectName = "Bleed";
        BleedAction bleedAction = new();
        criticalDamageAppliesBleed.effect += bleedAction.Effect;
        player.wheneverManager.AddWhenever(criticalDamageAppliesBleed);

        Whenever bleedDamageAppliesHeal = new();
        bleedDamageAppliesHeal.SetTrigger(DamageType.BLEED);
        bleedDamageAppliesHeal.SetTarget(Target.ATTACKER);
        bleedDamageAppliesHeal.effectName = "Heal";
        HealAction healAction = new();
        healAction.healAmount = 1;
        bleedDamageAppliesHeal.effect += healAction.Effect;
        player.wheneverManager.AddWhenever(bleedDamageAppliesHeal);

        /* Code that would be applied by an attack action */
        DamagePackage damagePackage = new();
        damagePackage.damageType = DamageType.PHYSICAL;
        damagePackage.damageAmount = 1;
        damagePackage.attacker = player;
        damagePackage.target = enemy;
        enemy.damageable.TakeDamage(ref damagePackage);
    }
}
