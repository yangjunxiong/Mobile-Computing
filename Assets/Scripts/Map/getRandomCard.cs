using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getRandomCard : MonoBehaviour {
    public Sprite pencile;
    public Sprite cabinet;
    public Sprite desk;
    public Sprite perfume;

    public Sprite scroll;
    public Sprite book;
    public Sprite bomb;
    public Sprite reaper;

    private Image card;
    public Text numCards;

    public string itemName;
    public int itemNum;

    // Use this for initialization
    void Start () {
        updateCard();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void updateCard()
    {
        card = GetComponent<Image>();
        System.Random rnd = new System.Random();
        System.Random rndNum = new System.Random();
        int random = rnd.Next(80);

        if (random < 20)
        {
            card.sprite = pencile;
            itemName = "pencile";
            itemNum = rndNum.Next(1, 6);
        }
        else if (random < 30)
        {
            card.sprite = cabinet;
            itemName = "cabinet";
            itemNum = rndNum.Next(1, 4);
        }
        else if (random < 35)
        {
            card.sprite = desk;
            itemName = "desk";
            itemNum = rndNum.Next(1, 3);
        }
        else if (random < 40)
        {
            card.sprite = perfume;
            itemName = "perfume";
            itemNum = rndNum.Next(1, 4);
        }
        else if (random < 60)
        {
            card.sprite = scroll;
            itemName = "scroll";
            itemNum = rndNum.Next(1, 6);
        }
        else if (random < 70)
        {
            card.sprite = book;
            itemName = "book";
            itemNum = rndNum.Next(1, 4);
        }
        else if (random < 75)
        {
            card.sprite = bomb;
            itemName = "bomb";
            itemNum = rndNum.Next(1, 3);
        }
        else if (random < 80)
        {
            card.sprite = reaper;
            itemName = "reaper";
            itemNum = rndNum.Next(1, 3);
        }

        numCards.text = itemNum + "";
    } 
}
