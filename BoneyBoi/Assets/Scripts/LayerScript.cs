using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is a quick fix for a layer problem
//it keeps the box in object layer


public class LayerScript : MonoBehaviour
{
    void Update()
    {
        this.gameObject.layer = 8;
    }
}
