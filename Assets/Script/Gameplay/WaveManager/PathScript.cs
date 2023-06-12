using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    public int targetNumber = 0;
    private Transform target;
    private Animator animator;
    public Transform[] checkpoints;
    public Transform exitPoint;
    public float navTimeUpdate;
    public float currentNavTime;
    public float moveSpeed;

    private bool canIncreaseTargetNumber = true;

    private void Start()
    {
        target = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (checkpoints != null)
        {
            currentNavTime += Time.deltaTime * moveSpeed;

            if (currentNavTime > navTimeUpdate)
            {
                if (targetNumber < checkpoints.Length)
                {
                    target.position = Vector2.MoveTowards(target.position, checkpoints[targetNumber].position, currentNavTime);
                }
                else
                {
                    target.position = Vector2.MoveTowards(target.position, exitPoint.position, currentNavTime);
                }
                currentNavTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("checkpoint"))
        {
            if (canIncreaseTargetNumber) 
            {
                checkPointPaths(other.gameObject);
                canIncreaseTargetNumber = false; 
            }
        }
        else if (other.CompareTag("checkpoint02"))
        {
            if (canIncreaseTargetNumber)
            {
                checkPointPaths(other.gameObject);
                canIncreaseTargetNumber = false;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("checkpoint") || other.CompareTag("checkpoint02"))
        {
            canIncreaseTargetNumber = true;
        }
    }

    private void checkPointPaths(GameObject checkpoint)
    {
        if (checkpoint != null)
        {
            if (checkpoint.name.Contains("UpPoint"))
            {
                animator.SetBool("isUpper", true);
                animator.SetBool("isDown", false);
            }
            else if (checkpoint.name.Contains("DownPoint"))
            {
                animator.SetBool("isDown", true);
                animator.SetBool("isUpper", false);
            }
            else
            {
                animator.SetBool("isDown", false);
                animator.SetBool("isUpper", false);
            }
            targetNumber += 1;
        }
    }


}