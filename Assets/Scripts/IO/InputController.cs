using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
	public IInputReceiver inputReceiver;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		inputReceiver = GetComponent<IInputReceiver>();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
        {
			inputReceiver?.OnUserInput(Vector2Int.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
			inputReceiver?.OnUserInput(Vector2Int.down);

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
			inputReceiver?.OnUserInput(Vector2Int.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
			inputReceiver?.OnUserInput(Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
			inputReceiver?.OnUserFinish();
        }
	}
}
