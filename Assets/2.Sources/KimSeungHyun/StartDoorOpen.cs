using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDoorOpen : MonoBehaviour
{

    [SerializeField]
    Transform door1;
    [SerializeField]
    Transform door2;
    bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        OpenTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(door1.transform.rotation.x) > 0.3f && isOpen == true)
            GameStartDoorOpen();
        else { }
    }
    void GameStartDoorOpen()
    {
        door1.transform.Rotate(new Vector3(-100* Mathf.Lerp(0,1,Time.deltaTime), 0, 0));
        door2.transform.Rotate(new Vector3(+100 * Mathf.Lerp(0, 1, Time.deltaTime), 0, 0));
    }

    public void OpenTrigger()
    {
        isOpen = true;
    }
}
