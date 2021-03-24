using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPuzzle : MonoBehaviour
{
    public bool isClear{ get { return clear;}}
    bool clear = false;

    [SerializeField]
    LayerMask whatIsKeys;

    [SerializeField]
    GraphPuzzleNode[] nodes;

    [SerializeField]
    Transform[] keys;

    [SerializeField]
    Color changeKeyColor;
    Color originKeyColor;

    Dictionary<Transform, Material> keyMaterialDictionary = new Dictionary<Transform, Material>();

    private void Start() {
        InitAll();
        originKeyColor = keys[0].GetComponent<MeshRenderer>().material.color;

        for (int i = 0; i < keys.Length; i++){
            keyMaterialDictionary.Add(keys[i], keys[i].GetComponent<MeshRenderer>().material);
        }
    }
    
    private void Update() {
        List<RaycastHit> hitList = new List<RaycastHit>();

        DrawCallAll();
        RaycastAllNodes(ref hitList);
        ResetKeyColor();
        HitKeyColorChange(ref hitList);
    }

    private void InitAll(){
        for (int i = 0; i < nodes.Length; i++){
            nodes[i].Init();
        }
    }

    private void DrawCallAll(){
        for (int i = 0; i < nodes.Length; i++){
            nodes[i].DrawLine();
        }
    }

    private void RaycastAllNodes(ref List<RaycastHit> _hitList){
        for (int i = 0; i < nodes.Length; i++){
            RaycastHit[] hits;
            Vector3 directionA = nodes[i].movePoint.position - nodes[i].staticPointA.position;
            float distanceA = Vector3.Distance(nodes[i].movePoint.position, nodes[i].staticPointA.position);

            Vector3 directionB = nodes[i].movePoint.position - nodes[i].staticPointB.position;
            float distanceB = Vector3.Distance(nodes[i].movePoint.position, nodes[i].staticPointB.position);

            Debug.DrawRay(nodes[i].staticPointA.position, directionA);
            hits = Physics.RaycastAll(nodes[i].staticPointA.position, directionA, distanceA, whatIsKeys);
            _hitList.AddRange(hits);

            Debug.DrawRay(nodes[i].staticPointB.position, directionB);
            hits = Physics.RaycastAll(nodes[i].staticPointB.position, directionB, distanceB, whatIsKeys);
            _hitList.AddRange(hits);
        }

        if (_hitList.Count == keys.Length){
            Debug.Log("Clear");
        }
    }

    private void ResetKeyColor(){
        for (int i = 0; i < keys.Length; i++){
            keyMaterialDictionary[keys[i].transform].color = originKeyColor;
        }
    }

    private void HitKeyColorChange(ref List<RaycastHit> _hitList){
        for (int i = 0; i < _hitList.Count; i++){
            keyMaterialDictionary[_hitList[i].transform].color = changeKeyColor;
        }
    }
}
