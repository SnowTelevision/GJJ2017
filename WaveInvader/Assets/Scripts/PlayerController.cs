using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isJump;

    public float jumpHeight;
    public float timeToJumpApex;
    public float playerMaxHealth;
    public float playerHealth;
    public float healthRegenSpeed;
    public float droneDamage;

    public float gravity;
    public float jumpVelocity;
    public Vector3 velocity;

    public WaveGenerator Wave;
    public GameObject thirdCam;
    public Image damageEffect;
    public Image healthBar;
    public Image progressBar;
    public Image crosshair;

    public LineRenderer laserBeam;
    public Transform laserStart;
    public Vector3 laserStartOffset;

    public Text scoreText;

    private bool laserGunCooling = false;
    private int score = 0;

    // Use this for initialization
    void Start()
    {
        isJump = false;

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        velocity = new Vector3(0, 0, 0);

        playerHealth = playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = string.Format("Score: {0,10}", score);

        velocity.x = 0;
        velocity.z = 0;

        //Calculate wave height
        Vector3 pos = transform.position;
        float height = Wave.SampleHeight(pos);
        pos.y = height + Wave.transform.position.y + 1;

        if (Input.GetButtonDown("Jump") && !isJump)
        {
            isJump = true;
            jumpp();
        }

        RaycastHit hit;
        GameObject target = null;

        if (Physics.Raycast(thirdCam.transform.position, thirdCam.transform.forward, out hit, Mathf.Infinity) &&
            hit.transform.tag == "Drone")
        {
            crosshair.color = Color.red;
            target = hit.transform.gameObject;
        }
        else
        {
            crosshair.color = Color.green;
            target = null;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            shoott(target);
        }

        if (!isJump) //Control player's transform while not jumping
        {
            velocity.y = 0;
            transform.position = pos;
        }

        else if (isJump)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        transform.Translate(velocity * Time.deltaTime);

        if (transform.position.y <= pos.y)
        {
            velocity.y = 0;
            isJump = false;
        }

        if (playerHealth <= 0)
        {
            Debug.LogError("You Lose!");
        }

        healthBar.fillAmount = playerHealth / playerMaxHealth;

        //if(velocity.y != 0)
        //{
        //    print(velocity.y);
        //}
    }

    private void FixedUpdate()
    {
        if (playerHealth < playerMaxHealth)
        {
            playerHealth += healthRegenSpeed;
        }
    }

    public void jumpp()
    {
        velocity.y = jumpVelocity;
    }

    public void shoott(GameObject go)
    {
        if (laserGunCooling)
            return;
        //print("You hit: " + hit.transform.name);
        Vector3 targetPosition;

        if (null != go && go.transform.tag == "Drone")
        {
            if (playerHealth <= 95)
            {
                playerHealth += 5f;
            }

            else if (playerHealth > 95 && playerHealth <= 100)
            {
                playerHealth = 100;
            }

            score++;
            go.transform.GetComponent<DroneBehavior>().getHit();
            targetPosition = go.transform.position;
        }
        else
        {
            Camera cam = Camera.main;
            targetPosition = cam.transform.position + cam.transform.forward * 100f;
        }

        laserBeam.SetPosition(0, laserStart.position + laserStartOffset);
        laserBeam.SetPosition(1, targetPosition);
        laserBeam.enabled = true;
        laserGunCooling = true;
        StartCoroutine(LaserGunTimer());
    }

    IEnumerator LaserGunTimer()
    {
        yield return new WaitForSeconds(0.02f);

        laserBeam.enabled = false;

        yield return new WaitForSeconds(0.02f);

        laserGunCooling = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Drone")
        {
            //print("Being Hit");

            playerHealth -= droneDamage;
            StartCoroutine(fadeDamageEffect(0.5f));
            Destroy(other.gameObject);
        }

        else
        {
            //print("circle");
        }
    }

    IEnumerator fadeDamageEffect(float duration)
    {
        float alpha = 47f / 255f;

        //print(alpha);

        for (float t = 0f; t < 1f; t += Time.deltaTime / duration)
        {
            Color newColor = new Color(damageEffect.color.r, damageEffect.color.g,
                                       damageEffect.color.b, Mathf.Lerp(alpha, 0, t));

            damageEffect.color = newColor;

            //print(damageEffect.color);

            yield return null;
        }
    }
}
