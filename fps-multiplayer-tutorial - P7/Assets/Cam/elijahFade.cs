using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elijahFade : MonoBehaviour
{
    //[SerializeField] GameObject objectOne;
    //[SerializeField] GameObject objectTwo;
    [SerializeField] Color objectOne;
    public bool isFadingOut;

    // Start is called before the first frame update
    void Start()
    {
        objectOne.a = 0;
        isFadingOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFadingOut)
        {
            objectOne.a += Time.deltaTime;
            if(objectOne.a == 1) 
            { 
                isFadingOut = true;
            }
        }

        if (isFadingOut)
        {
            objectOne.a -= Time.deltaTime;
            if (objectOne.a == 0)
            {
                isFadingOut = false;
            }
        }
        
    }
}
