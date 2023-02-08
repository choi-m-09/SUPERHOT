using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    GameObject poolingObjectPrefab;

    Queue<BulletMovement> poolingObjectQueue = new Queue<BulletMovement>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            Initialize(10);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    BulletMovement CreateNewObject()
    {
        var obj = Instantiate(poolingObjectPrefab, transform).GetComponent<BulletMovement>();
        obj.tr.time = 0;
        obj.gameObject.SetActive(false);
        return obj;
    }

    void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    public static BulletMovement GetObject()
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.tr.time = 0.3f;
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var obj = Instance.CreateNewObject();
            obj.tr.time = 0.3f;
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
    }

    public static void ReturnObject(BulletMovement bullet)
    {
        bullet.tr.time = 0;
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(bullet);
    }
}
