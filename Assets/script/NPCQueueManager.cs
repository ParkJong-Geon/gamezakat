using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQueueManager : MonoBehaviour
{
    public NPCController npc;

    public Transform spawnPoint;
    public Transform area1;
    public Transform servicePoint;
    public Transform exitPoint;
    // Start is called before the first frame update
    private void Start()
    {
        npc.transform.position = spawnPoint.position;
             // Jalan:
        // SpawnPoint → Area1 → ServicePoint
        npc.MoveThroughPoints(
            area1.position,
            servicePoint.position
        );
    }


    // Update is called once per frame
    private void Update()
    {
        if(!npc.IsMoving())
        {
            if(Input.GetKeyDown(KeyCode.Space)){
                npc.MoveThroughPoints(exitPoint.position);
            }
        }
    }
}
