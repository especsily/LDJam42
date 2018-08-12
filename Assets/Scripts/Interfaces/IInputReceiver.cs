using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReceiver {
    void OnUserInput(string key);
    void OnUserFinish();
}
