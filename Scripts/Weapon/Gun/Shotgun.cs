using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Shotgun : Gun
{
    public AudioClip pickup;
    public AudioClip empty;
    public AudioClip destroy;
    public AudioClip shot;

    Rigidbody rb;
    XRGrabInteractable Grab;
    [SerializeField]
    Transform Muzzle;
    GameObject Bullet;
    Vector3 Muzzlepos;
    Quaternion Muzzlerot;
    AudioSource shot_Audio;
    
    void Awake()
    {
        if (GetComponentInParent<Enemy>() == null) Equiped = false;
        shot_Audio = GetComponent<AudioSource>();
        Bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        Muzzle = this.transform.Find("Muzzle");
        rb = GetComponentInChildren<Rigidbody>();
        this.Bullets = 2;
        Grab = GetComponent<XRGrabInteractable>();
        Grab.activated.AddListener(delegate { Shot(); });
        Grab.selectEntered.AddListener(delegate { Pickup(); });
        Grab.selectExited.AddListener(delegate { Drop(); });
        //Grab.selectEntered 
        //Grab.selectExited.AddListener(delegate { Release(); });
    }

    // Update is called once per frame
    void Update()
    {
        Muzzlepos = Muzzle.position;
        Muzzlerot = Muzzle.rotation;
    }

    override public void Shot()
    {
        shot_Audio.clip = shot;   
        if (GetComponentInParent<Enemy>() != null)
        {
            shot_Audio.Play();
            bulletSpread();
        }
        else
        {
            if (Bullets > 0)
            {
                shot_Audio.Play();
                Bullets -= 1;
                bulletSpread();
            }
            else
            {
                shot_Audio.clip = empty;
                shot_Audio.Play();
            }
        }
        
    }

    protected override void Pickup()
    {
        Equiped = true;
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.None;
        SoundManager.instance.SFXPlay("GunPickup", pickup);
    }

    override public void Release()
    {
        Equiped = false;
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        //gameObject.layer = (1 << 7);

        rb.AddForce((Camera.main.transform.position - transform.position) * 2.0f, ForceMode.Impulse);
        rb.AddForce(Vector3.up * 1.5f, ForceMode.Impulse);
    }

    protected override void Drop()
    {
        Equiped = false;
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void bulletSpread()
    {
        for (int i = 0; i < 10; i++)
        {
            float Xrot = Muzzlerot.x - Random.Range(-0.1f, 0.1f) * Mathf.Rad2Deg;
            float Yrot = Muzzlerot.y - Random.Range(-0.1f, 0.1f) * Mathf.Rad2Deg;
            var obj = ObjectPool.GetObject();
            obj.transform.position = Muzzlepos;
            obj.transform.rotation = Muzzlerot;
            obj.transform.Rotate(Xrot, Yrot, 0);
            obj.GetComponent<BoxCollider>().isTrigger = true;
            StartCoroutine(Collider(obj));
            GameObject Ps = Instantiate(ps, Muzzlepos, Muzzlerot);
            Ps.GetComponent<ParticleSystem>().Play();
            Destroy(Ps, 1.0f);
            obj.DestroyBullet();
        }
    }

    IEnumerator Collider(BulletMovement obj)
    {
        yield return new WaitForSeconds(0.08f);
        obj.GetComponent<BoxCollider>().isTrigger = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == gameObject.layer) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet")) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Fragment")) return;
        if (collision.relativeVelocity.magnitude > 5.0f)
        {
            shot_Audio.clip = destroy;
            shot_Audio.Play();
            if (GetComponentInParent<Enemy>() == null && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyBodyPart bp = collision.gameObject.GetComponent<EnemyBodyPart>();

                bp.HideAndReplaced();
                bp.GetComponentInParent<Enemy>().Ragdoll();
            }
            GunBodyPart[] gbp = GetComponentsInChildren<GunBodyPart>();
            foreach (GunBodyPart gp in gbp)
            {
                gp.HideAndReplaced();
            }
            Destroy(gameObject);
        }
    }
}
