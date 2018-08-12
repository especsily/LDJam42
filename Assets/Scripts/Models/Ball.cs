using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	[HideInInspector]
	public bool isStart;
	[HideInInspector]
	public float spaceBarWidth;
	[HideInInspector]
	public float speed;
	private RectTransform rect;

	void Start()
	{
		rect = GetComponent<RectTransform>();
	}

	void Update()
	{
		if(isStart)
		{	
			rect.anchoredPosition += Vector2.left * speed * Time.deltaTime;
			if(rect.anchoredPosition.x <= -spaceBarWidth/2)
			{
				rect.anchoredPosition = new Vector2(spaceBarWidth - Mathf.Abs(rect.anchoredPosition.x), rect.anchoredPosition.y);
			}
		}
	}
}
