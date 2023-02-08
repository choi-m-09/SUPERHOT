using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class CDRoom_Movement : MonoBehaviour
{
    public Transform Screen;
    public Material Line;
    public PostProcessVolume P_Volume;
    public AudioClip CDRoom;
    public AudioClip Computer;
    
    Animator Anim;
    [SerializeField]
    bool MoveLine = false;
    float L_moveSpeed = 0.0f;
    Coroutine diffuse = null;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveLine)
        {
            L_moveSpeed += Time.deltaTime;
            Line.mainTextureOffset = new Vector2(0, L_moveSpeed);
            Camera.main.transform.GetComponent<TrackedPoseDriver>().trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
            if(diffuse == null) diffuse = StartCoroutine(Diffuse());
        }
    }

    IEnumerator CDRoom_Move(Collision other)
    {
        Destroy(other.gameObject);
        transform.GetChild(0).gameObject.SetActive(true);
        Anim.SetBool("Move",true);
        yield return new WaitForSeconds(0.5f);
        SoundManager.instance.SFXPlay("CloseDisk", CDRoom);
        yield return new WaitForSeconds(CDRoom.length);
        Screen.GetComponent<MeshRenderer>().enabled = true;
        MoveLine = true;
        SoundManager.instance.SFXPlay("Computer", Computer);
    }

    IEnumerator Diffuse()
    {
        while (P_Volume.weight < 1.0f)
        {
            P_Volume.weight += Time.deltaTime;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Airport");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CD"))
        {
            StartCoroutine(CDRoom_Move(other));
        }
    }

    
    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ãæµ¹");
        if (other.gameObject.layer == LayerMask.NameToLayer("CD"))
        {
            Debug.Log("Start");
            StartCoroutine(CDRoom_Move(other));
        }
    }*/
}
