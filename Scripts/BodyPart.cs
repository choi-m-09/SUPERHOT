using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour
{
    public bool Replaced;
    public Rigidbody rb;
    abstract public void HideAndReplaced();
}
