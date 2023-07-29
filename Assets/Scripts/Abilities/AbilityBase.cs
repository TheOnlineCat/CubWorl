using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AbilityBase : ScriptableObject
{
    public string Name;
    public float CooldownTime;
    public float ActiveTime;

    public virtual void Activate(GameObject parent) { }
}
