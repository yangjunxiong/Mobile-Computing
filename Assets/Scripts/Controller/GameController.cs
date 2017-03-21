using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject[] attackerPrefabs;
    public GameObject[] defenderPrefabs;
    public Text prompt;
    public GameObject attackList, defendList;
    public bool isAttacker = false;
    public bool inGame = true;

    private GameObject[] prefabs;
    private GameObject[] spawnTiles;
    private GameObject[] enemyTiles;
    private GameObject[] otherTiles;
    private Button[] buttons;
    private List<GameObject> activeUnits;
    private Transform finishPoint;
    private int objectIndex = -1;
    private string[] unitName;
    private int[] remainingUnitNumber;
    private float[] unitCooldownTime;

    private const int nullObjectIndex = -1;
    private const string attackTileTagName = "AttackTile";
    private const string defendeTileTagName = "DefendeTile";
    private const string tileTagName = "Tile";
    private const string finishPointTagName = "Finish";

    private void Start() {
        // Selection according to different scenes
        if (!inGame)
            return;

        // Retrieve data from server
        unitName = new string[4];
        unitName[0] = "Pencil";
        unitName[1] = "Desk";
        unitName[2] = "Cabinet";
        remainingUnitNumber = new int[4];
        remainingUnitNumber[0] = 100;
        remainingUnitNumber[1] = 5;
        remainingUnitNumber[2] = 5;
        unitCooldownTime = new float[4];
        unitCooldownTime[0] = 5f;
        unitCooldownTime[1] = 10f;
        unitCooldownTime[2] = 10f;

        // Assign appropriate value according to role
        if (isAttacker) {
            defendList.SetActive(false);
            buttons = attackList.GetComponent<ButtonListController>().buttons;
            spawnTiles = GameObject.FindGameObjectsWithTag(attackTileTagName);
            enemyTiles = GameObject.FindGameObjectsWithTag(defendeTileTagName);
            prefabs = attackerPrefabs;
        }
        else {
            attackList.SetActive(false);
            buttons = defendList.GetComponent<ButtonListController>().buttons;
            spawnTiles = GameObject.FindGameObjectsWithTag(defendeTileTagName);
            enemyTiles = GameObject.FindGameObjectsWithTag(attackTileTagName);
            prefabs = defenderPrefabs;
        }
        otherTiles = GameObject.FindGameObjectsWithTag(tileTagName);

        // Initialization for buttons, tiles
        for (int i = 0; i < spawnTiles.Length; i++)
            spawnTiles[i].GetComponent<TileMouseHandle>().tileIndex = i;
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.GetComponent<ButtonController>().SetValue(unitName[i], remainingUnitNumber[i], unitCooldownTime[i]);
        activeUnits = new List<GameObject>();
        finishPoint = GameObject.FindGameObjectWithTag(finishPointTagName).transform;
    }

    private void Update() {
        if (!inGame)
            return;

        if (isAttacker) {
            if (activeUnits.Count <= 0 && remainingUnitNumber[0] == 0 && remainingUnitNumber[1] == 0 && remainingUnitNumber[2] == 0 && remainingUnitNumber[3] == 0)
                RequestGameOver(false);
        }
    }

    // When spawn buttons are clicked, this function will change selected unit to spawn
    public void SelectObjectToSpawn(int index) {
        if (index >= 0 && index < prefabs.Length) {
            // Cancle previous selection
            if (objectIndex == index) {
                buttons[index].GetComponent<ButtonController>().Highlight(false);
                objectIndex = nullObjectIndex;
                for (int i = 0; i < spawnTiles.Length; i++)
                    spawnTiles[i].GetComponent<TileMouseHandle>().SwitchHighlight(false);
                return;
            }

            // New selection
            objectIndex = index;
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].GetComponent<ButtonController>().Highlight(false);
            buttons[index].GetComponent<ButtonController>().Highlight(true);
            for (int i = 0; i < spawnTiles.Length; i++)
                spawnTiles[i].GetComponent<TileMouseHandle>().SwitchHighlight(true);
        }
    }

    // This will send spawn request to server
    public void RequestSpawnObject(TileMouseHandle tileMouseHandle) {
        if (objectIndex != nullObjectIndex && spawnTiles[tileMouseHandle.tileIndex].GetComponent<TileMouseHandle>() == tileMouseHandle)
        {
            if (remainingUnitNumber[objectIndex] > 0 && !tileMouseHandle.isOccupied)
            {
                for (int i = 0; i < spawnTiles.Length; i++)
                    spawnTiles[i].GetComponent<TileMouseHandle>().SwitchHighlight(false);
                buttons[objectIndex].GetComponent<ButtonController>().Highlight(false);

                GameObject tile = spawnTiles[tileMouseHandle.tileIndex];
                GameObject unit = Instantiate(prefabs[objectIndex], tile.transform.position, Quaternion.identity);
                activeUnits.Add(unit);
                remainingUnitNumber[objectIndex]--;
                if (!unit.GetComponentInChildren<Unit>().canMove || unit.GetComponentInChildren<Unit>().speed == 0) {
                    tileMouseHandle.isOccupied = true;
                    unit.GetComponentInChildren<Unit>().SetAssignedTile(tileMouseHandle);
                }
                buttons[objectIndex].GetComponent<ButtonController>().Cooldown();
                objectIndex = nullObjectIndex;
            }
        }
    }

    // This will send Gameover request to server
    public void RequestGameOver(bool win) {

    }

    public void UnitDie(GameObject unit) {
        activeUnits.Remove(unit);
        TileMouseHandle tile = unit.GetComponentInChildren<Unit>().GetAssignedTile();
        if (tile != null)
            tile.isOccupied = false;
    }

    public int GetObjectIndex() {
        return objectIndex;
    }

    public List<GameObject> GetActiveUnits() {
        return activeUnits;
    }

    public Transform GetFinishPoint() {
        return finishPoint;
    }
}
