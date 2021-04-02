using System.Collections.Generic;
using UnityEngine;

public class Sample_Player : MonoBehaviour {


    // 리스트를 Dictionary 
    Dictionary<Sample_Interction.LifeKind, Sample_ILife> lifes = new Dictionary<Sample_Interction.LifeKind, Sample_ILife>();

    public Sample_Interction.LifeKind near;

    private void Start() {
        lifes.Add(Sample_Interction.LifeKind.Flower, new Sample_Farming());   // 0
        lifes.Add(Sample_Interction.LifeKind.Sea, new Sample_Fishing());   // 1
    }

    private void Update() {
        // 키입력
        if (Input.GetKeyDown(KeyCode.A)){
            // 근처에 오브젝트를 판별
            if (near != Sample_Interction.LifeKind.Null){
                lifes[near].Motion();
                lifes[near].Result();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        near = other.GetComponent<Sample_Interction>().lifeKind;
    }

    private void OnTriggerExit(Collider other) {
        near = Sample_Interction.LifeKind.Null;
    }
}