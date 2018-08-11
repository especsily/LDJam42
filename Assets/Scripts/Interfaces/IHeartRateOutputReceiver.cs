using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeartRateOutputReceiver {
	void SetBlipDuration(float duration);
	void PerformBlip(string result);
	void StartBlip();
}
