using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject playerAttachedTo;
    public GameObject[] snowBallAmmos;
    private int AMMO_MAX;


    // Start is called before the first frame update
    void Start()
    {
        AMMO_MAX = playerAttachedTo.GetComponent<PlayerMovement>().currentAmmoStock;
    }

    // Update is called once per frame
    void Update()
    {

        //Note : penser a refaire la prefab avec le visage vide et le mettre suivant 1p. 2p etc. et le reste suivant la team (gauche ou droite)
        for(int indexEnable = 1 ; indexEnable < playerAttachedTo.GetComponent<PlayerMovement>().currentAmmoStock+1 ; indexEnable++)
        {
            snowBallAmmos[indexEnable-1].GetComponent<SpriteRenderer>().enabled = true;
        }
        for(int indexDisable = playerAttachedTo.GetComponent<PlayerMovement>().currentAmmoStock ; indexDisable <= AMMO_MAX-1 ; indexDisable++)
        {
            snowBallAmmos[indexDisable].GetComponent<SpriteRenderer>().enabled = false;
        }        
    }
}
