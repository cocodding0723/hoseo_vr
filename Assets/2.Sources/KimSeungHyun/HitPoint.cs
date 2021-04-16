using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(objectDelete());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator objectDelete()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
