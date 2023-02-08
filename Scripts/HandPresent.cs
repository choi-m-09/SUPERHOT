using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresent : MonoBehaviour
{
    public InputActionProperty gripAnimationAction;
    public float gripValue;
    public Animator gripAnim;
    // Start is called before the first frame update
    void Start()
    {
        gripAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        gripValue = gripAnimationAction.action.ReadValue<float>();
        gripAnim.SetFloat("Grip", gripValue);
    }
}
