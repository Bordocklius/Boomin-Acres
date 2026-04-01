using UnityEngine;

public class WorldSpaceUIHandler : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
