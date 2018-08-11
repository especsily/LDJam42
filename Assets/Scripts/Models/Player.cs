using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, IPlayerAttack
{
    public IHealth enemy;
    public IPlayerAttackReceiver attackReceiver;

    public void AttackEnemy(int damage)
    {
        enemy.TakeDamage(damage);
        enemy.PlayHurtAnimation();
        attackReceiver.ResetManaBar();
    }
}
