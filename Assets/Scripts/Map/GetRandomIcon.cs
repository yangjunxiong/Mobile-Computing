using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomIcon : MonoBehaviour {
    public int randomCard;
    public GameObject capsule;
    public GameObject pencile;
    public GameObject cabinet;
    public GameObject desk;
    public GameObject perfume;
    public GameObject scroll;
    public GameObject book;
    public GameObject bomb;
    public GameObject reaper;

    public float timeLeft;
    public int maxWaitTime = 60;

    public bool hasShown = false;

    public float positionx;
    public float positionz;

    // Use this for initialization
    void Start () {
        System.Random rnd = new System.Random();
        timeLeft = rnd.Next(1, maxWaitTime);
    }
	
	// Update is called once per frame
	void Update () {
        if (hasShown)
        {
            return;
        }

        timeLeft -= UnityEngine.Time.deltaTime;
        if (timeLeft <= 0)
        {
            System.Random rnd1 = new System.Random();
            randomCard = rnd1.Next(1, 9);

            System.Random rndx = new System.Random();
            positionx = capsule.transform.position.x + rndx.Next(-8, 8);
            

            System.Random rndz = new System.Random();
            positionz = capsule.transform.position.z + rndz.Next(-2, 10);
            //print("randomCard: " + randomCard);
            //print(positionx + " " + positionz);

            if (randomCard == 1)
            {
                pencile.SetActive(true);
                pencile.transform.position = new Vector3(positionx, 1, positionz);
            }
            else if (randomCard == 2)
            {
                cabinet.SetActive(true);
                cabinet.transform.position = new Vector3(positionx, 1, positionz);
            }
            else if (randomCard == 3)
            {
                desk.SetActive(true);
                desk.transform.position = new Vector3(positionx, 1, positionz);
            }
            else if (randomCard == 4)
            {
                perfume.SetActive(true);
                perfume.transform.position = new Vector3(positionx, 1, positionz);
            }
            else if (randomCard == 5)
            {
                scroll.SetActive(true);
                scroll.transform.position = new Vector3(positionx, 1, positionz);
            }
            else if (randomCard == 6)
            {
                book.SetActive(true);
                book.transform.position = new Vector3(positionx, 1, positionz);
            }
            else if (randomCard == 7)
            {
                bomb.SetActive(true);
                bomb.transform.position = new Vector3(positionx, 1, positionz);
            }
            else if (randomCard == 8)
            {
                reaper.SetActive(true);
                reaper.transform.position = new Vector3(positionx, 1, positionz);
            }

            hasShown = true;
        }
    }

    public void turnOff()
    {
        if (!hasShown)
        {
            return;
        }
        hasShown = false;

        pencile.SetActive(false);
        cabinet.SetActive(false);
        desk.SetActive(false);
        perfume.SetActive(false);
        scroll.SetActive(false);
        book.SetActive(false);
        bomb.SetActive(false);
        reaper.SetActive(false);

        System.Random rnd = new System.Random();
        timeLeft = rnd.Next(1, maxWaitTime);
        //print("TimeLeft: "+timeLeft);
    }

}
