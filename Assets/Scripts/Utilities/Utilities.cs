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

    public Color SwitchResultColor(string result)
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
