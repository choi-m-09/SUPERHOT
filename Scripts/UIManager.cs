using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text Text;
    public TimeSystem Time; // UI�� ��Ÿ���� ���� �ð� ����
    public Spawn Wave;
    public Transform Head; // �÷��̾� ī�޶� ��ġ
    public float spawnDIst = 3f; // UI ���� �Ÿ�

    [SerializeField]
    GameObject pn;
    [SerializeField]
    Animator Anim;
    bool End = false;

    void Start()
    {
        Invoke("Start_Text", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Wave.End == true && End != true) End_Text();
        if (pn.activeSelf == true)
        {
            this.gameObject.transform.position = Head.position + new Vector3(Head.forward.x, 0.2f, Head.forward.z).normalized * spawnDIst;
            this.gameObject.transform.LookAt(new Vector3(Head.position.x, Head.position.y, Head.position.z));
            this.gameObject.transform.forward *= -1;
        }
    }

    void Start_Text()
    {
        pn.SetActive(true);
        Time.action = true;
        Anim.SetTrigger("Start");
    }

    void End_Text()
    {
        End = true;
        pn.SetActive(true);
        Text.text = "SUPER";
        Time.action = true;
        Anim.SetTrigger("End");
    }
}
