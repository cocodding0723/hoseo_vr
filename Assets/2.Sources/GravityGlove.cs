using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GravityGlove : ControllerGrabObject
{
    [SerializeField]
    LayerMask whatIsSelectableObject;

    protected override void Update() {
        base.Update();
        
    }

    protected override void TriggerDown()
    {
        base.TriggerDown();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 20f, whatIsSelectableObject)){
            hit.transform.GetComponent<Rigidbody>().velocity = GetVelocity(hit.transform.position, transform.position, 30f);
        }
    }

    // 물체에 가할 힘
    // 참고 : https://chanuklee0227.blogspot.com/2017/02/unity.html
    Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;
    
        Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);
    
        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = currentPos.y - targetPos.y;
    
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
    
        Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
    
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
    
        return finalVelocity;
    }
}
