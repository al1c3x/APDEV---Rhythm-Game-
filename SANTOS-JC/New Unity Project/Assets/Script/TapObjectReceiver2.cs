using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapObjectReceiver2 : MonoBehaviour, Itap
{
    // Start is called before the first frame update
    public void Ontap()
    {
        Destroy(gameObject);
    }
}
