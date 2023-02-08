using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RootMotion.FinalIK;

public class Enemy : MonoBehaviour
{
    public GameObject[] Elements; //WeaponHolder�� ���Ⱑ ���� �� ���� ������Ʈ�� ��� ����
    public Transform WeaponHolder; // ���� ���� ���� Ȯ�ο� ����
    public Transform Target; // NavMeshAgent�� Target�� ���� ����
    public bool Dead = false;
    public AudioClip col_sound;

    [SerializeField]
    Transform RightForeArm;
    AudioSource A_source;
    AimIK aimIK;
    BipedIK bipedIK;
    float Distance;
    Animator Anim;
    NavMeshAgent nav;
    GameObject Bullet;
    bool IsAttack = false;

    Coroutine pickup; // ���� pickup�� ����Ǵ� �ڷ�ƾ

    enum EnemyState
    {
        SHOTGUN,PISTAL,NONE
    }

    EnemyState State; // ���� ���¸� Ȯ���ϴ� ������ ����

    void Start()
    {
        State = EnemyState.NONE;
        A_source = GetComponent<AudioSource>();
        bipedIK = GetComponent<BipedIK>();
        aimIK = GetComponent<AimIK>();
        aimIK.solver.target = Camera.main.transform;
        //GetComponent<BipedIK>().SetLookAtPosition(Camera.main.transform.position);
        Bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        nav = GetComponent<NavMeshAgent>();
        Target = GameObject.Find("Player").transform;
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            if (Target != null)
            {
                Vector3 tv = new Vector3(Target.position.x, 0.0f, Target.position.z);
                Vector3 ev = new Vector3(transform.position.x, 0.0f, transform.position.z);
                Distance = Vector3.Distance(tv, ev);//(tv - ev).magnitude;
            }
            if (WeaponHolder.GetComponentInChildren<Pistal>() != null) State = EnemyState.PISTAL;
            else if (WeaponHolder.GetComponentInChildren<Shotgun>() != null) State = EnemyState.SHOTGUN;
            else State = EnemyState.NONE;
            if (!Dead) Movement();
        }
    }

    public void Movement()
    {
        if (nav.enabled != false)
        {
            //nav.SetDestination(Target.position);
            Anim.SetBool("IsRun", true);
        }
        switch(State)
        {
            case EnemyState.PISTAL:
                if (nav.enabled == false)
                {
                    nav.enabled = true;
                    Target = GameObject.Find("Player").transform;
                    nav.SetDestination(Target.position);
                    Anim.SetBool("Pistal", true);
                }
                if (aimIK.solver.IKPositionWeight < 1.0f)
                {
                    aimIK.solver.IKPositionWeight += Time.deltaTime;
                }

                if (Distance < 10.0f && Distance > 4.0f && !Anim.GetBool("Aiming"))
                {
                    Anim.SetBool("Aiming", true);
                    aimIK.solver.transform = GetComponentInChildren<Pistal>().transform.Find("AimTarget").transform;
                }
                
                else if (Distance < 4.0f)
                {
                    nav.enabled = false;
                    Anim.SetBool("Aiming", true);
                    aimIK.solver.transform = GetComponentInChildren<Pistal>().transform.Find("AimTarget").transform;
                    Vector3 tv = new Vector3(Target.position.x, 0.0f, Target.position.z);
                    Vector3 ev = new Vector3(transform.position.x, 0.0f, transform.position.z);
                    Vector3 pos = (tv - ev);
                    transform.rotation = Quaternion.LookRotation(pos, Vector3.up);
                    Anim.SetBool("IsRun", false);
                }
                break;
            case EnemyState.SHOTGUN:
                if (nav.enabled == false)
                {
                    nav.enabled = true;
                    Target = GameObject.Find("Player").transform;
                    nav.SetDestination(Target.position);
                    Anim.SetBool("Shotgun", true);
                }

                if (aimIK.solver.IKPositionWeight < 1.0f)
                {
                    aimIK.solver.IKPositionWeight += Time.deltaTime;
                }

                if (Distance < 10.0f && Distance > 4.0f && !Anim.GetBool("Aiming"))
                {
                    Anim.SetBool("Aiming", true);
                    aimIK.solver.transform = GetComponentInChildren<Shotgun>().transform.Find("AimTarget").transform;
                }
                else if (Distance < 4.0f)
                {
                    nav.enabled = false;
                    aimIK.solver.transform = GetComponentInChildren<Shotgun>().transform.Find("AimTarget").transform;
                    Anim.SetBool("Aiming", true);
                    Vector3 tv = new Vector3(Target.position.x, 0.0f, Target.position.z);
                    Vector3 ev = new Vector3(transform.position.x, 0.0f, transform.position.z);
                    Vector3 pos = (tv - ev);
                    transform.rotation = Quaternion.LookRotation(pos, Vector3.up);
                    Anim.SetBool("IsRun", false);
                }
                break;
            case EnemyState.NONE:
                Collider[] cols = Physics.OverlapSphere(transform.position, 4.0f, 1 << 7); // ���� ���� �ݶ��̴� ����

                if (cols.Length > 0 && cols != null && cols[0].transform.parent == null) // ���Ⱑ ������ ���
                {
                    Target = cols[0].gameObject.transform;
                    nav.SetDestination(Target.position);
                    if (Distance < 1.0f && pickup == null)
                    {
                        pickup = StartCoroutine(PickUp(cols[0]));
                    }
                }
                else // ���Ⱑ �������� ���� ���
                {
                    if (Distance < 1.2f)
                    {
                        bipedIK.enabled = false;
                        aimIK.enabled = true;
                        nav.enabled = false;
                        Anim.SetBool("IsRun", false);
                        Vector3 tv = new Vector3(Target.position.x, 0.0f, Target.position.z);
                        Vector3 ev = new Vector3(transform.position.x, 0.0f, transform.position.z);
                        Vector3 pos = (tv - ev); 
                        transform.rotation = Quaternion.LookRotation(pos, Vector3.up);
                        if(IsAttack == false)
                        {
                            Anim.SetTrigger("Punch");
                        }
                    }
                    else
                    {
                        Target = GameObject.Find("Player").transform;
                        nav.SetDestination(Target.position);
                    }
                }
                break;
        }
    }

    public void Ragdoll()
    {
        Anim.enabled = false;
        nav.enabled = false;
        bipedIK.enabled = false;
        aimIK.enabled = false;
        A_source.clip = col_sound;
        if (WeaponHolder.GetComponentInChildren<Pistal>() != null)
        {
            Pistal p = WeaponHolder.GetComponentInChildren<Pistal>();
            p.Release();
            p.Equiped = false;
        }
        else if (WeaponHolder.GetComponentInChildren<Shotgun>() != null)
        {
            Shotgun s = WeaponHolder.GetComponentInChildren<Shotgun>();
            s.Release();
            s.Equiped = false;
        }
        GetComponentInChildren<AimIK>().enabled = false;
        EnemyBodyPart[] Parts = GetComponentsInChildren<EnemyBodyPart>();
        foreach(EnemyBodyPart bp in Parts)
        {
            bp.rb.isKinematic = false;
            bp.rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        A_source.Play();
        Dead = true;
        Destroy(gameObject,1.0f);
    }

    public void Shot()
    {
        if (WeaponHolder.GetComponentInChildren<Pistal>() != null) WeaponHolder.GetComponentInChildren<Pistal>().Shot();
        else if (WeaponHolder.GetComponentInChildren<Shotgun>() != null) WeaponHolder.GetComponentInChildren<Shotgun>().Shot();
    }

    IEnumerator PickUp(Collider col)
    {
        if (col.GetComponentInParent<Pistal>()) col.GetComponent<Pistal>().Equiped = true;
        else if (col.GetComponentInParent<Shotgun>()) col.GetComponent<Shotgun>().Equiped = true;
        Anim.SetBool("IsRun", false);
        nav.enabled = false;
        Vector3 tv = new Vector3(Target.position.x, 0.0f, Target.position.z);
        Vector3 ev = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 pos = (tv - ev);
        transform.rotation = Quaternion.LookRotation(pos, Vector3.up);
        Anim.SetTrigger("Pickup");
        aimIK.enabled = false;
        bipedIK.enabled = true;
        bipedIK.solvers.rightHand.target = Target;
        bipedIK.solvers.spine.target = Target;
        bipedIK.solvers.rightHand.IKPositionWeight = 1.0f;
        yield return new WaitForSeconds(0.3f);
        WeaponHold(col);
        bipedIK.enabled = false;
        aimIK.enabled = true;
        yield break;
    }

    public void WeaponHold(Collider col)
    {
        if (col.gameObject.GetComponent<Pistal>())
        {
            Elements[0].SetActive(true);
        }
        else if(col.gameObject.GetComponent<Shotgun>())
        {
            Elements[1].SetActive(true);
        }
        Destroy(col.gameObject);
    }

    public void AttackStart()
    {
        RightForeArm.GetComponent<Rigidbody>().isKinematic = false;
        RightForeArm.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        IsAttack = true;

    }

    public void AttackEnd()
    {
        RightForeArm.GetComponent<Rigidbody>().isKinematic = true;
        RightForeArm.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        IsAttack = false;
    }
    
    /*
    float GetDistance(float eX, float eZ, float pX, float pZ)
    {
        float x = eX - pX;
        float z = eZ - pZ;

        float distance = (x * x) + (z * z);
        distance = Mathf.Sqrt(distance);

        return distance;
    }
    */
}
