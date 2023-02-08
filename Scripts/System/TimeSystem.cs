using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.Input;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;


public class TimeSystem : MonoBehaviour
{
    public AudioClip deadSound;
    public PostProcessVolume pv;
    public bool action = false;

    Vector3 lastHeadPos;
    Vector3 lastHandLPos;
    Vector3 lastHandRPos;
    [SerializeField]
    Transform handL;
    [SerializeField]
    Transform handR;
    [SerializeField]
    InputActionProperty Right_trigger;
    [SerializeField]
    InputActionProperty Left_trigger;
    [SerializeField]
    InputActionProperty Right_Grip;
    [SerializeField]
    InputActionProperty Left_Grip;
    

    float headSpeed;
    float handLSpeed;
    float handRSpeed;
    float timeScaleTarget;
    float tSpeed;

    private void Update()
    {
        if(action == false &&Right_trigger.action.triggered || Left_trigger.action.triggered)
        {
            StopCoroutine(Action());
            StartCoroutine(Action());
        }
        if(action == false && Right_Grip.action.triggered || Left_Grip.action.triggered)
        {
            StopCoroutine(Action());
            StartCoroutine(Action());
        }
        
    }
    private void FixedUpdate()
    {
        timeScaleTarget = action ? 1f : tSpeed;
        Time.timeScale = Mathf.Lerp(Time.timeScale, timeScaleTarget, Time.deltaTime * 30f);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        
        CalcTime();
    }
    void CalcTime()
    {
        headSpeed = (transform.position - lastHeadPos).magnitude;
        lastHeadPos = transform.position;

        handLSpeed = (handL.position - lastHandLPos).magnitude;
        lastHandLPos = handL.position;

        handRSpeed = (handR.position - lastHandRPos).magnitude;
        lastHandRPos = handR.position;

        tSpeed = Mathf.Clamp((headSpeed + handLSpeed + handRSpeed) * 10f, 0.0f, 1.0f);
    }

    public IEnumerator Action()
    {
        action = true;
        yield return new WaitForSecondsRealtime(0.1f);
        action = false;
    }

    public IEnumerator PlayerDead()
    {
        SoundManager.instance.SFXPlay("Dead", deadSound);
        GetComponent<TrackedPoseDriver>().trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
        Time.timeScale = 0.0f;
        while(pv.weight < 1.0f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            pv.weight += 0.1f;
        }
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Intro");
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            StopAllCoroutines();
            GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(PlayerDead());
        }
    }
    
}
