using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {
    public Text nameText;
    public Text numberText;
    public Text cooldownText;
    public Image cooldownImage;
    public string name;
    public int number;
    public float cooldownTime;

    private bool isCooldown = false;
    private float speed;
    private float remainingTime = 0f;
    private float height = 0f;

    private const int buttonSize = 120;

    void Update() {
        if (number <= 0) {
            numberText.text = "0";
            cooldownText.text = "";
            cooldownImage.rectTransform.sizeDelta = new Vector2(buttonSize, buttonSize);
            gameObject.GetComponent<Button>().interactable = false;
            return;
        }

        if (isCooldown) {
            height = Mathf.MoveTowards(cooldownImage.rectTransform.sizeDelta.y, 0f, speed * Time.deltaTime);
            remainingTime = Mathf.Round(height / buttonSize * cooldownTime);
            numberText.text = number.ToString();
            cooldownText.text = remainingTime > 0f ? (int)(remainingTime / 60) + ":" + (int)(remainingTime % 60) : "";
            cooldownImage.rectTransform.sizeDelta = new Vector2(buttonSize, height);
        }
        else {
            cooldownText.text = "";
            numberText.text = number.ToString();
            cooldownImage.rectTransform.sizeDelta = new Vector2(buttonSize, 0f);
        }
        nameText.text = name;
    }

    public void Cooldown() {
        number--;
        speed = buttonSize / cooldownTime;
        cooldownImage.rectTransform.sizeDelta = new Vector2(buttonSize, buttonSize);
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
