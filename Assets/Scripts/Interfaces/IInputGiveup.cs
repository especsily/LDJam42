using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputGiveup {
	void RemoveFromCurrentNote(GameObject note);
	void AddToStackNote(GameObject note);
	float GetMissLine();
	float GetStackLine();
}
