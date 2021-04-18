using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMag : MonoBehaviour
{

    public int gunMagCount;
    public bool isConnect;
    public bool isGrap;
    public Rigidbody gunMagRigid;
    
    void Awake()
    {
        isGrap = false;
        gunMagRigid = GetComponent<Rigidbody>();
        gunMagCount = 30;
        if (isConnect == false)
        {
            gunMagRigid.isKinematic = false;
            gunMagRigid.useGravity = true;
        }
        else if (isConnect == true)
            gunMagRigid.isKinematic = true;
        if (isGrap == true)
            gunMagRigid.useGravity = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GrapObject()
    {

    }
}
