using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerController : MonoBehaviour {
	public void Play(AudioClip clip, float time) {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        StartCoroutine(timer(time));
    }

    IEnumerator timer(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
