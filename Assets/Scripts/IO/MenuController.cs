using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class MenuController : MonoBehaviour, IMenuReceiver
{
    [Header("Lose panel")]
    [SerializeField] private Image losePanel;
    [SerializeField] private TMP_Text loseLabel;
    [SerializeField] private TMP_Text dealedDamageLabel;
    [SerializeField] private TMP_Text bossHPLabel;
    [SerializeField] private TMP_Text btnTryAgain;
    [SerializeField] private TMP_Text btnExit;

    [Header("Win panel")]
    [SerializeField] private Image winPanel;
    [SerializeField] private TMP_Text winLabel;
    [SerializeField] private TMP_Text congrat;
    [SerializeField] private Image cardBack;
    [SerializeField] private TMP_Text btnNext;
    [SerializeField] private TMP_Text btnGallery;
    public List<GalleryItem> listCard;
    private bool OnCardClicked = false;

    void Start()
    {
        btnTryAgain.GetComponent<Button>().onClick.AddListener(() => TryAgain());
        btnExit.GetComponent<Button>().onClick.AddListener(() => Exit());
        cardBack.GetComponent<Button>().onClick.AddListener(() => OnBackCardClicked());

        btnNext.GetComponent<Button>().onClick.AddListener(() => Next());
        btnGallery.GetComponent<Button>().onClick.AddListener(() => Gallery());
    }

    //  -------------------------- lose panel ---------------------------
    public void TryAgain()
    {
        TKSceneManager.ChangeScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        TKSceneManager.ChangeScene("StartScene");
    }

    public IEnumerator DisplayLosePanel(int dealedDamage, float bossHP)
    {
        Debug.Log("lose!!!");

        dealedDamageLabel.text = "Dealed damage : " + dealedDamage;
        bossHPLabel.text = "Boss HP Remaining : " + bossHP;
        yield return new WaitForSeconds(2f);

        losePanel.gameObject.SetActive(true);
        losePanel.DOColor(Utilities.ChangeColorAlpha(losePanel.color, 1), 0.5f);
        yield return new WaitForSeconds(0.5f);

        loseLabel.gameObject.SetActive(true);
        loseLabel.transform.DOMoveY(0, 0.5f).From();
        loseLabel.DOColor(Utilities.ChangeColorAlpha(loseLabel.color, 0), 0.5f).From();
        yield return new WaitForSeconds(0.5f);

        dealedDamageLabel.DOColor(Utilities.ChangeColorAlpha(dealedDamageLabel.color, 1), 0.5f);
        yield return new WaitForSeconds(0.5f);

        bossHPLabel.DOColor(Utilities.ChangeColorAlpha(bossHPLabel.color, 1), 0.5f);
        yield return new WaitForSeconds(0.5f);

        btnTryAgain.DOColor(Utilities.ChangeColorAlpha(btnTryAgain.color, 150f / 255f), 0.5f);
        yield return new WaitForSeconds(0.5f);

        btnExit.DOColor(Utilities.ChangeColorAlpha(btnExit.color, 150f / 255f), 0.5f);
    }

    //  -------------------------- win panel ---------------------------
    public IEnumerator DisplayWinPanel()
    {
		yield return new WaitForSeconds(2f);
        winPanel.gameObject.SetActive(true);
        winPanel.DOColor(Utilities.ChangeColorAlpha(winPanel.color, 1), 0.5f);
        yield return new WaitForSeconds(0.5f);
        winLabel.DOColor(Utilities.ChangeColorAlpha(winLabel.color, 1f), 0.5f);
        yield return new WaitForSeconds(0.5f);
        congrat.DOColor(Utilities.ChangeColorAlpha(congrat.color, 1f), 0.5f);
        yield return new WaitForSeconds(0.5f);

        cardBack.DOColor(Utilities.ChangeColorAlpha(cardBack.color, 1f), 0.5f);
        yield return new WaitForSeconds(0.5f);

		btnGallery.DOColor(Utilities.ChangeColorAlpha(btnGallery.color, 1f), 0.5f);
		btnNext.DOColor(Utilities.ChangeColorAlpha(btnNext.color, 1f), 0.5f);
    }

    public void OnBackCardClicked()
    {
        if (!OnCardClicked)
        {
            var currentBossID = PlayerPrefs.GetInt("CurrentBoss", 0);
            OnCardClicked = true;

            //flip card animation
            var sequence = DOTween.Sequence();
            sequence.Append(cardBack.transform.DOScaleX(0, 1));
            sequence.AppendCallback(() =>
            {
                var currentCharacter = listCard.Where(x => x.charId == currentBossID || x.charId == 0).FirstOrDefault();
                var rand = Random.Range(0, currentCharacter.pics.Count);
                Debug.Log(rand);
                cardBack.sprite = currentCharacter.pics[rand];
				PlayerPrefs.SetInt(currentCharacter+"_"+rand, 1);
            });
            sequence.Append(cardBack.transform.DOScaleX(1, 1));
            sequence.AppendInterval(5f);

            sequence.Play();
        }
    }

    public void Next()
    {
        if (OnCardClicked)
        {
            var currentBossID = PlayerPrefs.GetInt("CurrentBoss", 1);
            currentBossID++;
            if (currentBossID >= listCard.Count)
            {
                TKSceneManager.ChangeScene("StartScene");
            }
            else
            {
                PlayerPrefs.SetInt("CurrentBoss", currentBossID);
                TKSceneManager.ChangeScene("GameScene");
            }
        }
        else
        {
            OnBackCardClicked();
        }
    }

    public void Gallery()
    {
        if (OnCardClicked)
        {
            TKSceneManager.ChangeScene("GalleryScene");
        }
        else
        {
			OnBackCardClicked();
        }
    }
}
