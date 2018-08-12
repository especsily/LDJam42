using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
	public IInputReceiver inputReceiver;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
        {
			inputReceiver?.OnUserInput("up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
			inputReceiver?.OnUserInput("down");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
			inputReceiver?.OnUserInput("left");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
			inputReceiver?.OnUserInput("right");
        }

		//finish space
        if (Input.GetKeyDown(KeyCode.Space))
        {
			inputReceiver?.OnUserFinish();
        }
	}
}
