using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Spawn_Airplane : MonoBehaviour
{
    int Count = 0;
    [SerializeField]
    int Wave;

    public Transform[] SpawnPoint;
    public GameObject Pistal_Enemy;
    public GameObject Shotgun_Enemy;
    public PostProcessVolume pv;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitialWait());
    }

    // Update is called once per frame
    void Update()
    {
        Count = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (Count == 0)
        {
            if (Wave == 2)
            {
                Wave2();
            }
            else if (Wave == 3)
            {
                Wave3();
            }
            else if (Wave == 4)
            {
                StartCoroutine(SceneLoad());
            }
        }
    }

    void Wave1()
    {
        for (int i = 0; i < SpawnPoint.Length; i++)
        {
            Instantiate(Pistal_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
        }
        Wave += 1;
    }

    void Wave2()
    {
        for (int i = 0; i < SpawnPoint.Length; i++)
        {
            if (i == 0)
            {
                Instantiate(Shotgun_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
            }
            else Instantiate(Pistal_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
        }
        Wave += 1;
    }

    void Wave3()
    {
        for (int i = 0; i < SpawnPoint.Length; i++)
        {
            Instantiate(Shotgun_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
        }
        Wave += 1;
    }

    IEnumerator InitialWait()
    {
        Debug.Log("Initial");
        yield return new WaitForSeconds(1.0f);
        Wave1();
        yield break;
    }

    IEnumerator SceneLoad()
    {
        while (pv.weight < 1.0f)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            pv.weight += 0.1f;
        }
        SceneManager.LoadScene("Intro");
    }
}
