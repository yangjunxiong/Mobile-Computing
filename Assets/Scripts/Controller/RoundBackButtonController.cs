using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundBackButtonController : MonoBehaviour {
    private GameController gameController;

    private void Start() {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        GetComponent<Button>().onClick.AddListener(() => gameController.LoadScene(gameController.menuSceneName));
    }
}
