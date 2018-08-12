using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, IPlayerAttack
{
    public IHealth enemy;
    public IPlayerAttackReceiver attackReceiver;

    public void PlayerAttack()
    {
        attackReceiver.ResetManaBar();
        animator.SetTrigger("Attack");
    }
}
