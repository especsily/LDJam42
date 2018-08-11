using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class CanvasOutputController : MonoBehaviour, ICanvasOutputReceiver, ICanvasInfo
{
    //text
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private TMP_Text comboLabel;
    [SerializeField] private TMP_Text timeLabel;
    // [SerializeField] private TMP_Text skillNameLabel;

    //StackLine
    [SerializeField] private RectTransform stackLine;
    [SerializeField] private RectTransform inputPanel;

    // --------------------------------- Display interface methods -------------------------------
    public void DisplayCombo(int combo)
    {
        DOTween.Complete(comboLabel);
        comboLabel.text = "Combo X " + combo;
        comboLabel.color = Utilities.ChangeColorAlpha(comboLabel.color, 1);
        comboLabel.DOColor(Utilities.ChangeColorAlpha(comboLabel.color, 0), 1f);
    } 

    public void DisplayResult(string result, Color color)
    {
        DOTween.Complete(resultLabel);
        resultLabel.text = result;
        resultLabel.color = Utilities.ChangeColorAlpha(color, 1);
        comboLabel.DOColor(Utilities.ChangeColorAlpha(comboLabel.color, 0), 1f);
    }

    public void DisplaySongTime(float time)
    {
        timeLabel.text = Utilities.DisplayTime(time);
    }

    // --------------------------------- Info interface methods -------------------------------
    public float GetHalfInputPanelWidth()
    {
        return inputPanel.GetComponent<RectTransform>().rect.width / 2;
    }

    public RectTransform GetInputTransform()
    {
        return inputPanel;
    }

    public RectTransform GetStackTransform()
    {
        return stackLine;
    }

    public void RemoveAllStackPanel()
    {
        foreach (Transform child in stackLine)
        {
            Destroy(child.gameObject);
        }
    }

    // public void DisplayFinish(string result, ComboConfig combo = null)
    // {
    //     resultLabel.text = result;
    //     resultLabel.color = Utilities.ChangeColorAlpha(SwitchResultColor(result), 1);

    //     DOTween.CompleteAll();
    //     Sequence sequence = DOTween.Sequence();

    //     if (combo != null)
    //     {
    //         skillNameLabel.text = combo.skillName;
    //         skillNameLabel.color = Utilities.ChangeColorAlpha(skillNameLabel.color, 1);

    //         sequence.Join(skillNameLabel.DOColor(Utilities.ChangeColorAlpha(skillNameLabel.color, 0), 2f));
    //         sequence.Join(skillNameLabel.transform.DOScale(new Vector3(1.5f, 1.5f), 0.4f).SetLoops(2, LoopType.Yoyo));
    //     }

    //     sequence.Join(resultLabel.DOColor(Utilities.ChangeColorAlpha(resultLabel.color, 0), 1f));
    //     sequence.Play();
    // }

    // public void DisplayInputs(List<Vector2Int> currentInputs)
    // {
    //     DisplayComboToUI(currentInputs, inputPanel);
    // }

    // public void DisplaySuggestCombo(List<Vector2Int> suggestCombo)
    // {
    //     DisplayComboToUI(suggestCombo, suggestPanel);
    // }

    // private void DisplayComboToUI(List<Vector2Int> combo, RectTransform panel)
    // {
    //     if (combo.Count == 0)
    //     {
    //         foreach (Transform child in panel)
    //         {
    //             child.gameObject.SetActive(false);
    //         }
    //     }

    //     //display arrows
    //     for (int i = 0; i < combo.Count; i++)
    //     {
    //         if (i < panel.childCount)
    //         {
    //             SwitchArrowDirection(panel.GetChild(i) as RectTransform, combo[i]);
    //             panel.GetChild(i).gameObject.SetActive(true);
    //         }
    //         else
    //         {
    //             var newArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, panel);
    //             SwitchArrowDirection(newArrow.transform as RectTransform, combo[i]);
    //         }
    //     }
    // }

    // private void SwitchArrowDirection(RectTransform arrowImage, Vector2Int arrowDirection)
    // {
    //     if (arrowDirection == Vector2Int.up) arrowImage.rotation = Quaternion.Euler(0, 0, 90);
    //     if (arrowDirection == Vector2Int.down) arrowImage.rotation = Quaternion.Euler(0, 0, -90);
    //     if (arrowDirection == Vector2Int.left) arrowImage.rotation = Quaternion.Euler(0, 0, 180);
    //     if (arrowDirection == Vector2Int.right) arrowImage.rotation = Quaternion.identity;
    // }
}
