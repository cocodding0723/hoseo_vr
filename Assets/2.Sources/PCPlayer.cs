using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayer : MonoBehaviour
{
    [SerializeField]
    float move_speed = 5f;

    [SerializeField]
    float limit_rotate = 70f;
    float senetivity = 10f;
    float current_rot_x = 0f;
    float current_rot_y = 0f;

    Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
    }

    void Move(){
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 direction = (myTransform.forward * v + myTransform.right * h).normalized;
        myTransform.position += direction * move_speed * Time.deltaTime;
    }

    void Rotate(){
        float rot_y = Input.GetAxisRaw("Mouse X");
        float rot_x = Input.GetAxisRaw("Mouse Y");

        current_rot_x += rot_x * senetivity;
        current_rot_y += rot_y * senetivity;

        current_rot_x = Mathf.Clamp(current_rot_x, -limit_rotate, limit_rotate);

        myTransform.localEulerAngles = new Vector3(-current_rot_x, current_rot_y, 0f);
    }
}
