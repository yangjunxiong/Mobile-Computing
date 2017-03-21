using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMouseHandle : MonoBehaviour {
    public GameController gameController;
    public int tileIndex;
    public bool isOccupied = false;

    private bool highlight = false;
    private Renderer render;

    private void Start() {
        render = gameObject.GetComponent<Renderer>();
    }

    private void Update() {
        if (highlight) {
            render.material.color = Color.Lerp(Color.white, Color.clear, Mathf.PingPong(Time.time, 1));
        }
        else {
            render.material.color = Color.white;
        }
    }

    private void OnMouseUpAsButton() {
        gameController.RequestSpawnObject(this);
    }

    public void SwitchHighlight(bool on) {
        if (on && !isOccupied)
            highlight = true;
        else
            highlight = false;
    }
}
