using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanvasInfo {
	float GetHalfInputPanelWidth();
	RectTransform GetComingPanelTransform();
	RectTransform GetStackTransform();
	float GetSpaceBarWidth();
	bool isFinishAnimation();
}
