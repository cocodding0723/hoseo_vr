using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float h;
    private float v;
    private bool isJumping = false;
    private bool isMoving = false;

    public float moveSpeed = 10.0f;
    public float jumpPower = 5.0f;
    public float rotateSpeed = 5.0f;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 movement;
    private Transform camVec; // 카메라 벡터
    private Vector3 camDir; // 카메라가 보는 방향

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        movement = Vector3.zero;
        camVec = GameObject.Find("CameraVector").transform;
        camDir = camVec.localRotation * Vector3.forward;
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            //anim.SetTrigger("doJumping");
            //anim.SetBool("isJumping", true);
        }

        //AnimationUpdate();
    }

    void FixedUpdate()
    {
        Run();
        Turn();
        Jump();
    }

    void AnimationUpdate()
    {
        if (h == 0 && v == 0)
        {
            //anim.SetBool("isRunning", false);
        }
        else
        {
            //anim.SetBool("isRunning", true);
        }
    }

    void Run()
    {
        movement.Set(h, 0, v);

        if (h == 0 && v == 0)
        {
            return;
        }
        else
        {
            transform.Translate(camDir * moveSpeed * Time.deltaTime);
        }
    }

    void Turn()
    {
        if (h == 0 && v == 0) // 가만히 있을 땐 회전되지 못하게 막아두는 것
            return;
        Quaternion newRotation = Quaternion.LookRotation(camVec.TransformDirection(movement));

        rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, rotateSpeed * Time.deltaTime);

        if (movement != Vector3.zero)
            rb.MoveRotation(transform.rotation = newRotation);
    }

    void Jump()
    {
        if (!isJumping)
            return;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        isJumping = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Ground")
        {
            //anim.SetBool("isJumping", false);
        }
    }
}
