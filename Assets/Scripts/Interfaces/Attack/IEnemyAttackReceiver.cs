using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttackReceiver {
	void ResetCombo();

	float GetSongPos();
	void DisplayDamage(int damage);
}
