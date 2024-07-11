using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypoint = 0;
    [SerializeField] private float speed = 1.0f;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.y > this.transform.position.y)
            {
                isDead = true;
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 0.1f)
            {
                currentWaypoint = (++currentWaypoint) % waypoints.Length;
            }
        }
    }
}
