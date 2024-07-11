using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float fallDelay = 0.3f;
    private float fallTime = 2f;
    private float respawnTime = 15f;
    //private float destroyDelay = 2f;
    private Vector3 originalPos;
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        originalPos = this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(fallTime);
        rb.bodyType = RigidbodyType2D.Static;
        StartCoroutine(Respawn());
        //Destroy(gameObject, destroyDelay);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        GetComponent<BoxCollider2D>().enabled = true;
        this.transform.position = originalPos;
    }

}
