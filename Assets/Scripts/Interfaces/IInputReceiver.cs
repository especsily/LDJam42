using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReceiver {
    void OnUserInput(Vector2Int input);
    void OnUserFinish();
}
