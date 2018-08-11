using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
	[SerializeField] private InputController inputController;
	[SerializeField] private AudioOutputController audioOutputController;
	[SerializeField] private CanvasOutputController canvasOutputController;
	[SerializeField] private HeartRateMonitor heartRateMonitor;
	[SerializeField] private GameLogic gameController;
	[SerializeField] private Generator generator;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Awake()
	{
		BindInput();
		BindOutput();
		BindGenerator();
	}

	void BindInput()
	{
		inputController.inputReceiver = gameController;
	}

	void BindOutput()
	{
		gameController.audioInfo = audioOutputController;
		gameController.audioOutputReceiver = audioOutputController;
		gameController.canvasInfo = canvasOutputController;
		gameController.canvasOutputReceiver = canvasOutputController;
		gameController.heartRateOutputReceiver = heartRateMonitor;
	}

	void BindGenerator()
	{
		gameController.generator = generator;
	}
}
