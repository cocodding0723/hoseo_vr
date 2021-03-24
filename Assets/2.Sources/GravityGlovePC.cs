
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGlovePC : MonoBehaviour
{

    [SerializeField]
    LayerMask whatIsSelectableObject;

    [SerializeField]

    Vector3[] bezierLine;

    Transform myTransform;
    RaycastHit hit;
    GameObject selectObject;
    float prevAngle = 0f;

    int lineScale = 30;

    void Start()
    {
        myTransform = transform;
        selectObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Debug.DrawRay(transform.position, transform.forward * 10f);
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, whatIsSelectableObject)){
                selectObject = hit.transform.gameObject;
                prevAngle = myTransform.eulerAngles.x;
                StartCoroutine(UpdatePreAngle());
            }
        }
        else if (Input.GetMouseButton(0)){
            if (myTransform.eulerAngles.x - prevAngle > 20f && selectObject){
                selectObject.GetComponent<Rigidbody>().velocity = GetVelocity(selectObject.transform.position, myTransform.position, 30f);
            }
        }
        else if (Input.GetMouseButtonUp(0)){
            selectObject = null;
            prevAngle = 999f;
            StopCoroutine(UpdatePreAngle());
        }
    }

    IEnumerator UpdatePreAngle(){
        while(true){
            yield return new WaitForSeconds(1f);
            prevAngle = myTransform.eulerAngles.x;
        }
    }

    void DrawLineToBezierCurve(Vector3 start, Vector3 end){
        Vector3 middlePoint = Vector3.Lerp(start, end, 0.5f);
        middlePoint += Vector3.up;
        float t = 0f;
        float addValue = 1f / lineScale;
        Vector3[] points = new Vector3[lineScale];
        for (int i = 0; i < lineScale; i++){
            points[i] = BezierCurve.GetPointOnBezierCurve(start, middlePoint, end, t);
            t += addValue;
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
