using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
	[SerializeField] private InputController inputController;
	[SerializeField] private AudioOutputController audioOutputController;
	[SerializeField] private CanvasOutputController canvasOutputController;
	[SerializeField] private HeartRateMonitor heartRateMonitor;
	[SerializeField] private GameLogic gameController;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		BindInput();
		BindOutput();
	}

	void BindInput()
	{
		inputController.inputReceiver = gameController;
	}

	void BindOutput()
	{
		gameController.audioInfo = audioOutputController;
		gameController.audioOutputReceiver = audioOutputController;
		gameController.canvasOutputReceiver = canvasOutputController;
		gameController.heartRateOutputReceiver = heartRateMonitor;
	}
}
