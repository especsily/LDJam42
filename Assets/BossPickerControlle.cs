using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPickerControlle : MonoBehaviour {

    public Button fightBoss1;
    public Button fightBoss2;

    public void ChooseBoss1()
    {
        fightBoss1.gameObject.SetActive(true);
        fightBoss2.gameObject.SetActive(false);
    }

    public void ChooseBoss2()
    {
        fightBoss2.gameObject.SetActive(true);
        fightBoss1.gameObject.SetActive(false);
    }

	

}
