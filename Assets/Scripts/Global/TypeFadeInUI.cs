using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypeFadeInUI : MonoBehaviour
{
    private TextMeshProUGUI textMP;
    private Color32 color;
    public float timeToAppear = 2f;
    public float timeBeforeStart = 0;


    // Start is called before the first frame update
    void Start()
    {
        textMP = gameObject.GetComponent<TextMeshProUGUI>();
        color = Color.white;
        color.a = 0x01;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBeforeStart >= .1f)
        {
            timeBeforeStart -= Time.deltaTime;
        }
        if(color.a <= 0xFE && timeBeforeStart <= .1f)
        {
            color.a += 0x01;
            StartCoroutine(MakeTMPAppear(color, timeToAppear));
        }
    }
    
    private IEnumerator MakeTMPAppear(Color32 color, float timer)
    {
        textMP.GetComponent<TextMeshProUGUI>().color = color;
        yield return new WaitForSeconds(timer/255);
    }
}
