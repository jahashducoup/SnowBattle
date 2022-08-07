using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    // Prefabs
    public GameObject   snowballPrefab;
    public GameObject   loadingAmmoBarPrefab;
    public GameObject   myUIPrefab;
    public GameObject   outOfAmmoIndicatorPrefab;
    public AudioClip    audioAmmoReloadPrefab;


    // PlayerData variables
    public  Sprite      mySprite;
    private KeyCode     shootKey;
    private KeyCode     reloadKey;
    private KeyCode     crouchingKey;
    public int          playerNumber;
    public Sprite       myHead;


    // Variables that will grab a GameObject at Start()
    private SpriteRenderer  spriteRenderer;
    private Rigidbody2D     body;
    private AudioSource     audioSource;
    public GameObject       canvas;
    private Animator        animator;
    public PlayerData       playerData;


    // Variables that are setup at Start()
    private float   speed;
    private float   outofAmmoCd;
    private bool    playerIsInSafeZone;
    private float   shotCd;
    private bool    youCanShoot;
    private bool    outofAmmoCanPop;
    public int      currentAmmoStock;
    private bool    isReloading;
    private float   reloadingTime;
    private bool    youCanReloadInArea;
    private int     currentHp;
    public bool     youAreDead;
    private bool    gotHitBySnowball;
    private float   stunAfterHitCd;
    private String  myPlayerTag; //Used to save playertag data


    // Variables
    private GameObject  playerUI; 
    private Vector3     axisMovement;


    void Start()
    {
        GrabGameObjectsAtStart();
        TransferPlayerDataToVariable();
        SetupVariablesAtStart();
        InstantiatePlayerUI();
    }


    void Update()
    {

        // You dead
        if (currentHp <= 0) youAreDead = true;

        if (youAreDead == true)
        {
            YouAreDeadMotherFucker();
            return; // Prevent the player from doing anything else
        }

        // Catch player movement inputs
        axisMovement.x = Input.GetAxisRaw("Horizontal" + playerData.playerNumber + "P");
        axisMovement.y = Input.GetAxisRaw("Vertical"  + playerData.playerNumber + "P");

        // Can the player shoot? Then decrease the cooldown timer until he can shoot again
        if(youCanShoot == false)
        {
            shotCd -= Time.deltaTime;
            if (shotCd <= .1f) youCanShoot = true;
        }     
        
        // Can the OUTOFAMMO popup show? If no, then decrease the cooldown timer until it can pop again
        if(outofAmmoCanPop == false)
        {
            outofAmmoCd -= Time.deltaTime;
            if (outofAmmoCd <= .1f) outofAmmoCanPop = true;
        }    

        // Touched by Snowball ! Handle stun cooldown
        if(gotHitBySnowball == true)
        {
            stunAfterHitCd -= Time.deltaTime;
            speed = 0f;

            if(stunAfterHitCd <= .1f)
            {
                stunAfterHitCd = playerData.stunAfterHitCd;
                gotHitBySnowball = false;
                speed = playerData.speed;
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
                speed = playerData.speed;
            }
        }

        // You can do everything you want
        else 
        {
            // Shoot !
            if (Input.GetKeyDown(shootKey))
            {
                if(youCanShoot == true && gotHitBySnowball == false)
                {
                    if(currentAmmoStock > 0)
                    {
                        // Throw a snowball
                        Shoot();
                        shotCd = playerData.shotCd;
                        youCanShoot = false;
                        currentAmmoStock -= 1;
                    }
                    else if(currentAmmoStock == 0 && outofAmmoCanPop == true)
                    {
                        // Instantiate the OUTOFAMMO popup
                        GameObject outOfAmmoGObject = Instantiate(outOfAmmoIndicatorPrefab,  transform.position + new Vector3(transform.localScale.x*0.4f, 1f, 0f) , transform.rotation);
                        outOfAmmoGObject.GetComponent<InfoBox>().parent = gameObject;
                        outofAmmoCanPop = false;
                        outofAmmoCd = playerData.outOfAmmoBoxCd;
                    }
                }
            }

            // Reloading
            if(Input.GetKeyDown(reloadKey))
            {
                if(isReloading == false)
                {
                    if (youCanReloadInArea == true && currentAmmoStock < playerData.ammoMax)
                    {
                        currentAmmoStock += 1;
                        reloadingTime = playerData.reloadingDuration;
                        isReloading = true;
                        GameObject loadingAmmoGObject = Instantiate(loadingAmmoBarPrefab,  transform.position + new Vector3(0f, 1.3f, 0f) , transform.rotation);
                        audioSource.PlayOneShot(audioAmmoReloadPrefab);
                    }
                }
            }

            // Crouching
            if (Input.GetKeyDown(crouchingKey))
            {
                animator.SetBool("isCrouching", true);
                speed = playerData.speedWhenCrouching;
                if (playerIsInSafeZone == true) transform.tag = "SafePlayer";
            }
            
            // Uncrouching
            if (Input.GetKeyUp(crouchingKey))
            {
                animator.SetBool("isCrouching", false);
                speed = playerData.speed;
                transform.tag = myPlayerTag;
            }
        }
    }

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

    private void SetupVariablesAtStart()
    {
        playerIsInSafeZone      = false;
        youCanShoot             = true;
        outofAmmoCanPop         = true;
        isReloading             = false;
        youCanReloadInArea      = false;
        youAreDead              = false;
        gotHitBySnowball        = false;
        transform.tag           = "Player" + playerData.playerNumber.ToString();
        myPlayerTag             = transform.tag;
    }

    private void TransferPlayerDataToVariable()
    {
        spriteRenderer.sprite               = playerData.mySprite;
        speed                               = playerData.speed;
        outofAmmoCd                         = playerData.outOfAmmoBoxCd;
        shotCd                              = playerData.shotCd;
        currentAmmoStock                    = playerData.ammoMax;
        reloadingTime                       = playerData.reloadingDuration;
        currentHp                           = playerData.hp;
        stunAfterHitCd                      = playerData.stunAfterHitCd;
        shootKey                            = playerData.shootKey;
        reloadKey                           = playerData.reloadKey;
        crouchingKey                        = playerData.crouchingKey;
        playerNumber                        = playerData.playerNumber;
        myHead                              = playerData.myHead;
        animator.runtimeAnimatorController  = playerData.animator;
    }

    private void GrabGameObjectsAtStart()
    {
        canvas          = GameObject.Find("Canvas");
        audioSource     = GetComponent<AudioSource>();
        body            = GetComponent<Rigidbody2D>();
        animator        = GetComponent<Animator>();
        spriteRenderer  = GetComponent<SpriteRenderer>();
    }

    private void InstantiatePlayerUI()
    {
        if(playerNumber == 1)  playerUI = Instantiate(myUIPrefab, new Vector2(-8.5f, 4.5f), Quaternion.identity, canvas.transform);
        if(playerNumber == 2)  playerUI = Instantiate(myUIPrefab, new Vector2(8.5f, 4.5f), Quaternion.identity, canvas.transform);
        if(playerNumber == 3)  playerUI = Instantiate(myUIPrefab, new Vector2(-8.5f, -4.5f), Quaternion.identity, canvas.transform);
        if(playerNumber == 4)  playerUI = Instantiate(myUIPrefab, new Vector2(8.5f, -4.5f), Quaternion.identity, canvas.transform);

        playerUI.GetComponent<PlayerUI>().playerAttachedTo = gameObject;
    }

    private void YouAreDeadMotherFucker()
    {
        axisMovement.x = 0;
        axisMovement.y = 0;
        Color myColor = GetComponent<SpriteRenderer>().color;
        myColor.a = .5f;
        GetComponent<SpriteRenderer>().color = myColor; 
    }
}
