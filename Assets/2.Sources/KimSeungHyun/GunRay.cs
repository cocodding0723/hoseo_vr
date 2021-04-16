using UnityEngine;

public class GunRay : MonoBehaviour
{
    public GameObject FPivot;
    public GameObject BPivot;
    public GameObject scopeRed;
    public GameObject gunHitObject;
    public GameObject gunMagLight;
    [SerializeField]
    GameObject shotEffect;
    [SerializeField]
    Transform effectTr;
    GameObject GrapGunMagObject;
    GameObject useGunMagObject;
    GameObject gunMag;
    GunMag isGunMag;
    GunMag useGunMag;
    Transform gunReloadPivot;
    Transform gunMagPivot;
    RaycastHit shotHit;
    RaycastHit scopeHit;
    Quaternion cameraQt;
    Transform camTr;
    Ray shotRay;
    Ray scopeRay;
    float angle1;
    float angle2;
    int layerMaskBPivot;
    public enum Gun
    {
        NeedReload,
        Reload,
        able
    };
    Gun gun = new Gun();
    // Start is called before the first frame update
    void Start()
    {
        gun = Gun.able;
        cameraQt = Quaternion.Euler(0, 10f, 0);
        layerMaskBPivot = 1 << LayerMask.NameToLayer("BPivot");
        gunMag = transform.Find("GunMag").gameObject;
        camTr = Camera.main.transform;
        gunReloadPivot = transform.Find("GunReloadPivot");
        gunMagPivot = transform.Find("GunMagPivot");
        useGunMagObject = transform.Find("GunMag").gameObject;
        useGunMag = useGunMagObject.GetComponent<GunMag>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckScope();
        DisGunMag();
        ReLoad();
        Debug.DrawLine(camTr.position, FPivot.transform.position, Color.blue);
        if(shotHit.point !=null)
        Debug.DrawLine(camTr.position, shotHit.point - camTr.position);
        if (useGunMag.gunMagCount <= 0 && gun != Gun.Reload)
        {
            gun = Gun.NeedReload;
            gunMagLight.GetComponent<Light>().enabled = true;
        }
        else if(useGunMag.gunMagCount > 0 && gunMagLight.GetComponent<Light>().enabled == true)
        {
            gunMagLight.GetComponent<Light>().enabled = false;
        }
        if (Input.GetMouseButtonDown(0) && useGunMag.gunMagCount > 0 && gun == Gun.able)
        {
            Shot2();
        }
        else if(gun != Gun.able)
        {
            OnClickObject();
        }
        //Debug.Log(useGunMag.gunMagCount);
            
    }
    void CheckScope()
    {
        float dist = Vector3.Distance(camTr.position, FPivot.transform.position);
        scopeRay = new Ray(camTr.position, FPivot.transform.position);
        if (Physics.Raycast(FPivot.transform.position, (FPivot.transform.position - BPivot.transform.position).normalized, out shotHit, 100f))
        {
            if (shotHit.point == null)
                shotHit.point = FPivot.transform.forward * 100f;
            //print(shotHit.point);
        }
        if (Physics.Raycast(camTr.position, shotHit.point - camTr.position, out scopeHit, 1000f, 
            layerMaskBPivot) && shotHit.point != null)
        {
            Debug.Log("hit");
            if(scopeRed.activeSelf == false)
            scopeRed.SetActive(true);
            if (scopeHit.collider != null)
                scopeRed.transform.position = scopeHit.point;
        }
        else
            scopeRed.SetActive(false);
    }
    //void Shot()
    //{
    //    Debug.Log("Shot");
    //    if (Physics.Raycast(FPivot.transform.position, (FPivot.transform.position - scopeRed.transform.position).normalized,out shotHit,1000f))
    //    {
    //        if(shotHit.collider != null)
    //        {
    //            Instantiate(gunHitObject, shotHit.point, Quaternion.identity);
    //        }
    //    }
    //}
    void Shot2()
    {
        //Debug.Log("Shot");
        if (Physics.Raycast(FPivot.transform.position, (FPivot.transform.position - BPivot.transform.position).normalized, out shotHit, 100f))
        {
            Instantiate(shotEffect, effectTr.position,Quaternion.identity);
            if (shotHit.collider != null)
            {
                Instantiate(gunHitObject, shotHit.point, Quaternion.identity);
                useGunMag.gunMagCount--;
            }
            if(shotHit.collider.tag == "Monster")
            {
                FindParant(shotHit.transform);
                print("Check");
            }
        }
    }
    void ReLoad()
    {
        if (gun == Gun.Reload && GrapGunMagObject != null)
        {
            Debug.Log("Reload대기");
            if(Vector3.Distance(GrapGunMagObject.transform.position,gunReloadPivot.transform.position) < 10.0f)
            {
                Debug.Log("Reload가능");
                isGunMag.gunMagRigid.velocity = Vector3.zero;
                GrapGunMagObject.transform.position = new Vector3(gunReloadPivot.position.x,GrapGunMagObject.transform.position.y,gunReloadPivot.position.z);
                GrapGunMagObject.transform.rotation = gunMagPivot.rotation;
                    isGunMag.isConnect = true;
                isGunMag.gunMagRigid.isKinematic = true;
                if(Vector3.Distance(GrapGunMagObject.transform.position, gunMagPivot.transform.position) < 3.0f)
                {
                    Debug.Log("Reload완료");
                    GrapGunMagObject.transform.position = gunMagPivot.position;
                    GrapGunMagObject.transform.rotation = gunMagPivot.rotation;
                    gun = Gun.able;
                    useGunMagObject = GrapGunMagObject;
                    useGunMag = isGunMag;
                    useGunMagObject.transform.parent = transform;
                    isGunMag.gunMagRigid.velocity = Vector3.zero;
                    GrapGunMagObject = null;
                    isGunMag = null;
                }
            }
        }
    }
    void DisGunMag()
    {
        if((gun == Gun.able || gun == Gun.NeedReload)&& Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("InputReload");
            gun = Gun.Reload;
            useGunMag.isConnect = false;
            useGunMag.gunMagRigid.isKinematic = false;
            useGunMag.gunMagRigid.useGravity = true;
            useGunMagObject.transform.parent = null;
        }
    }
    void OnClickObject()
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mouseHit;
        if (GrapGunMagObject != null && Input.GetMouseButtonDown(0) && isGunMag.isGrap == true)
        {
            isGunMag.gunMagRigid.useGravity = true;
            isGunMag.isGrap = false;
            GrapGunMagObject = null;
            isGunMag = null;
            Debug.Log("ClickOut");
        }
        else if (Physics.Raycast(mousePos.origin, mousePos.direction, out mouseHit))
        {
            if(mouseHit.collider.tag == "Mag" && Input.GetMouseButtonDown(0))
            {
                Debug.Log("CLick");
                GrapGunMagObject = mouseHit.transform.gameObject;
                isGunMag = GrapGunMagObject.GetComponent<GunMag>();
                isGunMag.gunMagRigid.useGravity = false;
                isGunMag.isGrap = true;
                //if(isGunMag.isConnect == false)
                //    isGunMag.isConnect = true;
            }
            if (GrapGunMagObject != null )
            {
                if (isGunMag.isConnect == false)
                    GrapGunMagObject.transform.position = (camTr.position + (mouseHit.point - camTr.position).normalized * 35f);
                else
                {
                    Debug.Log("11");
                    GrapGunMagObject.transform.position = new Vector3(GrapGunMagObject.transform.position.x, camTr.position.y + (mouseHit.point - camTr.position).normalized.y * 35f, GrapGunMagObject.transform.position.z);
                }
                    
                Debug.DrawLine(camTr.position, (mouseHit.point - camTr.position).normalized * 35f);
            }
        }


    }
    void CulGunAngle()
    {
        //angle1 = Vector3.Angle(FPivot.transform.position - Camera.main.transform.position, FPivot.transform.position - BPivot.transform.position);
        angle1 = Vector3.Angle(BPivot.transform.position - camTr.forward * 100f, BPivot.transform.position - FPivot.transform.position);
        //Debug.Log(angle1);
        angle2 = 90 - angle1;
    }

    void FindParant(Transform pObject)
    {
        print("FindParant");
        if (pObject.gameObject.tag == "HasScript")
        {
            pObject.GetComponent<Monster>().skulHp -= 40;
        }
        else if (pObject.transform.parent != null)
        {
            pObject = pObject.transform.parent;
            FindParant(pObject);
        }
        else if (pObject.transform.parent == null) { }        
    }
}
