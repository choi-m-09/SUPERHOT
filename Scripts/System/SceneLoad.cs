using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    public InputActionProperty Rightgrip;
    public InputActionProperty Leftgrip;
    public ParticleSystem ps;
    public AudioClip Grip;

    bool Next;
    public void Load_Airport()
    {
        SceneManager.LoadScene("Airport");
    }

    public void Load_Airplane()
    {
        SceneManager.LoadScene("Airplane");
    }

    public void StageClear()
    {
        //Instantiate()
    }

    public void HideTextAndCube()
    {
        //Destroy(GetComponent<SphereCollider>());
        Instantiate(ps, transform.position, transform.rotation);
        SoundManager.instance.SFXPlay("Grip", Grip);
        GetComponentInChildren<Canvas>().enabled = false;
        Destroy(GetComponentInChildren<MeshRenderer>());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (gameObject.layer == LayerMask.NameToLayer("Load_Airport") && Rightgrip.action.triggered || Leftgrip.action.triggered)
            {
                Debug.Log("Airport");
                HideTextAndCube();
                Invoke("Load_Airport", 2.0f);
            }
            else if (gameObject.layer == LayerMask.NameToLayer("Load_Airplane") && Rightgrip.action.triggered || Leftgrip.action.triggered)
            {
                HideTextAndCube();
                Invoke("Load_Airplane", 2.0f);
            }
        }
    }
}
