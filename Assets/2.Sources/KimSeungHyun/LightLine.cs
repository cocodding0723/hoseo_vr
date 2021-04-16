using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLine : MonoBehaviour
{
    Transform startTransform = null;
    [SerializeField]
    Transform firstMirrorPosition = null;
    public LineRenderer mirrorLineRenderer = null;
    RaycastHit mirrorHit;
    public bool isOn = false;
    bool isClear = false;
    int hitCount = 1;
    [SerializeField]
    public LayerMask mirrorLayerMask;
    List<Vector3> mirrorVec = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        startTransform = this.transform;
        // mirrorLayerMask = 1 << LayerMask.NameToLayer("Mirror");
        mirrorLineRenderer = transform.GetComponent<LineRenderer>();
        mirrorLineRenderer.SetPosition(0, startTransform.position);
        mirrorLineRenderer.SetPosition(1, startTransform.position);
        mirrorLayerMask =  (1 << LayerMask.NameToLayer("Particle"));
        mirrorLayerMask = ~mirrorLayerMask;
    }

    // Update is called once per frame
    void Update()
    {
        CheckLineLight(startTransform.position,firstMirrorPosition.position - startTransform.position);
    }
    private void LateUpdate()
    {
        DrawLightLine();
    }

    void DrawLightLine()
    {
        if(isClear == false)
        {
            for (int i = 0; i <= hitCount - 2; i++)
            {
                mirrorLineRenderer.SetPosition(i + 1, mirrorVec[i]);
            }
            mirrorVec.Clear();
            hitCount = 1;
        }

    }
    void GameClear()
    {
        isClear = true;
    }
    void CheckLineLight(Vector3 hitPoint, Vector3 Dir)
    {
        //Debug.DrawRay(hitPoint, Dir, Color.red);
        if (Physics.Raycast(hitPoint, Dir, out mirrorHit))
        {
            Debug.Log(mirrorHit.collider);
            if(mirrorHit.collider.tag == "Mirror")
            {
                
                Vector3 nextDir = Vector3.Reflect(Dir, mirrorHit.normal);
                if (!mirrorVec.Contains(mirrorHit.point))
                {
                    mirrorVec.Add(mirrorHit.point);
                    hitCount++;
                    mirrorLineRenderer.SetVertexCount(hitCount+1);
                    mirrorLineRenderer.SetPosition(hitCount, hitPoint);
                    Debug.Log("리스트저장");
                }
                if (hitCount == 6 && mirrorHit.collider.tag == "ClearCapsule")
                {
                    GameClear();
                }
                    //Debug.Log(mirrorHit.collider.gameObject);
                    CheckLineLight(mirrorHit.point, nextDir);
            }
            else
            {
                //Debug.DrawLine(hitPoint, mirrorHit.point, Color.green);
                mirrorLineRenderer.SetPosition(hitCount, mirrorHit.point);
                print("NotMirrorHit");
            }
        }
        else
        {
            print("eslse");
            mirrorLineRenderer.SetPosition(hitCount, hitPoint + Dir* 100f);
        }
    }
}
