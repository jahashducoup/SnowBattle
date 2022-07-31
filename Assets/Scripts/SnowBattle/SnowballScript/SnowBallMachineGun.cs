using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallMachineGun : MonoBehaviour
{
    public GameObject snowballPrefab;

    private const float SHOT_FREQ = 2f;
    // Start is called before the first frame update

    private float shotCd = SHOT_FREQ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shotCd <= 0.1f)
        {
            Shot();
            shotCd = SHOT_FREQ;
        }
        else
        {
            shotCd -= Time.deltaTime;
        }
    }

    void Shot()
    {
        GameObject snowball = Instantiate(snowballPrefab, transform.position + new Vector3(-0.2f, 0, 0) , transform.rotation);
        snowball.GetComponent<SnowBallMovement>().masterSpawnPosition = transform.position;
        snowball.GetComponent<SnowBallMovement>().SpawnedBy = "MachineGun";
    }
}
