using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuReceiver {
	IEnumerator DisplayLosePanel(int dealedDamage, float bossHP);

	IEnumerator DisplayWinPanel();
}
