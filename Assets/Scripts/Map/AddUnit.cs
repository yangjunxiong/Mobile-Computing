using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddUnit : MonoBehaviour {
    private GameController gameController;
    private Text numberText;
    private Image itemImage;
    private string itemName;
    private int number;

    private void Start() {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        numberText = GameObject.Find("TextNub").GetComponent<Text>();
        itemImage = GameObject.Find("Card").GetComponent<Image>();
    }

    public void SetValue(string name, int number) {
        itemName = name;
        this.number = number;
    }

    public void AddUnits() {
        StartCoroutine(AddUnitsRoutine());
    }

    IEnumerator AddUnitsRoutine() {
        WWW www = new WWW(gameController.url + "AddUnits.php?id=" + gameController.id + "&name=" + itemName + "&number=" + number);
        yield return www;
        itemImage.gameObject.SetActive(false);
    }
}
