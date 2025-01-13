using System;
using Unity.VisualScripting;
using UnityEngine;

public class lamp : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        } 
    }
}
