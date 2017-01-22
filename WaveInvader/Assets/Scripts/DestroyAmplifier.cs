using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAmplifier : MonoBehaviour
{
    public float lastingTime;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, lastingTime);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
