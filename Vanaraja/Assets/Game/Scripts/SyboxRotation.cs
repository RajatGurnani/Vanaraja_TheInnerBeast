using System.Collections.Generic;
using UnityEngine;

public class SyboxRotation : MonoBehaviour
{
    public List<Material> skyboxes;
    public float rotationSpeed = 10f;
    public bool rotateSkybox;

    private void Update()
    {
        if (rotateSkybox)
        {
            RenderSettings.skybox.SetFloat("_Rotation", rotationSpeed);
        }
    }
}
