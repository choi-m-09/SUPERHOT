using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBodyPart : BodyPart
{
    public MeshRenderer BodyRenderer;
    public GameObject BodyPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        BodyRenderer = GetComponent<MeshRenderer>();
        this.rb = GetComponent<Rigidbody>();
        this.Replaced = false;
    }


    override public void HideAndReplaced()
    {
        if (Replaced) return;

        if (BodyRenderer != null)
            Destroy(BodyRenderer); //BodyRenderer.enabled = false;

        if (BodyPrefab != null)
        {
            GameObject part = Instantiate(BodyPrefab, transform.position, transform.rotation);

            Rigidbody rbs = BodyPrefab.GetComponent<Rigidbody>();

            rbs.interpolation = RigidbodyInterpolation.Interpolate;
            rbs.AddExplosionForce(5.0f, transform.position, 0.5f);

            this.enabled = false;
            Replaced = true;
            Destroy(part, 2.0f);
        }
    }
}
