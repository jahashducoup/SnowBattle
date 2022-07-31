using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InfoBox : MonoBehaviour
{
    private const float DELAYTODESTROY = 2f;
    public GameObject parent;
    
    private Vector3 offSetWithParent;
    // Start is called before the first frame update
    void Start()
    {
        offSetWithParent = new Vector3(.3f, .8f, 0);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(Blinking());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.transform.position + offSetWithParent;
    }

    private IEnumerator Blinking()
    {
        Destroy(gameObject, DELAYTODESTROY);
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.5f);
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
