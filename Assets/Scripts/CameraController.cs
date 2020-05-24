using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    // Public variables
    public Tilemap map;
    public Transform target;

    // Audio variables
    public int musicToPlay;
    private bool musicStarted;

    // Private variables
    private Vector3 topRightLimit;
    private Vector3 bottomLeftLimit;
    private float halfWidth;
    private float halfHeight;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) {
            target = PlayerController.instance.transform;
        }
        target = FindObjectOfType<PlayerController>().transform;
        
        this.halfHeight = Camera.main.orthographicSize;
        this.halfWidth = halfHeight * Camera.main.aspect;

        this.topRightLimit = map.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);
        this.bottomLeftLimit = map.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);

        PlayerController.instance.SetBounds(map.localBounds.min, map.localBounds.max);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y,transform.position.z);

        // Keep the camera inside the bounds
        var boundsX = Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x); 
        var boundsY = Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y);
        transform.position = new Vector3(boundsX, boundsY, transform.position.z);

        if (!musicStarted) {
            musicStarted = true;
            AudioManager.instance.PlayBackgroundMusic(musicToPlay);
        }
    }
}
