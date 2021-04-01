using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public float moveSpeed; // 플레이어를 따라가는 스피드

    private Transform target; // 플레이어 Transform
    private Vector3 Pos; // 자신의 위치

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        Pos = transform.position;
        transform.position += (target.position - Pos) * moveSpeed;
    }
}
