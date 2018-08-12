using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAnimDeactive : MonoBehaviour
{
    public float delay = 0f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(WaitTime(this.gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay));
    }

    private IEnumerator WaitTime(GameObject gameObject, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }
}
