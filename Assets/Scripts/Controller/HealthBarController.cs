using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour {
    private Transform bar;

    private void Start() {
        bar = transform.GetChild(0);
    }

    private void Update() {
        
    }

    public void SetScale(float scale) {
        bar.localScale = new Vector3(1f, scale, 1f);
    }
}
