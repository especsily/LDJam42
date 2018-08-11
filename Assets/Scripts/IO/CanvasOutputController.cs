using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class CanvasOutputController : MonoBehaviour, ICanvasOutputReceiver, ICanvasInfo
{
    [Header("Text")]
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private TMP_Text damageLabel;
    [SerializeField] private TMP_Text comboLabel;
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

    // --------------------------------- Display interface methods -------------------------------
    public void DisplayCombo(int combo)
    {
        DOTween.Complete(comboLabel);
        comboLabel.text = "Combo X " + combo;
        comboLabel.color = Utilities.ChangeColorAlpha(comboLabel.color, 1);
        comboLabel.DOColor(Utilities.ChangeColorAlpha(comboLabel.color, 0), 1f);
    }

    public void DisplayResult(string result, Color color, int comboStack, int damage)
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
        sequence.Append(damageLabel.DOColor(Utilities.ChangeColorAlpha(Color.red, 0), 1f));
        sequence.Join(damageLabel.transform.DOLocalMoveY(0, 1f).From());
        sequence.Append(Camera.main.DOShakePosition(1f, 5, 10));
        //TO DO: ANIMATION lose health
        sequence.Play();
    }
    public void DisplaySongTime(float time)
    {
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
            Destroy(child.gameObject);
        }
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
        if(firstCreate)
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
        runningOutLabel.DOColor(Utilities.ChangeColorAlpha(runningOutLabel.color, 1), 0.5f).SetLoops(4, LoopType.Yoyo);
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
}
