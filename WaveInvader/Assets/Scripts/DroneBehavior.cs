using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBehavior : MonoBehaviour
{
    public float accelSpeed;
    public GameObject amplifierPrefab;
    public float agroDistance;

    GameObject player;
    bool agro;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player");
        agro = false;
    }

    // Update is called once per frame
    void Update ()
    {
        this.transform.LookAt(player.transform);

        GetComponent<Rigidbody>().AddForce(this.transform.forward * accelSpeed, ForceMode.Acceleration);

        if(Vector3.Distance(transform.position, player.transform.position) < agroDistance)
        {
            agro = true;
        }

        if(agro)
        {
            this.transform.LookAt(player.transform);

            GetComponent<Rigidbody>().AddForce(this.transform.forward * accelSpeed * 100, ForceMode.Acceleration);
        }
    }

    public void getHit()
    {
        GameObject amplifier = Instantiate(amplifierPrefab, transform.position, transform. rotation);

        amplifier.transform.LookAt(GameObject.Find("SpacePumper").transform);

        Quaternion reverseRotation = new Quaternion();

        reverseRotation.eulerAngles = new Vector3(-amplifier.transform.rotation.eulerAngles.x, -amplifier.transform.rotation.eulerAngles.y, 
                                                  -amplifier.transform.rotation.eulerAngles.z);

        amplifier.transform.rotation = reverseRotation;
        
        Destroy(transform.gameObject);
    }
}
