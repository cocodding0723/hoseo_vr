using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BrokenObject : MonoBehaviour {
    public bool isReverse = false;

    [Range(1f,100f)]
    [SerializeField]
    float randomValueAdjust = 10f;

    [SerializeField]
    float brokenPower = 5f;

    [Tooltip("오브젝트 크기")]
    [SerializeField]
    float objectRange = 1f;

    [SerializeField]
    Transform breakFragmentsParent = null;

    Rigidbody rigid = null;

    bool isBroken = false;
    List<Rigidbody> childRigids = null;

    private void Start() {
        rigid = GetComponent<Rigidbody>();
        childRigids = GetComponentsInChildren<Rigidbody>().ToList();

        ChildRigidInit();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.relativeVelocity.magnitude > brokenPower && !isBroken){
            isBroken = true;
            
            GetComponent<BoxCollider>().enabled = false;

            Vector3 contactAveragePoint = GetAverageOfContacts(other.contacts);
            
            ObjectBreak(contactAveragePoint);
            ReleaseChildren();

            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        childRigids.Clear();
        rigid = null;
    }

    private void ReleaseChildren(){
        for (int i = 0; i < childRigids.Count; i++){
            childRigids[i].transform.parent = breakFragmentsParent;
        }
    }

    private Vector3 GetAverageOfContacts(ContactPoint[] contacts){
        Vector3 sum = Vector3.zero;

        for (int i = 0; i < contacts.Length; i++){
            sum += contacts[i].point;
        }

        return sum / contacts.Length;
    }

    private Vector3 GetAverageOfContacts(ContactPoint[] contacts, out Vector3 normal){
        Vector3 sum = Vector3.zero;
        normal = Vector3.zero;

        for (int i = 0; i < contacts.Length; i++){
            sum += contacts[i].point;
            normal += contacts[i].normal;
        }

        normal /= contacts.Length;

        return sum / contacts.Length;
    }

    private void ChildRigidInit(){
        for (int i = 0; i < transform.childCount; i++){
            Transform _child = transform.GetChild(i);
            Rigidbody _childRigid = _child.GetComponent<Rigidbody>();

            if (_childRigid == null){
                _child.gameObject.AddComponent<Rigidbody>();
                _childRigid = _child.GetComponent<Rigidbody>();
                childRigids.Add(_childRigid);
            }

            _childRigid.isKinematic = true;
            // w_childRigid.mass = rigid.mass / childRigids.Count;
        }
    }

    public void ObjectBreak(Vector3 breakPoint){
        for (int i = 0; i < childRigids.Count; i++){
            childRigids[i].isKinematic = false;
            Vector3 breakDirection = isReverse ? breakPoint - childRigids[i].position : childRigids[i].position - breakPoint;
            Vector3 randNormal = Vector3Extension.GetRandomNormal() / randomValueAdjust;
            childRigids[i].AddForceAtPosition((breakDirection + randNormal).normalized, childRigids[i].position, ForceMode.Impulse);
        }
    }

    public void ObjectBreak(Vector3 breakPoint, float breakPower){
        for (int i = 0; i < childRigids.Count; i++){
            childRigids[i].isKinematic = false;
            Vector3 breakDirection = isReverse ? breakPoint - childRigids[i].position : childRigids[i].position - breakPoint;
            Vector3 randNormal = Vector3Extension.GetRandomNormal() / randomValueAdjust;
            childRigids[i].AddForceAtPosition((breakDirection + randNormal).normalized * breakPower, childRigids[i].position, ForceMode.Impulse);
        }
    }
}