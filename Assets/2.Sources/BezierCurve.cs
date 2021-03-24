using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    // 2차 베지어 곡선
    public static Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 d = Vector3.Lerp(a, b, t);
    
        return d;
    }

    // 3차 베지어 곡선
    public static Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        Vector3 pointOnCurve = Vector3.Lerp(d, e, t);
    
        return pointOnCurve;
    }

    // 변형
    // 제작중
    public Vector3[] GetBezierPoints(Vector3[] points, float t){
        Vector3[] results = new Vector3[points.Length];

        if (points.Length > 3){
            Debug.LogError("3차부터 연산 가능");
            return null;
        }
        
        for (int i = 0; i < points.Length - 1; i++){

        }

        return results;
    }

}
