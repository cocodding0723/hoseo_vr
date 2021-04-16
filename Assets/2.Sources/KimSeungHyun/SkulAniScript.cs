using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulAniScript : MonoBehaviour
{
    Monster skulObject;
    // Start is called before the first frame update
    void Start()
    {
        skulObject = transform.parent.gameObject.GetComponent<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAttack()
    {
        skulObject.CheckAttack();
    }
}
