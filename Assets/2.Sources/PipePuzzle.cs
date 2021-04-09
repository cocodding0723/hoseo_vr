using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzle : MonoBehaviour
{
    public enum Rotate{
        Up, Down, Right, Left
    }

    public PipePuzzleNode startNode;

    public bool isClear;

    [SerializeField]
    LayerMask whatIsNode;

    RaycastHit hit;

    PipePuzzleNode currentNode = null;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
        if (Input.GetMouseButtonDown(0)){
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, whatIsNode)){
                currentNode = hit.transform.GetComponent<PipePuzzleNode>();
            }
        }

        if (currentNode){
            if (Input.GetKeyDown(KeyCode.W)){
                StartCoroutine(currentNode.SmoothRotateAndCheck(hit.normal, Rotate.Up));
            }
            if (Input.GetKeyDown(KeyCode.A)){
                StartCoroutine(currentNode.SmoothRotateAndCheck(hit.normal, Rotate.Left));
            }
            if (Input.GetKeyDown(KeyCode.S)){
                StartCoroutine(currentNode.SmoothRotateAndCheck(hit.normal, Rotate.Down));
            }
            if (Input.GetKeyDown(KeyCode.D)){
                StartCoroutine(currentNode.SmoothRotateAndCheck(hit.normal, Rotate.Right));
            }
        }

        if (Input.GetMouseButtonUp(0)){
            currentNode = null;
        }
    }

    void CheckPuzzleClear(){
        
    }
}
