using UnityEngine;

public class SkullBottle : MonoBehaviour
{
    public float glowDuration = 2f;
    public Material glowMaterial;
    public Material unglowMaterial;
    public MeshRenderer rightEyeRenderer;
    public MeshRenderer leftEyeRenderer;

    public void Update()
    {
        float value = Mathf.PingPong(Time.timeSinceLevelLoad, glowDuration) / glowDuration;
        rightEyeRenderer.material.Lerp(unglowMaterial, glowMaterial, value);
        leftEyeRenderer.material.Lerp(unglowMaterial, glowMaterial, value);
    }
}
