using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class pickupscript : MonoBehaviour
{
    public float roationSpeed = 50f;
    float rotation = 0f;
    
    private void Start()
    {
        rotation += Random.Range(0f, 360f);
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        child.transform.localRotation = Quaternion.Euler(child.transform.localRotation.eulerAngles.x, child.transform.localRotation.eulerAngles.y, rotation);
    }
    
    void Update()
    {
        rotation += roationSpeed; 
        
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        child.transform.localRotation = Quaternion.Euler(child.transform.localRotation.eulerAngles.x, child.transform.localRotation.eulerAngles.y, rotation);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            score.Add();
            Destroy(this.gameObject);
        }
    }
}
