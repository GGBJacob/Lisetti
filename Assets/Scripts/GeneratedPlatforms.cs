using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    public GameObject platformPrefab;
    public float speed = 3.0f;
    const int PLATFORMS_NUM = 6;
    const int POSITIONS_NUM = 6;
    int positionOffset = 0;
    GameObject[] platforms;
    Vector3[] positions;

    private void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[POSITIONS_NUM];
        int radius = 10;
        float angle = 90;
        for(int i=0; i<PLATFORMS_NUM; i++) 
        {
            float x= Mathf.Sin(Mathf.Deg2Rad*angle)*radius + transform.position.x, y= Mathf.Cos(Mathf.Deg2Rad * angle) * radius+transform.position.y;
            positions[i] = new Vector2(x, y);
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
            platforms[i].transform.SetParent(this.transform);
            platforms[i].tag = ("MovingPlatform");
            platforms[i].AddComponent<PolygonCollider2D>();
            BoxCollider2D platformBoxCollider = platforms[i].GetComponent<BoxCollider2D>();
            float ySize = platformBoxCollider.size.y;
            platformBoxCollider.isTrigger = true;
            platformBoxCollider.size = new Vector2(platformBoxCollider.size.x * 0.8f, platformBoxCollider.size.y * 0.3f);
            platformBoxCollider.offset = new Vector2(0,ySize/2);
            angle += (360f / PLATFORMS_NUM);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < PLATFORMS_NUM; i++)
        { 
            platforms[i].transform.position = Vector2.MoveTowards(platforms[i].transform.position, positions[(i+1+positionOffset)%POSITIONS_NUM], speed * Time.deltaTime);
        }

        //Debug.Log(Vector2.Distance(platforms[0].transform.position, positions[(positionOffset + 1) % POSITIONS_NUM]));

        if (Vector2.Distance(platforms[0].transform.position, positions[(positionOffset+1)%POSITIONS_NUM]) < 0.1f)
        {
            positionOffset++;
        }
    }
}
