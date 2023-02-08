using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorScreen : MonoBehaviour
{
    private Material monitorMat;
    float speed = 1.0f;
    float screenY;
    // Start is called before the first frame update
    void Start()
    {
        monitorMat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        screenY += speed * Time.deltaTime;
        monitorMat.SetTextureOffset("_MainTex", new Vector2(0, screenY));
    }
}
