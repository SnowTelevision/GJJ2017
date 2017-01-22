using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnergyCircle : MonoBehaviour
{
    public GameObject energyCirclePrefab;
    public GameObject circleColliderPrefab;
    public float expandRate;
    public float energyCircleDuration;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(autoSpawnCircle());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void createEnergyCircle(float ExpandRate, float duration)
    {
        GameObject circle = Instantiate(energyCirclePrefab, new Vector3(transform.position.x, transform.position.y - 6, transform.position.z), energyCirclePrefab.transform.rotation);
        circle.GetComponent<SizeEnlarger>().expandRate = ExpandRate;

        Destroy(circle, duration);

        //GameObject trigger = Instantiate(circleColliderPrefab, new Vector3(transform.position.x, transform.position.y - 6, transform.position.z), circleColliderPrefab.transform.rotation);
        //trigger.GetComponent<IncreaseSpeed>().accelRate = ExpandRate;
        //
        //Destroy(trigger, 0.5f);
    }

    IEnumerator autoSpawnCircle()
    {
        while (true)
        {
            createEnergyCircle(expandRate, energyCircleDuration);

            yield return new WaitForSeconds(1f);
        }
    }
}
