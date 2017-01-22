using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatControl : MonoBehaviour
{
    public string difficulty;
    public GenerateEnergyCircle pumperCircle;
    public GeneratePortal pumperPortal;
    public PlayerController player;
    public float calibrat;
    public float portalAhead;
    public float circleAhead;
    public float beatRange;
    public float jumpRange;
    public AudioSource soundTrack;

    bool isPlaying;
    float[] timeStampsPortal;
    float[] timeStampsCircle;
    int portalMark;
    int circleMark;

	// Use this for initialization
	void Start ()
    {
        portalAhead = 1f;

        timeStampsPortal = new float[] { 5.5f, 9.5f, 15.5f, 16.5f, 18.5f, 20.5f, 22.5f, 24.5f, 26.5f,
                                        28.5f, 30.5f, 32.5f, 34.5f, 36.5f, 38.5f, 40.5f, 42.5f, 44.5f,
                                        46.5f, 51.5f, 53.5f, 55.5f, 57.5f, 59.5f, 61.5f, 63.5f, 65.5f,
                                        67.5f, 69.5f, 71.5f, 73.5f, 75.5f, 77.5f, 79f, 79.75f, 85f,
                                        86.5f, 88.5f, 90.5f, 92.5f, 94.5f, 96.5f, 98.5f, 99.5f,
                                        101.5f, 103.5f, 105.5f, 107.5f, 109.5f, 111.5f, 114.75f,
                                        117.25f, 118.5f, 119.75f, 121f, 122.25f, 123.5f, 124.75f,
                                        126f, 127.25f, 128.5f, 129.75f, 131.5f };
        timeStampsCircle = new float[] { 32f, 39f, 47f, 84f, 100f, 116f, 124f, 132f };

        soundTrack.Play();
        isPlaying = true;

        portalMark = 0;
        circleMark = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isPlaying)
        {
            for (int i = portalMark; i < timeStampsPortal.Length; i++)
            {
                createPortal(timeStampsPortal[i]);
            }

            for (int i = circleMark; i < timeStampsPortal.Length; i++)
            {
                createCircle(timeStampsCircle[i]);
            }
        }
    }

    void createPortal(float beatTime)
    {
        if (Time.time >= beatTime + calibrat - portalAhead && Time.time <= beatTime + calibrat - portalAhead + beatRange)
        {
            pumperPortal.createPortal(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z - 7), new Quaternion(), portalAhead);
            portalMark++;
        }
    }

    void createCircle(float circleTime)
    {
        if (Time.time >= circleTime + calibrat - circleAhead && Time.time <= circleTime + calibrat - circleAhead + beatRange)
        {
            pumperCircle.createEnergyCircle(pumperCircle.expandRate, pumperCircle.energyCircleDuration);
            circleMark++;
        }
    }
}
