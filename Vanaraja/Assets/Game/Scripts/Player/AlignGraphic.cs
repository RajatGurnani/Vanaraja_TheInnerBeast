using UnityEngine;

/// <summary>
/// Align the player mesh perpendicular to the ground
/// </summary>
public class AlignGraphic : MonoBehaviour
{
    public float maxDistance;
    public Transform graphic;
    public Vector3 offset;
    public PlayerMovement playerMovement;

    private void Update()
    {
        if (playerMovement.distance>=200)
        {
            graphic.rotation= Quaternion.identity;
            Destroy(this);
        }

        Ray ray = new Ray(transform.position + offset, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance))
        {
            graphic.rotation = Quaternion.Slerp(graphic.rotation, Quaternion.FromToRotation(graphic.up, hitInfo.normal), Time.deltaTime);
        }
    }
}
