using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] private string noteKey;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite destroySprite;
    private float noteSpeed;
    private bool isMissed = false;
    public IInputGiveup gameController;

    public string GetKey()
    {
        return noteKey;
    }

    public void SetSpeed(float speed)
    {
        this.noteSpeed = speed;
    }

    void Update()
    {
        if (!isMissed)
        {
            (transform as RectTransform).anchoredPosition += Vector2.left * noteSpeed * Time.deltaTime;
        }
        if ((transform as RectTransform).anchoredPosition.x <= -gameController.GetMissLine())
        {
			isMissed = true;
            gameController.RemoveFromCurrentNote(this.gameObject);
            gameController.AddToStackNote(this.gameObject);
        }
    }
}
