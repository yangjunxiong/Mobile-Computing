using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    public void Vanish(float time) {
        StartCoroutine(VanishRoutine(time));
    }

    IEnumerator VanishRoutine(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
