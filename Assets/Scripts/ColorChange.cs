using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    SpriteRenderer sr;
    float speed= 1f;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = new Color(sr.color.r + (speed * Time.deltaTime), sr.color.g, sr.color.b);
        sr.gameObject.SetActive(false);
        sr.gameObject.SetActive(true);
    }
}
