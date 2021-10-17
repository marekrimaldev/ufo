using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animator>().Play("Clouds");
    }
}
