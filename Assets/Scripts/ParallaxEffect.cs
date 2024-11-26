using Unity.VisualScripting;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] float effect;
    GameObject mainCamera;
    Vector3 lastCamPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main.gameObject;
        lastCamPosition = mainCamera.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 cameraMovement = mainCamera.transform.position - lastCamPosition;
        transform.position += new Vector3 (cameraMovement.x * effect, cameraMovement.y, 0);
        lastCamPosition = mainCamera.transform.position;
    }
}
