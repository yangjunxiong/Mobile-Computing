using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform player;
    public float radius;
    public float maxHeight;
    public float minHeight;
    public float rotationSpeed;
    public float heightSpeed;

    private float rotation;
    private float height;

    private void Start() {
        rotation = Mathf.PI;
        height = (maxHeight + minHeight) / 2;
    }

    private void Update() {
        // For PC
        if (Input.GetKey(KeyCode.LeftArrow)) {
            rotation += rotationSpeed;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            rotation -= rotationSpeed;
        }
        else if (Input.GetKey(KeyCode.UpArrow)) {
            height += heightSpeed;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            height -= heightSpeed;
        }

        // For phone
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Moved) {
                if (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
                    rotation += touch.deltaPosition.x * (rotationSpeed / 5);
                else
                    height -= touch.deltaPosition.y * (heightSpeed / 3);
            }
        }

        if (rotation > 2 * Mathf.PI)
            rotation -= 2 * Mathf.PI;
        if (rotation < -2 * Mathf.PI)
            rotation += 2 * Mathf.PI;
        if (height > maxHeight)
            height = maxHeight;
        if (height < minHeight)
            height = minHeight;

        transform.LookAt(player.position);
        float x = player.position.x + radius * Mathf.Sin(rotation);
        float y = player.position.y + height;
        float z = player.position.z + radius * Mathf.Cos(rotation);
        transform.position = new Vector3(x, y, z);
    }
}
