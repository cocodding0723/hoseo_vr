using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GraphPuzzleNode : MonoBehaviour
{
    public Transform staticPointA;
    public Transform staticPointB;
    public Transform movePoint;

    LineRenderer lineRenderer;

    public void Init() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
    }

    public void DrawLine(){
        lineRenderer.SetPosition(0, staticPointA.position);
        lineRenderer.SetPosition(1, movePoint.position);
        lineRenderer.SetPosition(2, staticPointB.position);
    }
}
