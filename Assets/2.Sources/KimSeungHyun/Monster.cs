using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    Animator skulAnimator;
    public AvatarMask skulAvatar;
    Transform playerTr;
    Transform skulTr;
    NavMeshAgent skulNav;
    Collider[] colls;
    Vector3 randVec;
    public Transform startRandomVec;
    public int skulHp = 100;
    public int skulDamage = 10;
    bool isPlayer;
    bool beforeWalk;
    bool isFind;
    bool randCheckBool = false;
    RaycastHit findPlayerHit;
    float findPlayerFloat = 20f;
    // Start is called before the first frame update
    public enum SkulState
    {
        idle,
        walk,
        run,
        damage,
        attack1,
        attack2,
        down,
        death
    };
    SkulState state = new SkulState();
    void Awake()
    {
        isPlayer = false;
        isFind = false;
        beforeWalk = false;
        randVec = Vector3.zero;
        skulNav = this.GetComponent<NavMeshAgent>();
        state = SkulState.idle;
        skulAnimator = transform.GetComponent<Animator>();
        skulTr = this.transform;
        CreateRandomTr();
    }
    private void Start()
    {
        playerTr = Player.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(skulHp > 0)
        {
            CheckSkulState();
            if (Vector3.Distance(playerTr.position, skulTr.position) < 6f)
                skulTr.forward = (new Vector3(playerTr.position.x, skulTr.position.y, playerTr.position.z) - skulTr.position);
        }
        if(skulHp < 0)
        {
            DeadAnimatorOff();
            state = SkulState.death;
        }
    }
    void SkulAniState()
    {

    }
    void CheckSkulState()
    {
        if (skulAnimator.GetBool("isMove"))
        {
            skulAnimator.SetFloat("MoveSpeed", skulNav.speed);
            state = SkulState.walk;
            FindPlayer();
        }
        if (Vector3.Distance(randVec, skulTr.position) < 2f && beforeWalk == false && state == SkulState.walk)
        {
            beforeWalk = true;
            StartCoroutine(WalkChangeIdle());
        }
    }

    float CulAngle(Transform target)
    {
        float angle = Vector3.Dot(skulTr.forward, (target.position - skulTr.position).normalized);
        float cosAngle = Mathf.Acos(angle) * Mathf.Rad2Deg;
        //print(cosAngle);
        return cosAngle;
    }

    void CreateRandomTr()
    {
        beforeWalk = false;
        randVec = new Vector3(startRandomVec.transform.position.x + Random.Range(-20, 20),
            startRandomVec.transform.position.y, startRandomVec.transform.position.z + Random.Range(-20, 20));
        state = SkulState.walk;
        skulNav.SetDestination(randVec);
    }
    void RandomVector()
    {
        randVec = new Vector3(startRandomVec.transform.position.x + 
            Random.Range(-5, 5), startRandomVec.transform.position.y, startRandomVec.transform.position.z + Random.Range(-20, 20));
        colls = Physics.OverlapSphere(randVec, 1f);
        for(int i = 0; i<colls.Length; i++)
        {
            if (colls[i].gameObject.tag == "Obstacle")
            {
                randCheckBool = true;
                print("ReRandom");
            }
            colls[i] = null;
        }
        if(randCheckBool == true)
            RandomVector();

    }
    IEnumerator WalkChangeIdle()
    {   
        state = SkulState.idle;
        for(float i =35; i >= 0; i--)
        {
            skulNav.speed = (i/10);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(Random.Range(1.0f,4.0f));
        if(skulNav.enabled == true)
        CreateRandomTr();
        for (float i = 0; i <= 35; i++)
        {
            skulNav.speed = (float)(i / 10);
            yield return new WaitForSeconds(0.05f);
        }
        state = SkulState.walk;
    }
    void FindPlayer()
    {
        if(Vector3.Distance(skulTr.position,playerTr.position) < 10f)
        {
            if(CulAngle(playerTr) < 30f)
            {
                if(Physics.Raycast(skulTr.position, playerTr.position - skulTr.position, out findPlayerHit, 10f))
                {
                    Debug.Log(findPlayerHit.collider);
                    if(findPlayerHit.collider.tag == "Player")
                    {
                        isFind = true;
                        StopAllCoroutines();
                        StartCoroutine(FindPlayerRun());
                    }
                }
            }
        }
        CheckAttack();
    }
    IEnumerator FindPlayerRun()
    {
        for (float i = skulNav.speed * 10; i <= 100; i++)
        {
            skulNav.speed = (float)(i / 10);
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void CheckAttack()
    {
        if (isFind == true)
        {
            if (Vector3.Distance(playerTr.position, skulTr.position) > 5f)
            {
                skulNav.SetDestination(playerTr.position + (skulTr.position - playerTr.position).normalized * 4f);
                state = SkulState.run;
            }
            if(Vector3.Distance(playerTr.position, skulTr.position) < 6f)
            {
                state = SkulState.attack1;
                skulAnimator.SetBool("isAttack", true);
                skulAnimator.SetBool("isMove", false);            
            }
            else
            {
                state = SkulState.walk;
                skulAnimator.SetBool("isAttack", false);
                skulAnimator.SetBool("isMove", true);
            }
        }
    }
    void DeadAnimatorOff()
    {
        skulNav.enabled = false;
        skulAnimator.enabled = false;
    }    
    public void UseGravityGlove()
    {
        skulAnimator.SetBool("isFly", true);
        SetSkulState(state);
        skulNav.enabled = false;
    }

    public void ExitGravityGlove()
    {
        skulAnimator.SetBool("isFly", false);
        skulAnimator.SetBool("isMove", true);
        skulNav.enabled = true;
    }

    void SetSkulState(SkulState st)
    {
        if(st == SkulState.idle || st == SkulState.walk || st == SkulState.run)
        {
            skulAnimator.SetBool("isMove", false);
        }
        else if(st == SkulState.attack1)
        {
            skulAnimator.SetBool("isAttack1", false);
        }
    }
}
