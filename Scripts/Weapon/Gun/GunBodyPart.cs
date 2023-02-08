using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBodyPart : BodyPart
{
    public MeshRenderer gunBodyRenderer;
    public GameObject BodyPrefab;
    
    // Start is called before the first frame update
    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        gunBodyRenderer = GetComponent<MeshRenderer>();
        this.Replaced = false;
    }


    override public void HideAndReplaced()
    {
        if (Replaced) return;

        if (gunBodyRenderer != null)
            Destroy(gunBodyRenderer); //gunBodyRenderer.enabled = false;

        if (BodyPrefab != null)
        {
            GameObject part = Instantiate(BodyPrefab, transform.position, transform.rotation);

            Rigidbody rbs = BodyPrefab.GetComponent<Rigidbody>();

            rbs.interpolation = RigidbodyInterpolation.Interpolate;
            rbs.AddExplosionForce(15, transform.position, 10);
            //rb.AddExplosionForce(15, transform.position, 5);

            this.enabled = false;
            Replaced = true;
            Destroy(part, 2.0f);
        }
    }
}
