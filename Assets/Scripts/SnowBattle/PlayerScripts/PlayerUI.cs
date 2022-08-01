using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    
    public enum UIEmplacement
    {
        BottomLeft,
        BottomRight,
        UpperLeft,
        UpperRight,
    }

    public GameObject playerAttachedTo;
    public GameObject ammoPrefab;
    private int AMMO_MAX;
    private GameObject[] ammos;

    private List<Vector3> snowBallsLocation;
    public UIEmplacement emplacementUI = UIEmplacement.UpperLeft;
    private float iconsOffset;
    private GameObject myHead;

    void Start()
    {
        //Set Variables
        AMMO_MAX = playerAttachedTo.GetComponent<PlayerMovement>().currentAmmoStock;
        ammos = new GameObject[AMMO_MAX];

        //Get the emplacement of the UI
        if (playerAttachedTo.GetComponent<PlayerMovement>().playerNumber == 1) emplacementUI = UIEmplacement.UpperLeft;
        if (playerAttachedTo.GetComponent<PlayerMovement>().playerNumber == 2) emplacementUI = UIEmplacement.UpperRight;
        if (playerAttachedTo.GetComponent<PlayerMovement>().playerNumber == 3) emplacementUI = UIEmplacement.BottomLeft;
        if (playerAttachedTo.GetComponent<PlayerMovement>().playerNumber == 4) emplacementUI = UIEmplacement.BottomRight;

        //Display the head of the player
        myHead = Instantiate(new GameObject(), gameObject.transform.position , Quaternion.identity, gameObject.transform);
        myHead.name = "myHead";
        myHead.layer = 15; // UI Elements
        myHead.AddComponent<SpriteRenderer>();
        myHead.GetComponent<SpriteRenderer>().sortingLayerName = "UI 2";
        myHead.GetComponent<SpriteRenderer>().sprite = playerAttachedTo.GetComponent<PlayerMovement>().myHead;

        //Display ammos on UI
        iconsOffset = (emplacementUI == UIEmplacement.UpperLeft || emplacementUI == UIEmplacement.BottomLeft)? 1f:-1f;

        snowBallsLocation = new List<Vector3>{gameObject.transform.position + new Vector3(iconsOffset*1f , .25f , 00),
                                            gameObject.transform.position + new Vector3(iconsOffset*1.5f , .25f , 00),
                                            gameObject.transform.position + new Vector3(iconsOffset*2f , .25f , 00),
                                            gameObject.transform.position + new Vector3(iconsOffset*1f , -.25f , 00),
                                            gameObject.transform.position + new Vector3(iconsOffset*1.5f , -.25f , 00),
                                            gameObject.transform.position + new Vector3(iconsOffset*2f , -.25f , 00)};

        for (int ammoIndex = 0 ; ammoIndex < AMMO_MAX ; ammoIndex++)
        {
            ammos[ammoIndex] = Instantiate(ammoPrefab , snowBallsLocation[ammoIndex] , Quaternion.identity, gameObject.transform);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        for(int indexEnable = 1 ; indexEnable < playerAttachedTo.GetComponent<PlayerMovement>().currentAmmoStock+1 ; indexEnable++)
        {
            ammos[indexEnable-1].GetComponent<SpriteRenderer>().enabled = true;
        }
        for(int indexDisable = playerAttachedTo.GetComponent<PlayerMovement>().currentAmmoStock ; indexDisable <= AMMO_MAX-1 ; indexDisable++)
        {
            ammos[indexDisable].GetComponent<SpriteRenderer>().enabled = false;
        }        
    }
}
