using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public GameObject player;
    int playerHp = 100;
    float h;
    float v;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");
        //player.transform.position = (new Vector3(player.transform.position.x + h, player.transform.position.y, player.transform.position.z + v));
    }
    void OnTriggerEnter(Collider col)
    {
        print(col);
        if (col.tag == "MonsterArm")
            PlayerHit(10);
    }
    void PlayerHit(int damage)
    {
        playerHp -= damage;
        print("Hit!!");
    }

    //void TakeGunMag()
    //{
    //    if (Vector3.Distance(HDMTr.position, leftHand.position) < takeGunMagDis)
    //    {
    //        Instantiate(GunMag, leftHand.position, leftHand.rotation);
    //        GunMag.GetComponent<GunMag>().isGrap = true;
    //        GunMag.GetComponent<GunMag>().gunMagRigid.useGravity = false;
    //    }
    //}
}
