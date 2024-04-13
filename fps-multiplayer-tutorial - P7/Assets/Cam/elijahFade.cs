using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class elijahFade : MonoBehaviour
{
    [SerializeField] GameObject objectOne;
    //[SerializeField] GameObject objectTwo;
    [SerializeField] Color colorOne;
    //[SerializeField] Color colorTwo;
    [SerializeField] Image imageOne;
    public bool isFadingOut;

    // Start is called before the first frame update
    void Start()
    {
        colorOne = objectOne.GetComponent<Image>().color;
        //colorTwo = objectTwo.GetComponent<Renderer>().material.color;
        colorOne.a = 0f;
        //colorTwo.a = 0;
        isFadingOut = false;

        //imageOne = objectOne.GetComponent<Image>();

        //imageOne.color.a = 0;
        //imageTwo = objectTwo.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFadingOut)
        {
            //imageOne.color.a += Time.deltaTime;
            colorOne.a += Time.deltaTime;
            //colorTwo.a += Time.deltaTime;
            if (colorOne.a > .95f) 
            { 
                isFadingOut = true;
            }
        }

        if (isFadingOut)
        {
            colorOne.a -= Time.deltaTime;
            //colorTwo.a -= Time.deltaTime;
            if (colorOne.a < .05f)
            {
                isFadingOut = false;
            }
        }
        
    }
}
