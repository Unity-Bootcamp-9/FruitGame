using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input Action/SwipeInputActionReference")]
public class SwipeInputActionReference : ScriptableObject
{
    public InputActionProperty PrimaryContact;
    public InputActionProperty PrimaryPosition;
}
