using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMag : MonoBehaviour
{

    public int gunMagCount;
    public bool isConnect;
    public bool isGrap;
    public Rigidbody gunMagRigid;
    
    void Start()
    {
        isGrap = false;
        gunMagRigid = GetComponent<Rigidbody>();
        gunMagCount = 30;
        if(isConnect == false)
        {
            gunMagRigid.useGravity = true;
        }
        else
        {
            gunMagRigid.useGravity = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
