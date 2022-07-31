using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    // Constants
    private const float STUN_AFTER_HIT_CD = 1f;
    private const float SPEED = 2.5f;
    private const float SPEED_CROUCH = 1f;
    private const float SHOT_CD = .8f;
    private const int AMMO_MAX = 6;
    private const float OUTOFAMMOBOX = 3f;
    private const float RELOADINGTIME = 1f;
    private const int HPMAX = 10;

    //

    public KeyCode shootKey = KeyCode.Q;
    public KeyCode reloadKey = KeyCode.E;
    public KeyCode crouchingKey = KeyCode.C;

    [SerializeField]
    private float speed = SPEED;
    private float outofAmmoCd = OUTOFAMMOBOX;
    private Vector3 axisMovement;
    private Rigidbody2D body;
    public GameObject snowballPrefab;
    public GameObject outOfAmmoIndicatorPrefab;
    public GameObject loadingAmmoBar;
    public GameObject myUI;
    private AudioSource audioSource;
    public AudioClip audioAmmoReload;
    private GameObject playerUI; 

    private Animator animator;
    private bool playerIsInSafeZone = false;
    private string myPlayerTag;
    private float shotCd = SHOT_CD;
    private bool youCanShoot = true;
    private bool outofAmmoCanPop = true;
    public int currentAmmoStock = AMMO_MAX;

    private bool isReloading = false;
    private float reloadingTime = RELOADINGTIME;
    private bool youCanReloadInArea = false;
    private int currentHp = HPMAX;
    public bool youAreDead = false;

    //Hit
    private bool gotHitBySnowball = false;
    private float stunAfterHitCd = STUN_AFTER_HIT_CD;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myPlayerTag = transform.tag;
        playerUI = Instantiate(myUI);
        playerUI.GetComponent<PlayerUI>().playerAttachedTo = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // You dead
        if (currentHp == 0)
        {
            youAreDead = true;
        }

        if (youAreDead == true)
        {
            axisMovement.x = 0;
            axisMovement.y = 0;
            Color myColor = GetComponent<SpriteRenderer>().color;
            myColor.a = .5f;
            GetComponent<SpriteRenderer>().color = myColor; 
            return;
        }

        axisMovement.x = Input.GetAxisRaw("Horizontal" + gameObject.name.Split("(")[0]);
        axisMovement.y = Input.GetAxisRaw("Vertical"  + gameObject.name.Split("(")[0]);

        if(youCanShoot == false)
        {
            shotCd -= Time.deltaTime;

            if (shotCd < .1f)
            {
                youCanShoot = true;
            }
        }     
        
        if(outofAmmoCanPop == false)
        {
            outofAmmoCd -= Time.deltaTime;

            if (outofAmmoCd <= .1f)
            {
                outofAmmoCanPop = true;
            }
        }    

        // Touched by Snowball !
        if(gotHitBySnowball == true)
        {
            stunAfterHitCd -= Time.deltaTime;
            speed = 0f;

            if(stunAfterHitCd <= .1f)
            {
                stunAfterHitCd = STUN_AFTER_HIT_CD;
                gotHitBySnowball = false;
                speed = SPEED;
            }
        }

        // Reloading !
        if(isReloading == true)
        {
            reloadingTime -= Time.deltaTime;
            speed = 0f;

            if(reloadingTime <= .1f)
            {
                isReloading = false;
                speed = SPEED;
            }
        }
        else
        {
            // Shoot !
            if (Input.GetKeyDown(shootKey))
            {
                if(youCanShoot == true)
                    if(currentAmmoStock > 0)
                    {
                        // Throw a snowball
                        Shoot();
                        shotCd = SHOT_CD;
                        youCanShoot = false;
                        currentAmmoStock -= 1;
                    }
                    else if(currentAmmoStock == 0 && outofAmmoCanPop == true)
                    {
                        GameObject outOfAmmoGObject = Instantiate(outOfAmmoIndicatorPrefab,  transform.position + new Vector3(transform.localScale.x*0.4f, 1f, 0f) , transform.rotation);
                        outOfAmmoGObject.GetComponent<InfoBox>().parent = gameObject;
                        outofAmmoCanPop = false;
                        outofAmmoCd = OUTOFAMMOBOX;
                    }
            }

            // Reloading
            if(Input.GetKeyDown(reloadKey))
            {
                if(isReloading == false)
                {
                    if (youCanReloadInArea == true && currentAmmoStock < AMMO_MAX)
                    {
                        currentAmmoStock += 1;
                        reloadingTime = STUN_AFTER_HIT_CD;
                        isReloading = true;
                        GameObject loadingAmmoGObject = Instantiate(loadingAmmoBar,  transform.position + new Vector3(0f, 1.3f, 0f) , transform.rotation);
                        audioSource.PlayOneShot(audioAmmoReload);
                    }
                }
            }

            // Crouching
            if (Input.GetKeyDown(crouchingKey))
            {
                animator.SetBool("isCrouching", true);
                speed = SPEED_CROUCH;

                if (playerIsInSafeZone == true)
                {
                    transform.tag = "SafePlayer";
                }
            }
            
            // Uncrouching
            if (Input.GetKeyUp(crouchingKey))
            {
                animator.SetBool("isCrouching", false);
                speed = SPEED;
                transform.tag = myPlayerTag;
            }
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        body.velocity = axisMovement.normalized * speed;
        CheckForFlipping();
    }

    private void CheckForFlipping()
    {
        if(axisMovement.x < 0)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y);
        }
        else if(axisMovement.x > 0)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y);
        }
    }

    void Shoot()
    {
        GameObject snowball = Instantiate(snowballPrefab, transform.position + new Vector3(transform.localScale.x*0.3f, 0, 0) , transform.rotation);
        snowball.GetComponent<SnowBallMovement>().masterSpawnPosition = transform.position;
        snowball.GetComponent<SnowBallMovement>().SpawnedBy = myPlayerTag;
        if(transform.tag == "SafePlayer")
        {
            //You fucking cant shot
            snowball.GetComponent<SnowBallMovement>().mustKillAtFirstCollide = true;
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile" 
        && other.GetComponent<SnowBallMovement>().SpawnedBy != transform.tag)
        {
            if (transform.tag != "SafePlayer")
            {
                currentHp -= 1;
                gotHitBySnowball = true;
                StartCoroutine(VisualIndicator(Color.red));
            }
        }
        else if (other.tag == "SafeZone")
        {
            playerIsInSafeZone = true;
        }
        else if (other.tag == "SafeZone")
        {
            playerIsInSafeZone = true;
        }
        else if (other.tag == "Reload")
        {
            youCanReloadInArea = true;
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "SafeZone")
        {
            playerIsInSafeZone = false;
        }
        else if (other.tag == "Reload")
        {
            youCanReloadInArea = false;
        }
    }

    private IEnumerator VisualIndicator(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.10f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.10f);
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.10f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
