using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GravityGlove : ControllerGrabObject
{
    [SerializeField]
    LayerMask whatIsSelectableObject;

    [SerializeField]
    float throwMin = 30f;
    [SerializeField]
    float throwMax = 90f;

    [Tooltip("작동되는 최소 각도")]
    [SerializeField]
    float activeMinAngle = 20f;

    LineRenderer lineRenderer;
    int lineScale = 30;

    Transform controllerTransform;
    RaycastHit hit;
    GameObject selectObject;
    float prevAngle = 0f;
    float senetivity = 10f;
    float current_rot_x = 0f;

    void Start()
    {
        controllerTransform = transform;
        selectObject = null;
        lineRenderer = GetComponentInChildren<LineRenderer>();

    }

    private void OnEnable() {
        InputManager.Instance.trigger.onState += Trigger;
        InputManager.Instance.trigger.onStateDown += TriggerDown;
        InputManager.Instance.trigger.onStateUp += TriggerUp;
    }

    private void OnDisable() {
        InputManager.Instance.trigger.onState += Trigger;
        InputManager.Instance.trigger.onStateDown += TriggerDown;
        InputManager.Instance.trigger.onStateUp -= TriggerUp;
    }

    public void TransformUpdate(SteamVR_Behaviour_Pose currentPose, SteamVR_Input_Sources sources){
        controllerTransform = currentPose.transform;

        Debug.Log(currentPose.inputSource);
    }

    private void Trigger(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource){
        if (controllerTransform.eulerAngles.x - activeMinAngle > activeMinAngle && selectObject){
            Vector3 selectObjPos = selectObject.transform.position;
            Vector3 controllerTransformPos = controllerTransform.position;
            
            selectObject.GetComponent<Rigidbody>().velocity = GetVelocity(selectObjPos, controllerTransformPos, GetThrowAngle(selectObjPos, controllerTransformPos));
            selectObject = null;

            // DrawLineToBezierCurve(transform.forward, selectObject.transform.position);
        }
    }

    private void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource){
        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, 10f, whatIsSelectableObject)){
            selectObject = hit.transform.gameObject;
            prevAngle = controllerTransform.eulerAngles.x;
            StartCoroutine(UpdatePreAngle());

            lineRenderer.enabled = true;
        }
    }

    private void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource){
        selectObject = null;
        prevAngle = 999f;
        StopCoroutine(UpdatePreAngle());

        Debug.Log("EE");
        
        // lineRenderer.enabled = false;
    }


    // protected override void Trigger(){
    //     base.Trigger();
    //     if (controllerTransform.eulerAngles.x - activeMinAngle > activeMinAngle && selectObject){
    //         Vector3 selectObjPos = selectObject.transform.position;
    //         Vector3 controllerTransformPos = controllerTransform.position;
            
    //         selectObject.GetComponent<Rigidbody>().velocity = GetVelocity(selectObjPos, controllerTransformPos, GetThrowAngle(selectObjPos, controllerTransformPos));
    //         selectObject = null;

    //         DrawLineToBezierCurve(transform.forward, selectObject.transform.position);
    //     }
    // }

    // protected override void TriggerDown(){
    //     base.TriggerDown();
    //     if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, 10f, whatIsSelectableObject)){
    //         selectObject = hit.transform.gameObject;
    //         prevAngle = controllerTransform.eulerAngles.x;
    //         StartCoroutine(UpdatePreAngle());

    //         lineRenderer.enabled = true;
    //     }
    // }

    // protected override void TriggerUp(){
    //     base.TriggerUp();
    //     selectObject = null;
    //     prevAngle = 999f;
    //     StopCoroutine(UpdatePreAngle());
        
    //     lineRenderer.enabled = false;
    // }

    IEnumerator UpdatePreAngle(){
        while(true){
            yield return new WaitForSeconds(1f);
            prevAngle = controllerTransform.eulerAngles.x;
        }
    }

    void DrawLineToBezierCurve(Vector3 start, Vector3 end){
        Vector3 middlePoint = new Vector3(end.x, start.y, end.z);
        float t = 0f;
        float addValue = 1f / lineScale;
        Vector3[] points = new Vector3[lineScale];
        for (int i = 0; i < lineScale; i++){
            points[i] = BezierCurve.GetPointOnBezierCurve(start, middlePoint, end, t);
            t += addValue;
        }

        lineRenderer.SetPositions(points);
    }

    float GetThrowAngle(Vector3 currentPos, Vector3 targetPos){
        float throwAngle = 0f;

        Vector3 currentPosToPlane = new Vector3(currentPos.x, 0f, currentPos.z);  // y축이 0값인 controllerTransform 벡터 생성
        Vector3 targetPosToPlane = new Vector3(targetPos.x, 0f, targetPos.z);     // y축이 0값인 selectObject 벡터 생성
        Vector3 planeDirection = targetPosToPlane - currentPosToPlane;      // 평면 방향 벡터 생성
        Vector3 velocityDirection = targetPos - currentPos;         // 방향 벡터 생성

        throwAngle = Vector3.Angle(planeDirection.normalized, velocityDirection.normalized);  // 초기 각도 설정
        throwAngle = Mathf.Clamp(throwAngle + 20f, throwMin, throwMax);                       // 각도 보정

        return throwAngle;
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
