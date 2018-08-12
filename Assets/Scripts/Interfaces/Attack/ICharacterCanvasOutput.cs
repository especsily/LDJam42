using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterCanvasOutput {
	float ShowCharacterImage(Sprite CharacterSprite);
	void ShowEnemyMana(float manaTimer);
	void ShowEnemyHealth(float currentHP);
	void ShowPlayerHealth(float currentHP);
}
