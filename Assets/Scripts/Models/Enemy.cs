using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Enemy : Character, IPlayerAttackReceiver
{
    public IEnemyAttackReceiver gameLogic;
    [HideInInspector] public float AttackTime;
    private float manaTimer;

    public void ResetManaBar()
    {
        manaTimer = 0;
    }

    void Update()
    {
		//enemy attack
        if (gameLogic.GetSongPos() > 0 && !gameLogic.IsComplete())
        {
            manaTimer += Time.deltaTime;
            if (manaTimer >= AttackTime)
            {
                manaTimer = 0;
				animator.SetTrigger("Attack");
                otherCharacter.TakeDamage(Damage);
                gameController.SetDelayGenerator(true);

                DOTween.Complete(Camera.main);
                Camera.main.DOShakePosition(1f, 5f);
                // gameLogic.ResetCombo();
            }
            // manaTimerText.text = (int)(AttackTime - manaTimer) + "";
            canvasOutput.ShowEnemyMana(AttackTime - manaTimer);
        }
    }
}
