using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanvasOutputReceiver {
	void DisplayInputs(List<Vector2Int> currentInputs);
	void DisplaySuggestCombo(List<Vector2Int> availableCombos);
	void DisplayFinish(string result, ComboConfig combo = null);
}
