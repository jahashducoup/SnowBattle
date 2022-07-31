using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLoader : MonoBehaviour
{
    public GameObject fillBar;
    public GameObject pivot;
    private const float TIMETOLOAD = 1f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        pivot.transform.localScale += new Vector3(TIMETOLOAD * Time.deltaTime, 0f, 0f);

        if(pivot.transform.localScale.x >= .95f)
        {
            Destroy(gameObject);
        }
    }
}
