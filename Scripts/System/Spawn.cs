using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour
{
    public int Count = 0; // 현재 남아있는 적의 수
    public int Wave = 1; // 현재 웨이브
    public Transform[] SpawnPoint; // 스폰 지점
    public Vector3 cubePosition; // 레벨이 클리어 시 나타나는 큐브 위치
    public GameObject Enemy;
    public GameObject Pistal_Enemy;
    public GameObject Shotgun_Enemy;
    public GameObject StageClear_Cube; // 레벨 클리어 시 나타나는 큐브 오브젝트
    public float spawnDist = 0.5f;
    public bool End = false;
    [SerializeField]
    Transform Head;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Wave1", 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Count = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Head = Camera.main.transform;
        if (Count == 0)
        {
            switch(Wave)
            {
                case 2: 
                    Wave2();
                    break;
                case 3:
                    Wave3();
                    break;
                case 4:
                    if (End == false)
                    {
                        cubePosition = Head.transform.position - new Vector3(Head.transform.position.x, 0.0f, Head.transform.position.z).normalized * spawnDist;
                        End = true;
                        Instantiate(StageClear_Cube, cubePosition, Quaternion.identity);
                    }
                    break;
            }    
            /*
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
            */
        }
    }

    void Wave1()
    {
        if (SceneManager.GetActiveScene().name == "Airport")
        {
            for (int i = 0; i < SpawnPoint.Length - 2; i++)
            {
                Instantiate(Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Airplane")
        {
            for (int i = 0; i < SpawnPoint.Length; i++)
            {
                Instantiate(Pistal_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
            }
        }
        Wave += 1;
    }

    void Wave2()
    {
        if (SceneManager.GetActiveScene().name == "Airport")
        {
            for (int i = 0; i < SpawnPoint.Length - 1; i++)
            {
                Instantiate(Pistal_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Airplane")
        {
            for (int i = 0; i < SpawnPoint.Length; i++)
            {
                if (i == 0)
                {
                    Instantiate(Shotgun_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
                }
                else Instantiate(Pistal_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
            }
        }
        Wave += 1;
    }

    void Wave3()
    {
        if (SceneManager.GetActiveScene().name == "Airport")
        {
            for (int i = 0; i < SpawnPoint.Length; i++)
            {
                if (i == 0) Instantiate(Shotgun_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
                else Instantiate(Pistal_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
            }
        }
        else if(SceneManager.GetActiveScene().name == "Airplane")
        {
            for (int i = 0; i < SpawnPoint.Length; i++)
            {
                Instantiate(Shotgun_Enemy, SpawnPoint[i].position, SpawnPoint[i].rotation);
            }
        }
        Wave += 1;
    }
}
