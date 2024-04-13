using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class elijahFade : MonoBehaviour
{
    [SerializeField] GameObject objectOne;
    [SerializeField] GameObject objectTwo;
    //[SerializeField] Color colorOne;
    //[SerializeField] Color colorTwo;
    //[SerializeField] Image imageOne;
    public bool isFadingOut;
    public float fadeOutTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //colorOne = objectOne.GetComponent<Image>().color;
        //colorTwo = objectTwo.GetComponent<Renderer>().material.color;
        objectOne.GetComponent<Image>().color = new Color(objectOne.GetComponent<Image>().color.r, objectOne.GetComponent<Image>().color.g, objectOne.GetComponent<Image>().color.b, 0);
        objectTwo.GetComponent<TextMeshProUGUI>().color = new Color(objectTwo.GetComponent<TextMeshProUGUI>().color.r, objectTwo.GetComponent<TextMeshProUGUI>().color.g, objectTwo.GetComponent<TextMeshProUGUI>().color.b, 0);
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
            objectOne.GetComponent<Image>().color = new Color(objectOne.GetComponent<Image>().color.r, objectOne.GetComponent<Image>().color.g, objectOne.GetComponent<Image>().color.b, objectOne.GetComponent<Image>().color.a + (Time.deltaTime * fadeOutTime));
            objectTwo.GetComponent<TextMeshProUGUI>().color = new Color(objectTwo.GetComponent<TextMeshProUGUI>().color.r, objectTwo.GetComponent<TextMeshProUGUI>().color.g, objectTwo.GetComponent<TextMeshProUGUI>().color.b, objectTwo.GetComponent<TextMeshProUGUI>().color.a + (Time.deltaTime * fadeOutTime));
            //colorTwo.a += Time.deltaTime;
            if (objectOne.GetComponent<Image>().color.a > .95f) 
            { 
                isFadingOut = true;
            }
        }

        if (isFadingOut)
        {
            objectOne.GetComponent<Image>().color = new Color(objectOne.GetComponent<Image>().color.r, objectOne.GetComponent<Image>().color.g, objectOne.GetComponent<Image>().color.b, objectOne.GetComponent<Image>().color.a - (Time.deltaTime * fadeOutTime));
            objectTwo.GetComponent<TextMeshProUGUI>().color = new Color(objectTwo.GetComponent<TextMeshProUGUI>().color.r, objectTwo.GetComponent<TextMeshProUGUI>().color.g, objectTwo.GetComponent<TextMeshProUGUI>().color.b, objectTwo.GetComponent<TextMeshProUGUI>().color.a - (Time.deltaTime * fadeOutTime));
            //colorTwo.a -= Time.deltaTime;
            if (objectOne.GetComponent<Image>().color.a < .05f)
            {
                isFadingOut = false;
            }
        }
        
    }
}
