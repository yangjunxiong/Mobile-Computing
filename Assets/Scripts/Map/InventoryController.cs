using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {
    private GameController gameController;
    private Text pencilText, deskText, cabinetText, perfumeText, scrollText, bookText, bombText, skullText;

    private const string pencilTextName = "PencileNum";
    private const string deskTextName = "DeskNum";
    private const string cabinetTextName = "CabinetNum";
    private const string perfumeTextName = "PerfumeNum";
    private const string scrollTextName = "ScrollNum";
    private const string bookTextName = "BookNum";
    private const string bombTextName = "BombNum";
    private const string skullTextName = "ReaperNum";

    public void OnEnable() {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        pencilText = GameObject.Find(pencilTextName).GetComponent<Text>();
        deskText = GameObject.Find(deskTextName).GetComponent<Text>();
        cabinetText = GameObject.Find(cabinetTextName).GetComponent<Text>();
        perfumeText = GameObject.Find(perfumeTextName).GetComponent<Text>();
        scrollText = GameObject.Find(scrollTextName).GetComponent<Text>();
        bookText = GameObject.Find(bookTextName).GetComponent<Text>();
        bombText = GameObject.Find(bombTextName).GetComponent<Text>();
        skullText = GameObject.Find(skullTextName).GetComponent<Text>();
        StartCoroutine(UpdateInventoryRoutine());
    }

    IEnumerator UpdateInventoryRoutine() {
        WWW www = new WWW(gameController.url + "Units.php?id=" + gameController.id + "&side=defender");
        yield return www;
        UnitProp[] temp = JsonUtility.FromJson<UnitPropList>(www.text).propList;
        pencilText.text = temp[0].number.ToString();
        deskText.text = temp[1].number.ToString();
        cabinetText.text = temp[2].number.ToString();
        perfumeText.text = temp[3].number.ToString();

        WWW www2 = new WWW(gameController.url + "Units.php?id=" + gameController.id + "&side=attacker");
        yield return www2;
        temp = JsonUtility.FromJson<UnitPropList>(www2.text).propList;
        scrollText.text = temp[0].number.ToString();
        bookText.text = temp[1].number.ToString();
        bombText.text = temp[2].number.ToString();
        skullText.text = temp[3].number.ToString();
    }
}
