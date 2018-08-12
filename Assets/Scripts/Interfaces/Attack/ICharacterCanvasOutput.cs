using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterCanvasOutput {
	void ShowEnemyMana(float manaTimer);
	void ShowEnemyHealth(float currentHP);
	void ShowPlayerHealth(float currentHP);
}
