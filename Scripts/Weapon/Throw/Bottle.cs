using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bottle : WeaponScript
{
    Rigidbody rb;
    XRGrabInteractable Grab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Grab = GetComponent<XRGrabInteractable>();
        Grab.selectExited.AddListener(delegate { Release(); });

    }
    public override void Release()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Fragment")) return;
        if (collision.relativeVelocity.magnitude > 2.0f)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyBodyPart bp = collision.gameObject.GetComponent<EnemyBodyPart>();

                bp.HideAndReplaced();
                bp.GetComponentInParent<Enemy>().Ragdoll();
            }
            ThrowBodyPart[] gbp = GetComponentsInChildren<ThrowBodyPart>();
            foreach (ThrowBodyPart gp in gbp)
            {
                gp.HideAndReplaced();
            }
            Destroy(gameObject);
        }
    }
}
