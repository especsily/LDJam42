using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character, IPlayerAttackReceiver  {
	public IHealth player;
	public IEnemyAttackReceiver gameLogic;
	[SerializeField] private float AttackTime;
	[SerializeField] private Image ManaBar;
	private float ManaTimer;

    public void ResetManaBar()
    {
		ManaTimer = 0;
    }

    void Update()
	{
		ManaTimer += Time.deltaTime;
		if(ManaTimer >= AttackTime)
		{
			ManaTimer = 0;
			player.TakeDamage(Damage);
			player.PlayHurtAnimation();
			gameLogic.ResetCombo();
		}
		ManaBar.fillAmount = (AttackTime - ManaTimer) / AttackTime; 
	}

}
