using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float Speed;
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * Speed;
		if(transform.position.x <= -550)
		{
			transform.position = new Vector3(550, transform.position.y);
		}
    }
}
