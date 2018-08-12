using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
	[Header("Controller")]
	[SerializeField] private InputController inputController;
	[SerializeField] private AudioOutputController audioOutputController;
	[SerializeField] private CanvasOutputController canvasOutputController;
	[SerializeField] private GameLogic gameController;
	[SerializeField] private Generator generator;

	[Header("Characters")]
	[SerializeField] private Enemy enemy;
	[SerializeField] private Player player;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Awake()
	{
		BindInput();
		BindOutput();
		BindGenerator();
		BindCharacter();
	}

	void BindInput()
	{
		inputController.inputReceiver = gameController;
	}

	void BindOutput()
	{
		gameController.audioController = audioOutputController;
		gameController.audioInfo = audioOutputController;
		gameController.canvasInfo = canvasOutputController;
		gameController.canvasOutputReceiver = canvasOutputController;

		canvasOutputController.enemy = enemy;
		canvasOutputController.player = player;
	}

	void BindGenerator()
	{
		gameController.generator = generator;
	}

	void BindCharacter()
	{
		gameController.player = player;

		enemy.gameLogic = gameController;
		enemy.gameController = gameController;
		enemy.player = player;
		enemy.canvasOutput = canvasOutputController;

		player.enemy = enemy;
		player.attackReceiver = enemy;
		player.gameController = gameController;
		player.canvasOutput = canvasOutputController;
	}
}
