using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerator {
	GameObject GenerateNote(int type, Vector3 pos, float speed, Queue<GameObject> listNote, RectTransform parent);
}
