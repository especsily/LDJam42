using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour, IMenuReceiver {
	[Header("Lose panel")]
	[SerializeField] private Image losePanel;
	[SerializeField] private TMP_Text label;
	[SerializeField] private TMP_Text dealedDamageLabel;
	[SerializeField] private TMP_Text bossHPLabel;
	[SerializeField] private Button btnTryAgain;
	[SerializeField] private Button btnExit;

	void Start()
	{
		btnTryAgain.GetComponent<Button>().onClick.AddListener(() => TryAgain());
		btnExit.GetComponent<Button>().onClick.AddListener(() => Exit());
	}

	public void TryAgain()
	{
		TKSceneManager.ChangeScene(SceneManager.GetActiveScene().name);
	}

	public void Exit()
	{
		TKSceneManager.ChangeScene("StartScene");
	}

    public void DisplayLosePanel(int dealedDamage, float bossHP)
    {
        dealedDamageLabel.text = "Dealed damage : " + dealedDamage;
		bossHPLabel.text = "Boss HP Remaining : " + bossHP;
		
		losePanel.gameObject.SetActive(true);
    }
}
