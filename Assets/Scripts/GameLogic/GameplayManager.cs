using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {
	[Header("Controller")]
	[SerializeField] private InputController inputController;
	[SerializeField] private AudioOutputController audioOutputController;
	[SerializeField] private CanvasOutputController canvasOutputController;
	[SerializeField] private GameLogic gameController;
	[SerializeField] private MenuController menuController;
	[SerializeField] private Generator generator;

	[Header("Characters")]
	[SerializeField] private Enemy enemy;
	[SerializeField] private Player player;
	[SerializeField] private Image playerImage;
	[SerializeField] private Image enemyImage;


	[SerializeField] private GalleryList listCard;
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

		menuController.listCard = listCard.listItem;
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
		enemy.otherCharacter = player;
		enemy.canvasOutput = canvasOutputController;
		enemy.audioController = audioOutputController;
		enemy.menuController = menuController;
		enemy.characterImage = enemyImage;

		player.otherCharacter = enemy;
		player.attackReceiver = enemy;
		player.gameController = gameController;
		player.canvasOutput = canvasOutputController;
		player.audioController = audioOutputController;
		player.menuController = menuController;
		player.characterImage = playerImage;
	}
}
