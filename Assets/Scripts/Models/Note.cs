using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] private string noteKey;
	[SerializeField] private int type; //0 nut 4 huong, 1 nut 8 huong, 2 nut do?, 3 nut chu*~
    private float noteSpeed;
    private bool isMissed = false;
    private bool isStacked = false;
    public IInputGiveup gameController;

    [SerializeField] private Sprite activeNote;
    [SerializeField] private Sprite deactiveNote;    

    public string GetKey()
    {
        return noteKey;
    }

	public int GetNoteType()
	{
		return type;
	}

    public void SetSpeed(float speed)
    {
        this.noteSpeed = speed;
    }

    public Sprite ReturnActiveNote()
    {
        return activeNote;
    }
    public Sprite ReturnDeactiveNote()
    {
        return deactiveNote;
    }

    void Update()
    {
        if (!isStacked)
        {
            (transform as RectTransform).anchoredPosition += Vector2.left * noteSpeed * Time.deltaTime;
        }

        if ((transform as RectTransform).anchoredPosition.x <= gameController.GetStackLine())
        {
            isStacked = true;
            gameController.AddToStackNote(this.gameObject);
            GetComponent<Image>().sprite = deactiveNote;
        }
        if ((transform as RectTransform).anchoredPosition.x <= -gameController.GetMissLine() && !isMissed)
        {
			isMissed = true;
            gameController.RemoveFromCurrentNote(this.gameObject);
        }
    }
}
