using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzle : MonoBehaviour
{
    public enum Rotate{
        Up, Down, Right, Left
    }

    public bool isClear;
    public PipePuzzleNode startNode;

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

        if (!isClear && currentNode){
            if (Input.GetKeyDown(KeyCode.W)){
                StartCoroutine(WaitNodeRotate(currentNode.SmoothRotateAndCheck(hit.normal, Rotate.Up)));
            }
            if (Input.GetKeyDown(KeyCode.A)){
                StartCoroutine(WaitNodeRotate(currentNode.SmoothRotateAndCheck(hit.normal, Rotate.Left)));
            }
            if (Input.GetKeyDown(KeyCode.S)){
                StartCoroutine(WaitNodeRotate(currentNode.SmoothRotateAndCheck(hit.normal, Rotate.Down)));
            }
            if (Input.GetKeyDown(KeyCode.D)){
                StartCoroutine(WaitNodeRotate(currentNode.SmoothRotateAndCheck(hit.normal, Rotate.Right)));
            }
        }

        if (Input.GetMouseButtonUp(0)){
            currentNode = null;
        }
    }

    IEnumerator WaitNodeRotate(IEnumerator rotateRoutine){
        yield return StartCoroutine(rotateRoutine);

        CheckPuzzleClear();
    }

    void CheckPuzzleClear(){
        Queue<PipePuzzleNode> queue = new Queue<PipePuzzleNode>();
        List<PipePuzzleNode> visit = new List<PipePuzzleNode>();
        queue.Enqueue(startNode);

        while(queue.Count != 0){
            PipePuzzleNode node = queue.Dequeue();

            if (PipePuzzleNode.Type.End == node.type){
                isClear = true;
                break;
            }

            if (!visit.Contains(node)){
                visit.Add(node);

                for (int i = 0; i < node.nearNodes.Count; i++){
                    queue.Enqueue(node.nearNodes[i]);
                }
            }
        }
    }
}