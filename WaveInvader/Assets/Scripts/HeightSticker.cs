using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightSticker : MonoBehaviour
{
    public WaveGenerator terrain;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float height = terrain.SampleHeight(pos);
        pos.y = height;
        transform.position = pos;
    }
}
