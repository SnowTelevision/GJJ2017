using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeEnlarger : MonoBehaviour
{
    public float expandRate;
    public WaveGenerator Wave;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(enlarge(60f));

        Wave = GameObject.Find("Wave").GetComponent<WaveGenerator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Vector3 pos = transform.position;
        //float height = Wave.SampleHeight(pos);
        //pos.y = height + Wave.transform.position.y;
        //
        //transform.position = pos;
    }

    IEnumerator enlarge(float speed)
    {
        while (true)
        {
            transform.localScale *= expandRate;

            yield return new WaitForSeconds(1f / speed);
        }
    }
}
