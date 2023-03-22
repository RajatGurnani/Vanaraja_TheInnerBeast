using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MovementSpecification", fileName = "Generic Specification")]
public class MovementSpecification : ScriptableObject
{
    [Tooltip("Max speed that the character can achieve")]
    public float maxSpeed = 10f;
    public float forwardSpeed = 10f;
    public float sideSpeed = 10f;
    public float jumpSpeed = 100f;
    public bool canJump = false;
   
    [Tooltip("How much to scale the character")]
    public Vector3 scale = Vector3.one; 
    public Vector3 cameraPosition = Vector3.zero;
}
