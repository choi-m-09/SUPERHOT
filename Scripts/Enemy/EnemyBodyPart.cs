using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : BodyPart
{
    [SerializeField]
    SkinnedMeshRenderer BodyRenderer;
    
    public GameObject BodyPrefab;
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Replaced = false;
        ps = Resources.Load<ParticleSystem>("Prefabs/Particle/EnemyHitParticle");
    }

    // Update is called once per frame
    override public void HideAndReplaced()
    {
        if (Replaced) return;

        if (BodyRenderer != null)
            BodyRenderer.enabled = false;

        if (BodyPrefab != null)
        {
            GameObject part = Instantiate(BodyPrefab, transform.position, transform.rotation);

            Rigidbody[] rbs = part.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody r in rbs)
            {
                r.interpolation = RigidbodyInterpolation.Interpolate;
                r.AddExplosionForce(15, transform.position, 5);
            }
            Instantiate(ps, transform.position, transform.rotation);
            this.enabled = false;
            Replaced = true;
            Destroy(part, 2f);
        }
    }
}
