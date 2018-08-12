using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuReceiver {
	void DisplayLosePanel(int dealedDamage, float bossHP);
}
