using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float moveSpeed = 2f;

    public Transform groundPoint;
    
    private Vector3[] waypoints;
    private int currentWaypoint = 0;
    private bool isMoving = false;

    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
    }
    
    public void MoveThroughPoints(params Vector3[] points)
    {
        if(points == null || points.Length == 0)
        return;

        waypoints = points;
        currentWaypoint = 0;
        isMoving = true;

        animator.SetBool("IsWalking", true);
    }   

    public void SetGroundPosition(Vector3 targetPosition){
        if(groundPoint == null){
            Debug.LogWarning(gameObject.name +  "tidak ada ground");
            transform.position = targetPosition;
            return;
        }

        Vector3 offset= targetPosition - groundPoint.position;
        transform.position += offset;
    }


    // Update is called once per frame
    private void Update()
    {
        if(!isMoving)
        return;

        Vector3 targetPosition = waypoints[currentWaypoint];

        // transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 movement = Vector3.MoveTowards(groundPoint.position, targetPosition, moveSpeed *Time.deltaTime);

        Vector3 offset = movement - groundPoint.position;
        transform.position += offset;
        //sampai waypoint
        if (Vector3.Distance(groundPoint.position, targetPosition) < 0.01f) {
            SetGroundPosition( targetPosition);
            currentWaypoint++;

            if(currentWaypoint >= waypoints.Length)
            {

            isMoving = false;

            animator.SetBool("IsWalking", false);
            }
        }

    }

    public bool IsMoving(){
        return isMoving;
    }
}
