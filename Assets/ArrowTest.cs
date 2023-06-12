using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTest : MonoBehaviour
{
    [Tooltip("Position we want to hit")]
    public GameObject target;

    public GameObject arrow;

    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 10;

    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 1;

    Vector3 targetPos;
    GameObject originalPos;
    GameObject newArrow;
    void Start()
    {
        // Cache our start position, which is really the only thing we need
        // (in addition to our current position, and the target).
        newArrow = GameObject.FindGameObjectWithTag("Ally");
        originalPos = GameObject.FindGameObjectWithTag("Tower");
        targetPos = target.transform.position;
    }

    void Update()
    {
        
        // Compute the next position, with arc added in
        float x0 = transform.position.x;
        float x1 = targetPos.x;
        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(newArrow.transform.position.x, x1, speed * Time.deltaTime);
        float baseY = Mathf.Lerp(transform.position.y, targetPos.y, (nextX - x0) / dist);
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
        Vector3 nextPos = new Vector3(nextX, baseY + arc, newArrow.transform.position.z);

        // Rotate to face the next position, and then move there
        newArrow.transform.rotation = LookAt2D(nextPos - newArrow.transform.position);
        newArrow.transform.position = nextPos;

        // Do something when we reach the target
        if (nextPos == targetPos)
        {
            Arrived();
        }
    }

    void Arrived()
    {
        //Destroy(newArrow);
    }

    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
}
