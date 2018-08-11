using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanvasOutputReceiver {
	void DisplayResult(string result, Color color, int comboStack, int damage);
	void DisplayCombo(int combo);
	void DisplaySongTime(float time);
	void RemoveAllStackPanel();
	void RemoveComingPanel();
	void MoveBall(float speed);
	void DisplaySpaceLeft(bool firstCreate, int spaceLeft);
	void DisplayRunningOut();
}
