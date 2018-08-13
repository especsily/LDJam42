using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CanvasOutputController : MonoBehaviour, ICanvasOutputReceiver, ICanvasInfo, ICharacterCanvasOutput
{
    [Header("Text")]
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private TMP_Text damageLabel;
    [SerializeField] private TMP_Text comboLabel;
    [SerializeField] private TMP_Text countDownLabel;
    [SerializeField] private TMP_Text timeLabel;
    [SerializeField] private TMP_Text runningOutLabel;

    [Header("Input")]
    //StackLine
    [SerializeField] private RectTransform stackPanel;
    [SerializeField] private RectTransform comingPanel;
    [SerializeField] private RectTransform inputPanel;
    [SerializeField] private RectTransform spaceBar;

    [Header("Space bar")]
    [SerializeField] private RectTransform ball;
    [SerializeField] private RectTransform spaceLeftPanel;
    [SerializeField] private GameObject spaceLeftPrefab;
    private Sequence sequence;
    private List<GameObject> spaceLeftList = new List<GameObject>();

    [Header("Characters")]
    [SerializeField] private float ShowCharacterImageTime;
    [SerializeField] private Image CharacterImage;
    [SerializeField] private TMP_Text ManaTimer;
    [SerializeField] private Image enemyHealth;
    [SerializeField] private Image playerHealth;
    public IHealth enemy, player;

    // --------------------------------- Display interface methods -------------------------------
    public void DisplayCountDown(bool active, float time)
    {
        if (active)
        {
            countDownLabel.text = (int)Mathf.Abs(time) + "";
            if (time >= -1)
            {
                countDownLabel.text = "Start";
            }
        }
        countDownLabel.gameObject.SetActive(active);
    }

    public void DisplayCombo(int combo)
    {
        DOTween.Complete(comboLabel);
        comboLabel.text = "Combo X " + combo;
        comboLabel.color = Utilities.ChangeColorAlpha(comboLabel.color, 1);
        comboLabel.DOColor(Utilities.ChangeColorAlpha(comboLabel.color, 0), 1f);
    }

    public void DisplayPlayerAttack(string result, Color color, int comboStack, int damage)
    {
        //reset
        DOTween.Complete(resultLabel);
        DOTween.Complete(damageLabel);
        sequence = DOTween.Sequence();

        //display result
        sequence.AppendCallback(() =>
        {
            resultLabel.text = result;
            resultLabel.color = Utilities.ChangeColorAlpha(color, 1);

            comboLabel.text = "Combo X " + comboStack;
            comboLabel.color = Utilities.ChangeColorAlpha(comboLabel.color, 1);
        });
        sequence.Append(resultLabel.DOColor(Utilities.ChangeColorAlpha(resultLabel.color, 0), 1f));
        sequence.Join(comboLabel.DOColor(Utilities.ChangeColorAlpha(comboLabel.color, 0), 1f));

        //display damage
        sequence.AppendCallback(() =>
        {
            damageLabel.text = damage.ToString();
            damageLabel.color = Utilities.ChangeColorAlpha(Color.white, 1);
        });
        sequence.Append(damageLabel.DOColor(Utilities.ChangeColorAlpha(Color.red, 0), 2f));
        sequence.Join(damageLabel.transform.DOLocalMoveY(0, 2f).From());

        //TO DO: ANIMATION lose health
        sequence.Play();
        sequence.OnStart(() => enemy.TakeDamage(damage));
        sequence.OnComplete(() =>
        {
            DOTween.Complete(Camera.main);
            Camera.main.DOShakePosition(1f, 5, 10);
        });
    }
    public void DisplaySongTime(float time)
    {
        timeLabel.gameObject.SetActive(true);
        timeLabel.text = Utilities.DisplayTime(time);
    }

    public void MoveBall(float speed)
    {
        ball.GetComponent<Ball>().speed = speed;
        ball.GetComponent<Ball>().isStart = true;
        ball.GetComponent<Ball>().spaceBarWidth = GetSpaceBarWidth();
    }

    public void RemoveAllStackPanel()
    {
        foreach (Transform child in stackPanel)
        {
            JumpAndFallAnimation(child);
        }
        // foreach (Transform child in stackPanel)
        // {
        //     Destroy(child.gameObject);
        // }
    }

    private void JumpAndFallAnimation(Transform child)
    {
        var randomPosX = child.transform.localPosition.x + Random.Range(-200, 200);

        child.transform.DOLocalJump(new Vector2(randomPosX, -1000), Random.Range(100, 200), 1, 1f)
        .OnStart(() => child.SetParent(inputPanel));

        child.gameObject.GetComponent<Image>().DOColor(Utilities.ChangeColorAlpha(child.gameObject.GetComponent<Image>().color, 0), 1f)
        .OnComplete(() => Destroy(child.gameObject));
    }

    public void RemoveComingPanel()
    {
        foreach (Transform child in comingPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplaySpaceLeft(bool firstCreate, int spaceLeft)
    {
        if (firstCreate)
        {
            for (int i = 0; i < spaceLeft; i++)
            {
                var space = Instantiate(spaceLeftPrefab, Vector3.zero, Quaternion.identity, spaceLeftPanel);
                spaceLeftList.Add(space);
            }
        }
        else
        {
            spaceLeftList[spaceLeft].gameObject.SetActive(false);
        }
    }

    public void DisplayRunningOut()
    {
        runningOutLabel.DOColor(Utilities.ChangeColorAlpha(runningOutLabel.color, 1), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    // --------------------------------- Info interface methods -------------------------------
    public float GetHalfInputPanelWidth()
    {
        return inputPanel.GetComponent<RectTransform>().rect.width / 2;
    }

    public RectTransform GetComingPanelTransform()
    {
        return comingPanel;
    }

    public float GetSpaceBarWidth()
    {
        return spaceBar.rect.width;
    }

    public RectTransform GetStackTransform()
    {
        return stackPanel;
    }

    public bool isFinishAnimation()
    {
        if (sequence == null)
        {
            return false;
        }
        else
        {
            return sequence.IsPlaying();
        }
    }

    // ----------------------------- Character interface methods --------------------------

    public void ShowEnemyMana(float manaTimer)
    {
        ManaTimer.gameObject.SetActive(true);
        ManaTimer.text = (int)manaTimer + "";
    }

    public void ShowEnemyHealth(float currentHP)
    {
        DOTween.To(() => enemyHealth.fillAmount, x => enemyHealth.fillAmount = x, currentHP, 0.5f);
    }

    public void ShowPlayerHealth(float currentHP)
    {
        DOTween.To(() => playerHealth.fillAmount, x => playerHealth.fillAmount = x, currentHP, 0.5f);
    }

    public void DisplayEnemyAttack(int damage)
    {
        DOTween.Complete(damageLabel);

        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            damageLabel.text = damage.ToString();
            damageLabel.color = Utilities.ChangeColorAlpha(Color.white, 1);
        });
        sequence.Append(damageLabel.DOColor(Utilities.ChangeColorAlpha(Color.red, 0), 1f));
        sequence.Join(damageLabel.transform.DOLocalMoveY(0, 1f).From());
        sequence.Play();
        sequence.OnStart(() => player.TakeDamage(damage));
        sequence.OnComplete(() =>
        {
            DOTween.Complete(Camera.main);
            Camera.main.DOShakePosition(1f, 5, 10);
        });
    }

    public void SpaceEffect()
    {
        spaceBar.transform.DOScaleX(1.1f, 0.5f).SetLoops(2, LoopType.Yoyo);
    }

    public void SpaceResult(Color color)
    {
        spaceBar.GetComponent<Image>().DOColor(color, 0.5f).SetLoops(2, LoopType.Yoyo);
    }
}
