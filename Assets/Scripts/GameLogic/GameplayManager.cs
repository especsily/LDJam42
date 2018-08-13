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
	private List<GalleryItem> listCharacter;
	private int currentBossID;
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Awake()
	{
		currentBossID = PlayerPrefs.GetInt("CurrentBoss", 1);
		listCharacter = listCard.listItem;

		BindInput();
		BindOutput();
		BindGenerator();
		BindCharacter();
	}

	void BindPlayer()
	{
		//main 0
		player.MaxHP = listCharacter[0].MaxHP;
		player.AttackEffect = listCharacter[0].AttackEffect;
		player.hurtSound = listCharacter[0].hurtSound;
		player.attackSound = listCharacter[0].attackSound;
		player.HalfImage = listCharacter[0].halfImage;
		player.FinishImage = listCharacter[0].finishImage;

		player.animatorController = listCharacter[0].charAnimator;
	}

	void BindEnemy(int currentBossID)
	{
		enemy.Damage = (int) listCharacter[currentBossID].Damage;
		enemy.AttackTime = listCharacter[currentBossID].AttackTime;
		enemy.MaxHP = listCharacter[currentBossID].MaxHP;
		enemy.AttackEffect = listCharacter[currentBossID].AttackEffect;
		enemy.hurtSound = listCharacter[currentBossID].hurtSound;
		enemy.attackSound = listCharacter[currentBossID].attackSound;
		enemy.HalfImage = listCharacter[currentBossID].halfImage;
		enemy.FinishImage = listCharacter[currentBossID].finishImage;

		enemy.animatorController = listCharacter[currentBossID].charAnimator;
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
		canvasOutputController.audioController = audioOutputController;

		menuController.listCard = listCard.listItem;
		gameController.menuController = menuController;
		gameController.enemy = enemy;
	}

	void BindGenerator()
	{
		gameController.generator = generator;
	}

	void BindCharacter()
	{
		gameController.player = player;
		gameController.enemy = enemy;

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

		BindPlayer();
		BindEnemy(currentBossID);
	}
}
