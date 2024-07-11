using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private float verticalOffset;
    private float horizontalOffset;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetParent(Player.transform);
        verticalOffset = Player.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        horizontalOffset = Player.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = Player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
    }
}
