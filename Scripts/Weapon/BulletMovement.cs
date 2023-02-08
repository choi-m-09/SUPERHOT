using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public TrailRenderer tr;
    public float Speed;
    public ParticleSystem ps; // 충돌 시 발생되는 파티클
    public AudioClip Defend; // 총알 방어 시 재생되는 사운드

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    public void DestroyBullet()
    {
        Invoke("ReturnBullet", 3.0f);
    }
    private void ReturnBullet()
    {
        ObjectPool.ReturnObject(this);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == this.gameObject.layer) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyBodyPart bp = collision.gameObject.GetComponent<EnemyBodyPart>();

            bp.HideAndReplaced();
            bp.GetComponentInParent<Enemy>().Ragdoll();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Bottle"))
        {
            ThrowBodyPart[] gbp = collision.gameObject.GetComponentsInChildren<ThrowBodyPart>();
            foreach (ThrowBodyPart gp in gbp)
            {
                gp.HideAndReplaced();
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon")) SoundManager.instance.SFXPlay("Defend",Defend);

        Instantiate(ps, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
