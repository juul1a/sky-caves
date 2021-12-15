using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door thisDoor;
    // Start is called before the first frame update
    void Awake()
    {
        thisDoor = this;
    }

    public void Enable(){
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
