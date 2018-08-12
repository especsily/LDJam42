using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Enemy : Character, IPlayerAttackReceiver
{
    public IHealth player;
    public IEnemyAttackReceiver gameLogic;
    [SerializeField] private float AttackTime;
    private float manaTimer;

    public void ResetManaBar()
    {
        manaTimer = 0;
    }

    void Update()
    {
		//enemy attack
        if (gameLogic.GetSongPos() > 0)
        {
            manaTimer += Time.deltaTime;
            if (manaTimer >= AttackTime)
            {
                manaTimer = 0;
				animator.SetTrigger("Attack");
                player.TakeDamage(Damage);
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
