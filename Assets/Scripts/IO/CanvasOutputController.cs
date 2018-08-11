using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class CanvasOutputController : MonoBehaviour, ICanvasOutputReceiver
{
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private TMP_Text skillNameLabel;
    [SerializeField] private RectTransform inputPanel;
    [SerializeField] private RectTransform suggestPanel;
    [SerializeField] private GameObject arrowPrefab;

    public void DisplayFinish(string result, ComboConfig combo = null)
    {
        resultLabel.text = result;
        resultLabel.color = Utilities.ChangeColorAlpha(SwitchResultColor(result), 1);

        DOTween.CompleteAll();
        Sequence sequence = DOTween.Sequence();

        if (combo != null)
        {
            skillNameLabel.text = combo.skillName;
            skillNameLabel.color = Utilities.ChangeColorAlpha(skillNameLabel.color, 1);

            sequence.Join(skillNameLabel.DOColor(Utilities.ChangeColorAlpha(skillNameLabel.color, 0), 2f));
            sequence.Join(skillNameLabel.transform.DOScale(new Vector3(1.5f, 1.5f), 0.4f).SetLoops(2, LoopType.Yoyo));
        }

        sequence.Join(resultLabel.DOColor(Utilities.ChangeColorAlpha(resultLabel.color, 0), 1f));
        sequence.Play();
    }

    public void DisplayInputs(List<Vector2Int> currentInputs)
    {
        DisplayComboToUI(currentInputs, inputPanel);
    }

    public void DisplaySuggestCombo(List<Vector2Int> suggestCombo)
    {
        DisplayComboToUI(suggestCombo, suggestPanel);
    }

    private void DisplayComboToUI(List<Vector2Int> combo, RectTransform panel)
    {
        if (combo.Count == 0)
        {
            foreach (Transform child in panel)
            {
                child.gameObject.SetActive(false);
            }
        }

        //display arrows
        for (int i = 0; i < combo.Count; i++)
        {
            if (i < panel.childCount)
            {
                SwitchArrowDirection(panel.GetChild(i) as RectTransform, combo[i]);
                panel.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                var newArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, panel);
                SwitchArrowDirection(newArrow.transform as RectTransform, combo[i]);
            }
        }
    }

    private void SwitchArrowDirection(RectTransform arrowImage, Vector2Int arrowDirection)
    {
        if (arrowDirection == Vector2Int.up) arrowImage.rotation = Quaternion.Euler(0, 0, 90);
        if (arrowDirection == Vector2Int.down) arrowImage.rotation = Quaternion.Euler(0, 0, -90);
        if (arrowDirection == Vector2Int.left) arrowImage.rotation = Quaternion.Euler(0, 0, 180);
        if (arrowDirection == Vector2Int.right) arrowImage.rotation = Quaternion.identity;
    }

    private Color SwitchResultColor(string result)
    {
        if (result == "Perfect")
            return Color.magenta;
        else if (result == "Good")
            return Color.green;
        else if (result == "Cool")
            return Color.cyan;
        else if (result == "Bad")
            return Color.grey;
        else if (result == "Miss")
            return Color.red;
        return Color.red;
    }
}
