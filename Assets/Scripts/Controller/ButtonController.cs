using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {
    public Text numberText;
    public Image cooldownImage;
    public int number;
    public float cooldownTime;
    public bool isCooldown = false;
    public int index;

    private GameController gameController;
    private float speed;
    private float remainingTime = 0f;
    private float height = 0f;
    private float buttonSizeX, buttonSizeY;

    private void Start() {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        GetComponent<Button>().onClick.AddListener(() => gameController.SelectObjectToSpawn(index));
        buttonSizeX = cooldownImage.rectTransform.sizeDelta.x;
        buttonSizeY = cooldownImage.rectTransform.sizeDelta.y;
    }

    void Update() {
        if (number <= 0) {
            numberText.text = "0";
            cooldownImage.rectTransform.sizeDelta = new Vector2(buttonSizeX, buttonSizeY);
            gameObject.GetComponent<Button>().interactable = false;
            return;
        }

        if (isCooldown) {
            height = Mathf.MoveTowards(cooldownImage.rectTransform.sizeDelta.y, 0f, speed * Time.deltaTime);
            remainingTime = Mathf.Round(height / buttonSizeY * cooldownTime);
            numberText.text = number.ToString();
            cooldownImage.rectTransform.sizeDelta = new Vector2(buttonSizeX, height);
        }
        else {
            numberText.text = number.ToString();
            cooldownImage.rectTransform.sizeDelta = new Vector2(buttonSizeX, 0f);
        }
    }

    public void Cooldown() {
        number--;
        speed = buttonSizeY / cooldownTime;
        cooldownImage.rectTransform.sizeDelta = new Vector2(buttonSizeX, buttonSizeY);
        StartCoroutine(CoolDownCount());
    }

    IEnumerator CoolDownCount() {
        isCooldown = true;
        gameObject.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(cooldownTime);
        gameObject.GetComponent<Button>().interactable = true;
        isCooldown = false;
    }

    public void Highlight(bool on) {
        if (on)
            gameObject.GetComponent<Image>().color = Color.red;
        else
            gameObject.GetComponent<Image>().color = Color.white;
    }

    public void SetValue(string name, int number, float cooldownTime)
    {
        this.name = name;
        this.number = number;
        this.cooldownTime = cooldownTime;
    }
}
