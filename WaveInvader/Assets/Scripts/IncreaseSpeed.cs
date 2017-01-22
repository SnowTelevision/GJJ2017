using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSpeed : MonoBehaviour
{
    public float accelRate;
    public WaveGenerator Wave;

    Quaternion accelDirection;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(accel(60f));

        Vector3 newDir = new Vector3(0, 180, 0);
        accelDirection.eulerAngles = newDir;
        transform.rotation = accelDirection;

        Wave = GameObject.Find("Wave").GetComponent<WaveGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float height = Wave.SampleHeight(pos);
        pos.y = height + Wave.transform.position.y;

        transform.position = pos;
    }

    IEnumerator accel(float speed)
    {
        while (true)
        {
            //this.transform.LookAt(GameObject.Find("Player").transform);
            this.GetComponent<Rigidbody>().AddForce(this.transform.forward * accelRate, ForceMode.Acceleration);
            accelRate *= 2f;

            yield return new WaitForSeconds(1f / speed);
        }
    }
}
