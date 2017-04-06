using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectIcon : MonoBehaviour {
    public Sprite pencile;
    public Sprite cabinet;
    public Sprite desk;
    public Sprite perfume;

    public Sprite scroll;
    public Sprite book;
    public Sprite bomb;
    public Sprite reaper;

    public int cardNo;
    public string itemName;
    public int itemNum;

    public GetRandomIcon getIcon;
    public Image card;
    public GameObject cardObject;
    public Text numCardsText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseUpAsButton()
    {
        //print("hello");
        getIcon.turnOff();
        cardNo = getIcon.randomCard;
        cardObject.SetActive(true);
        System.Random rndNum = new System.Random();

        if (cardNo == 1)
        {
            card.sprite = pencile;
            itemName = "pencil";
            itemNum = rndNum.Next(1, 6);
        }
        else if (cardNo == 2)
        {
            card.sprite = cabinet;
            itemName = "cabinet";
            itemNum = rndNum.Next(1, 4);
        }
        else if (cardNo == 3)
        {
            card.sprite = desk;
            itemName = "desk";
            itemNum = rndNum.Next(1, 3);
        }
        else if (cardNo == 4)
        {
            card.sprite = perfume;
            itemName = "perfume";
            itemNum = rndNum.Next(1, 4);
        }
        else if (cardNo == 5)
        {
            card.sprite = scroll;
            itemName = "scroll";
            itemNum = rndNum.Next(1, 6);
        }
        else if (cardNo == 6)
        {
            card.sprite = book;
            itemName = "book";
            itemNum = rndNum.Next(1, 4);
        }
        else if (cardNo == 7)
        {
            card.sprite = bomb;
            itemName = "bomb";
            itemNum = rndNum.Next(1, 3);
        }
        else if (cardNo == 8)
        {
            card.sprite = reaper;
            itemName = "skull";
            itemNum = rndNum.Next(1, 3);
        }
        cardObject.GetComponentInChildren<AddUnit>().SetValue(itemName, itemNum);
        numCardsText.text = itemNum + "";
    }
}
