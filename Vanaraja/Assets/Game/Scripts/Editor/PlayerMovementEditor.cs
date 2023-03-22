using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerMovement playerMovement= target as PlayerMovement;
        //playerMovement.maxSpeed = EditorGUILayout.Slider("Max Speed",playerMovement.maxSpeed,10f,100f);

        base.OnInspectorGUI();
    }
}
