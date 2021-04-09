using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzleNode : MonoBehaviour {
    [System.Serializable]    
    public class RayInfo{
        public Vector3 startPoint;
        public Vector3 direction;
    }

    public List<RayInfo> rayInfos = new List<RayInfo>();

    [HideInInspector]
    public List<PipePuzzleNode> nodes = new List<PipePuzzleNode>();

    [SerializeField]
    private float rayDistance = 0.15f;
    private Transform myTransform;
    private LayerMask whatIsNode;

    bool isRotate = false;

    private void Start() {
        myTransform = transform;
        whatIsNode = gameObject.layer;

        CheckNearNodes();
    }

    private void OnDestroy() {
        myTransform = null;
        nodes = null;
        rayInfos = null;
    }

    void CheckNearNodes(){
        for (int i = 0; i < rayInfos.Count; i++){
            RaycastHit hit;

            Vector3 rayStartPoint = myTransform.rotation * (rayInfos[i].startPoint);
            Vector3 rayDirection = myTransform.rotation * rayInfos[i].direction;

            rayStartPoint = myTransform.position + rayStartPoint;

            if (Physics.Raycast(rayInfos[i].startPoint, rayInfos[i].direction, out hit, rayDistance, whatIsNode)){
                PipePuzzleNode near = hit.transform.GetComponent<PipePuzzleNode>();
                if (near){
                    nodes.Add(near);
                }
            }
        }
    }

    public IEnumerator SmoothRotateAndCheck(Vector3 normal, PipePuzzle.Rotate rotate){
        bool flag = isRotate;

        Vector3 cross = Vector3.zero;

        switch(rotate){
            case PipePuzzle.Rotate.Up:
                cross = Vector3.Cross(normal, Vector3.up);
                break;
            case PipePuzzle.Rotate.Down:
                cross = Vector3.Cross(normal, Vector3.down);
                break;
            case PipePuzzle.Rotate.Right:
                cross = Vector3.down;
                break;
            case PipePuzzle.Rotate.Left:
                cross = Vector3.up;
                break;
        }

        Quaternion targetQuaternion = Quaternion.AngleAxis(90f, cross) * transform.rotation;

        Debug.DrawLine(transform.position, transform.position + cross, Color.cyan, 2f);

        isRotate = true;

        yield return null;

        while (!flag){
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetQuaternion, 0.1f);

            if (Quaternion.Angle(myTransform.rotation, targetQuaternion) < 0.1f){
                myTransform.rotation = targetQuaternion;
                flag = true;
                isRotate = false;
            }
            yield return null;
        }

        CheckNearNodes();
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < rayInfos.Count; i++){
            Vector3 rayStartPoint = transform.rotation * (rayInfos[i].startPoint);
            Vector3 rayDirection = transform.rotation * rayInfos[i].direction;

            rayStartPoint = transform.position + rayStartPoint;

            Debug.DrawRay(rayStartPoint, rayDirection * rayDistance, Color.red);
        }
    }
}