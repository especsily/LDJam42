using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanvasInfo {
	float GetHalfInputPanelWidth();
	RectTransform GetInputTransform();
	RectTransform GetStackTransform();
}
