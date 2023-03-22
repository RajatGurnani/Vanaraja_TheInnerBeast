using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Settings", fileName = "Generic Settings")]
public class GameplaySettings : ScriptableObject
{
    public AnimationCurve difficultyCurve;

    [Tooltip("Max speed that the character can achieve")]
    public float maxSpeed = 10f;
    public float forwardSpeed = 10f;
    public float sideSpeed = 10f;

    [Tooltip("How much to scale the character")]
    public Vector3 scale = Vector3.one;
    public Vector3 cameraPosition = Vector3.zero;
}
