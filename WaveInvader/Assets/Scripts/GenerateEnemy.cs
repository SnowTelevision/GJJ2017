using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemy : MonoBehaviour
{
    public GameObject dronePrefab;
    public float WaitTime;

    public Renderer rend;

    public Vector4 param;

    private int paramId;
    private Material mat;

	// Use this for initialization
	void Start ()
    {
        mat = new Material(rend.sharedMaterial);
        paramId = Shader.PropertyToID("_Param");
        mat.SetVector(paramId, param);
        rend.material = mat;

        createEnemy(WaitTime);

        StartCoroutine(portalStart(gameObject, WaitTime));

        Destroy(gameObject, WaitTime);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void createEnemy(float waitTime)
    {
        StartCoroutine(enemyStart(waitTime, transform.position, transform.rotation));
    }

    IEnumerator portalStart(GameObject portal, float duration)
    {
        //float alpha = portal.GetComponent<Renderer>().material.color.a;

        for (float t = 0f; t < 1f; t += Time.deltaTime / duration)
        {
            //Color newColor = new Color(portal.GetComponent<Renderer>().material.color.r, portal.GetComponent<Renderer>().material.color.g,
            //                           portal.GetComponent<Renderer>().material.color.b, Mathf.Lerp(alpha, 0, t));

            //portal.GetComponent<Renderer>().material.color = newColor;

            param.z = Mathf.Lerp(0, -7, t);
            mat.SetVector(paramId, param);

            yield return null;
        }
    }

    IEnumerator enemyStart(float waitTime, Vector3 startPosition, Quaternion startRotation)
    {
        //print("spawn drone");
        yield return new WaitForSeconds(waitTime - 0.2f);

        //print("spawn drone now");
        GameObject drone = Instantiate(dronePrefab, startPosition, startRotation);
    }
}
