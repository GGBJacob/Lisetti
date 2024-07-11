using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
public class PlanetRotatingManager : MonoBehaviour
{
    public static PlanetRotatingManager instance;
    public float variablesScale = 1.0f;
    public float XAxisSpeed;
    public float ZoomSpeed;
    public float delayTime=3f;
    [SerializeField]private GameObject[] PlanetsArray;
    public float currentXSpeed;
    public enum ZoomDirection {IN,OUT}
    public ZoomDirection direction = ZoomDirection.IN;
    private int planetIndex = 0;
    private bool switchingPlanets = false;
    //private Rigidbody2D rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentXSpeed = XAxisSpeed;
        planetIndex = UnityEngine.Random.Range(0, PlanetsArray.Length);
        //rigidbody = PlanetsArray[0].GetComponent<Rigidbody2D>();
    }

    void ZoomOut(GameObject planet)
    {
        planet.transform.position = new Vector3(planet.transform.position.x-XAxisSpeed*variablesScale*Time.deltaTime, planet.transform.position.y, planet.transform.position.z);
        planet.transform.localScale = new Vector3(planet.transform.localScale.x- (ZoomSpeed * variablesScale * Time.deltaTime), planet.transform.localScale.y - (ZoomSpeed * variablesScale * Time.deltaTime), planet.transform.localScale.z - (ZoomSpeed * variablesScale * Time.deltaTime));
    }

    void ZoomIn(GameObject planet, float Xspeed, float scaleSpeed)
    {
        //rigidbody.MovePosition(new Vector2(planet.transform.position.x + Xspeed * Time.deltaTime * variablesScale, planet.transform.position.y));
        //planet.transform.Translate(Xspeed * Time.deltaTime * variablesScale, 0.0f, 0.0f,Space.World);
        planet.transform.position = new Vector3(planet.transform.position.x + Xspeed * Time.deltaTime * variablesScale, planet.transform.position.y, planet.transform.position.z);
        planet.transform.localScale = new Vector3(planet.transform.localScale.x + (scaleSpeed * Time.deltaTime * variablesScale), planet.transform.localScale.y + (scaleSpeed * Time.deltaTime * variablesScale), planet.transform.localScale.z + (scaleSpeed * Time.deltaTime * variablesScale));
    }

    public void NextPlanet()
    {
        switchingPlanets = true;
        currentXSpeed = XAxisSpeed;
        //rigidbody = PlanetsArray[planetIndex % PlanetsArray.Length].GetComponent<Rigidbody2D>();
        if (instance.gameObject.activeInHierarchy)// NAPRAWIA B£¥D!
        {
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        switchingPlanets = false;
        planetIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!switchingPlanets)
        { 
            GameObject currentPlanet = PlanetsArray[planetIndex % PlanetsArray.Length];


            if (direction == ZoomDirection.IN)
            {
                if(currentPlanet.transform.position.x<0 && currentXSpeed<-2.2f && currentPlanet.name!="Saturn")
                {
                    currentXSpeed -= 0.0006f*XAxisSpeed;
                }
                ZoomIn(currentPlanet,currentXSpeed,ZoomSpeed);

            }
            else if (direction == ZoomDirection.OUT)
                ZoomOut(currentPlanet);
        }
    }
}
