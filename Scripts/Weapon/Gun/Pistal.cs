using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Pistal : Gun
{
    public AudioClip pickup;
    public AudioClip empty;
    public AudioClip destroy;
    public AudioClip shot;

    AudioSource pistalAudio;
    Rigidbody rb; 
    XRGrabInteractable Grab;
    TimeSystem Action;
    Enemy Dead;
    GameObject Bullet;
    [SerializeField]
    Transform Muzzle;
    Vector3 pos;
    Quaternion rot;

    private void Awake()
    {
        if (GetComponentInParent<Enemy>() == null) Equiped = false;
        pistalAudio = GetComponent<AudioSource>();
        pistalAudio.volume = 3.0f;
        Muzzle = this.transform.Find("Muzzle");
        Bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        this.Bullets = 12;
        Dead = GetComponent<Enemy>();
        Grab = GetComponent<XRGrabInteractable>();
        Grab.activated.AddListener( delegate { Shot(); } );
        Grab.selectEntered.AddListener(delegate { Pickup(); });
        Grab.selectExited.AddListener(delegate { Drop(); });
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        pos = Muzzle.position;
        rot = Muzzle.rotation;
    }

    override public void Shot()
    {
        pistalAudio.clip = shot;
        if (GetComponentInParent<Enemy>() != null)
        {
            pistalAudio.Play();
            var obj = ObjectPool.GetObject();
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            GameObject Ps = Instantiate(ps, pos, rot);
            Ps.GetComponent<ParticleSystem>().Play();
            Destroy(Ps, 2.0f);
            obj.DestroyBullet();
        }
        else
        {
            if (Bullets > 0)
            {
                pistalAudio.Play();
                Bullets -= 1;
                var obj = ObjectPool.GetObject();
                obj.transform.position = pos;
                obj.transform.rotation = rot;
                GameObject Ps = Instantiate(ps, pos, rot);
                Ps.GetComponent<ParticleSystem>().Play();
                Destroy(Ps, 2.0f);
                obj.DestroyBullet();
            }
            else
            {
                pistalAudio.clip = empty;
                pistalAudio.Play();
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

    public override void Release()
    {
        rb.isKinematic = true;
        Invoke("Kinematic", 0.1f);
        Equiped = false;
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        rb.AddForce((Camera.main.transform.position - transform.position) * 1.2f, ForceMode.Impulse);
        rb.AddForce(Vector3.up * 1.2f, ForceMode.Impulse);
    }

    protected override void Drop()
    {
        Equiped = false;
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void Kinematic()
    {
        rb.isKinematic = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == gameObject.layer) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet")) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Fragment")) return;
        if (collision.relativeVelocity.magnitude > 5.0f)
        {
            //Debug.Log(collision.relativeVelocity.magnitude);
            pistalAudio.clip = destroy;
            pistalAudio.Play();
            if (this.GetComponentInParent<Enemy>() == null && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyBodyPart bp = collision.gameObject.GetComponent<EnemyBodyPart>();

                bp.HideAndReplaced();
                bp.GetComponentInParent<Enemy>().Ragdoll();
                Destroy(this.gameObject);
            }
            if (this.GetComponentInParent<Enemy>() == null && Equiped == false)
            { 
                GunBodyPart[] gbp = GetComponentsInChildren<GunBodyPart>();
                foreach (GunBodyPart gp in gbp)
                {
                    gp.HideAndReplaced();
                }
                Destroy(gameObject);
            }
        }
        /*else if(collision.relativeVelocity.magnitude > 3.0f && Equiped == true && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyBodyPart bp = collision.gameObject.GetComponent<EnemyBodyPart>();

            bp.HideAndReplaced();
            bp.GetComponentInParent<Enemy>().Ragdoll();
        }*/
    }

}
