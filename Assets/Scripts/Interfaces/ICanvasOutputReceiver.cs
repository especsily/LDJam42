using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanvasOutputReceiver {
	void DisplayResult(string result, Color color);
	void DisplayCombo(int combo);
	void DisplaySongTime(float time);
	void RemoveAllStackPanel();
}
