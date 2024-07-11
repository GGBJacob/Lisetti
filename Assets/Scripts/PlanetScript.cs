using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    Vector3 startingPosition;
    Vector3 originalSize;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        originalSize = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: "+collision.tag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Stopped colliding with: " +collision.tag);
        if (collision.CompareTag("MainCamera"))
        {
            Debug.Log(transform.position);
            transform.position = startingPosition;
            transform.localScale = originalSize;
            PlanetRotatingManager.instance.NextPlanet();
        }
    }

    //private void OnBecameInvisible()
    //{
    //    Debug.Log(transform.position);
    //    transform.position = startingPosition;
    //    transform.localScale = new Vector3(1, 1, 1);
    //    PlanetRotatingManager.instance.NextPlanet();
    //}
    void Update()
    {
        
    }
}
