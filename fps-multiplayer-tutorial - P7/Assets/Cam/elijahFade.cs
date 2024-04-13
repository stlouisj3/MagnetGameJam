using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elijahFade : MonoBehaviour
{
    [SerializeField] GameObject objectOne;
    [SerializeField] GameObject objectTwo;
    [SerializeField] Color colorOne;
    [SerializeField] Color colorTwo;
    public bool isFadingOut;

    // Start is called before the first frame update
    void Start()
    {
        colorOne = objectOne.GetComponent<Renderer>().material.color;
        colorTwo = objectTwo.GetComponent<Renderer>().material.color;
        colorOne.a = 0;
        colorTwo.a = 0;
        isFadingOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFadingOut)
        {
            colorOne.a += Time.deltaTime;
            colorTwo.a += Time.deltaTime;
            if (colorOne.a == 1) 
            { 
                isFadingOut = true;
            }
        }

        if (isFadingOut)
        {
            colorOne.a -= Time.deltaTime;
            colorTwo.a -= Time.deltaTime;
            if (colorOne.a == 0)
            {
                isFadingOut = false;
            }
        }
        
    }
}
