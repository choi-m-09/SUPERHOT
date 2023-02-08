using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ColliderMatching : MonoBehaviour
{
    public AudioClip deadSound;
    public Transform Cam;
    CapsuleCollider PlayerCollider;
    Vector3 Position;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerCollider = this.GetComponent<CapsuleCollider>();
        Cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Position.x = Cam.position.x;
        Position.y = Cam.position.y;
        Position.z = 0;
        PlayerCollider.center = Position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Debug.Log("Ãæµ¹");
            //SoundManager.instance.SFXPlay("Dead", deadSound);
        }
    }
}
