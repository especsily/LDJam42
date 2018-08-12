using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utilities : MonoBehaviour
{
    public static string DisplayTime(float timer)
    {
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
        return string.Format("{0}:{1}", minutes, seconds);
    }

	public static Color ChangeColorAlpha(Color color, float a)
	{
		return new Color(color.r, color.g, color.b, a);
	}
}
