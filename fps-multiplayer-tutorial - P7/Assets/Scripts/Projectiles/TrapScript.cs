using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static Fusion.NetworkCharacterController;

public class TrapScript : NetworkBehaviour
{
    // Start is called before the first frame update


    private void OnTriggerEnter(Collider coll)
    {
        print(coll.gameObject.name);
        if (coll.gameObject.GetComponent<HPHandler>() != null)
        {
            coll.gameObject.GetComponent<HPHandler>().OnTakeDamage("Trap", 100);
        }
    }

    
}
