using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePortal : MonoBehaviour
{
    public GameObject portalPrefab;
    public float PortalDuration;
    public float randomRangeX;
    public float randomRangeY;
    public float heightDisplacement;
    public bool autoSpawn;

    // Use this for initialization
    void Start ()
    {
        if (autoSpawn)
        {
            //StartCoroutine(autoSpawnPortal());
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void createPortal(Vector3 spawnPosition, Quaternion spawnRotation, float portalDuration)
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        GameObject portal = Instantiate(portalPrefab, new Vector3(
            spawnPosition.x + Random.Range(-(randomRangeX / 2f), randomRangeX / 2f ), 
            spawnPosition.y + Random.Range(-(randomRangeY / 2f), randomRangeY / 2f ) + heightDisplacement, 
            spawnPosition.z), spawnRotation);

        portal.GetComponent<GenerateEnemy>().WaitTime = portalDuration;

        StartCoroutine(portalStart(portal, portalDuration));

        Destroy(portal, portalDuration);
    }

    IEnumerator portalStart(GameObject portal, float duration)
    {
        float alpha = portal.GetComponent<Renderer>().material.color.a;

        for(float t = 0f; t < 1f; t += Time.deltaTime / duration)
        {
            Color newColor = new Color(portal.GetComponent<Renderer>().material.color.r, portal.GetComponent<Renderer>().material.color.g, 
                                       portal.GetComponent<Renderer>().material.color.b, Mathf.Lerp(alpha, 0, t));

            portal.GetComponent<Renderer>().material.color = newColor;

            yield return null;
        }
    }

    IEnumerator autoSpawnPortal()
    {
        while(true)
        {
            createPortal(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z - 7), new Quaternion(), 1f);

            yield return new WaitForSeconds(1f);
        }
    }
}
