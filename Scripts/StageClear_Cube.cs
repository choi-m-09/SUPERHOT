using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.InputSystem;

public class StageClear_Cube : MonoBehaviour
{
    public GameObject Particle;
    [SerializeField]
    PostProcessVolume pv;
    [SerializeField]
    InputActionProperty Right_Grip;
    [SerializeField]
    InputActionProperty Left_Grip;

    private void Start()
    {
        pv = GameObject.Find("PostProcessing").GetComponent<PostProcessVolume>();
    }

    IEnumerator Grab()
    {
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Instantiate(Particle, transform.position, transform.rotation);
        yield return new WaitForSeconds(2.0f);
        while(pv.weight < 1.0f)
        {
            pv.weight += Time.deltaTime;
        }
        if (SceneManager.GetActiveScene().name == "Airport") SceneManager.LoadScene("AirPlane");
        else if (SceneManager.GetActiveScene().name == "Airplane") SceneManager.LoadScene("Intro");
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && Right_Grip.action.triggered || Left_Grip.action.triggered)
        {
            StopAllCoroutines();
            StartCoroutine(Grab());
        }
    }
}
