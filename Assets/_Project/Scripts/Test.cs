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
        /* Code that would be applied by card or default state */
        Whenever fireDamageAppliesBurn = new();
        fireDamageAppliesBurn.SetTrigger(DamageType.FIRE);
        BurnAction burnAction = new();
        fireDamageAppliesBurn.effect += burnAction.Effect;
        player.wheneverManager.AddWhenever(fireDamageAppliesBurn);

        /* Code that would be applied by an attack action */
        DamagePackage damagePackage = new();
        damagePackage.damageType = DamageType.FIRE;
        damagePackage.damageAmount = 1;
        damagePackage.attacker = enemy;
        damagePackage.target = player;
        player.damageable.TakeDamage(ref damagePackage);
        player.wheneverManager.CheckWhenevers(damagePackage, player);
        enemy.wheneverManager.CheckWhenevers(damagePackage, enemy);

        /* Code that would be called by a turn manager */
        player.StartTurn();
        player.StartTurn();
        player.StartTurn();
        player.StartTurn();
        player.StartTurn();
    }
}
