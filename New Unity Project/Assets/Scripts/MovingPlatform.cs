using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] positions;
    

    private int targetPosition = 1;
    private bool changingPosition = false;
        
    void Update()
    {
        if (changingPosition)
            return;

        Vector3 move = Vector3.zero;
        Transform target = positions[targetPosition];
        move.x = target.position.x - transform.position.x;
        move.y = target.position.y - transform.position.y;

        if (Mathf.Abs(move.x) < 0.025 && Mathf.Abs(move.y) < 0.025)
        {   
                changingPosition = true;
                Invoke("ChangePosition", 1f);            
        }

        move.Normalize();
        transform.position = transform.position + (move * Time.deltaTime);
        
    }

    void ChangePosition()
    {        
        targetPosition = (targetPosition + 1) % positions.Length;
        changingPosition = false;
    }
}
