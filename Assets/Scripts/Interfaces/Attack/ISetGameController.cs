using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetGameController {
	void SetDelayGenerator(bool isDelay);
	GameObject GetPlayerAttackEffect(int damage);
	GameObject GetEnemyAttackEffect(); 
}
