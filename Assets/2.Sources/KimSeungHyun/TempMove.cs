using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMove : MonoBehaviour
{
    public Transform thisTr;
    float h;
    float v;
    // Start is called before the first frame update
    void Start()
    {
        thisTr = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        thisTr.transform.position = (new Vector3(thisTr.transform.position.x + h, thisTr.transform.position.y, thisTr.transform.position.z + v));
    }
}
