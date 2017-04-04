using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameController : NetworkBehaviour {
    public GameObject[] attackerPrefabs;
    public GameObject[] defenderPrefabs;
    public Sprite attackWin, attackLose, defendWin, defendLose;
    public AudioClip gameBGM, gameoverAudio;
    public float pairTryRate;
    public float pollRate;
    public float gameoverTime = 120f;
    public float returnMenuTime;
    public bool isAttacker;

    private int id = -1;
    private int roundID;
    private int enemyID;
    private bool ready;
    private bool gameover;
    private float timeOfStart;
    private Canvas canvas;
    private AudioSource audioSource;
    private GameObject attackList, defendList;
    private GameObject gameoverImage;
    private Text timerText;
    private GameObject[] prefabs, enemyPrefabs;
    private GameObject[] spawnTiles, enemyTiles, otherTiles;
    private Button[] buttons;
    private List<GameObject> activeUnits;
    private Transform finishPoint;
    private int objectIndex = -1;
    private string[] unitName;
    private int[] remainingUnitNumber;
    private float[] unitCooldownTime;

    private const int nullObjectIndex = -1;
    private const string menuSceneName = "Menu";
    private const string gameSceneName = "Round";
    private const string mapSceneName = "Map";
    private const string attackTileTagName = "AttackTile";
    private const string defendeTileTagName = "DefendeTile";
    private const string tileTagName = "Tile";
    private const string finishPointTagName = "Finish";
    private const string url = "http://mobilecomputing-codingbear.c9users.io/";

    void OnEnable() {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == menuSceneName) {
            roundID = -1;
            enemyID = -1;
            ready = false;
            gameover = false;
        }
        else if (scene.name == gameSceneName) {
            // Retrieve unit data from server
            Time.timeScale = 0f;
            StartCoroutine(RetrieveUnitsRoutine());

            // Initialize UI
            canvas = FindObjectOfType<Canvas>();
            attackList = canvas.transform.Find("Attack List").gameObject;
            defendList = canvas.transform.Find("Defende List").gameObject;
            gameoverImage = canvas.transform.Find("Gameover Image").gameObject;
            gameoverImage.SetActive(false);
            timerText = canvas.transform.Find("Timer").transform.Find("Time").GetComponent<Text>();

            // Assign appropriate value according to role
            if (isAttacker) {
                defendList.SetActive(false);
                buttons = attackList.GetComponent<ButtonListController>().buttons;
                spawnTiles = GameObject.FindGameObjectsWithTag(attackTileTagName);
                enemyTiles = GameObject.FindGameObjectsWithTag(defendeTileTagName);
                prefabs = attackerPrefabs;
                enemyPrefabs = defenderPrefabs;
            }
            else {
                attackList.SetActive(false);
                buttons = defendList.GetComponent<ButtonListController>().buttons;
                spawnTiles = GameObject.FindGameObjectsWithTag(defendeTileTagName);
                enemyTiles = GameObject.FindGameObjectsWithTag(attackTileTagName);
                prefabs = defenderPrefabs;
                enemyPrefabs = attackerPrefabs;
            }
            otherTiles = GameObject.FindGameObjectsWithTag(tileTagName);

            // Other initialization
            for (int i = 0; i < spawnTiles.Length; i++)
                spawnTiles[i].GetComponent<TileMouseHandle>().tileIndex = i;
            for (int i = 0; i < enemyTiles.Length; i++)
                enemyTiles[i].GetComponent<TileMouseHandle>().tileIndex = i;
            activeUnits = new List<GameObject>();
            finishPoint = GameObject.FindGameObjectWithTag(finishPointTagName).transform;
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = gameBGM;
            audioSource.Play();
        }
        else if (scene.name == mapSceneName) {

        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (SceneManager.GetActiveScene().name == gameSceneName) {
            if (!ready || gameover)
                return;
            int tempTime = (int)(gameoverTime - (Time.time - timeOfStart));
            timerText.text = (tempTime / 60).ToString() + " : " + (tempTime % 60).ToString();
            // Gameover situation: attack uses up all units
            if (isAttacker) {
                if (activeUnits.Count <= 0 && remainingUnitNumber[0] == 0 && remainingUnitNumber[1] == 0 && remainingUnitNumber[2] == 0 && remainingUnitNumber[3] == 0)
                    RequestGameOver(false);
            }
            // Gameover situation: time up
            if (tempTime <= 0f)
                RequestGameOver(isAttacker ? false : true);
        }
        else if (SceneManager.GetActiveScene().name == menuSceneName) {

        }
        else if (SceneManager.GetActiveScene().name == mapSceneName) {

        }
    }

    // Send pair request to server
    public void NewGame(string side) {
        if (id == -1)
            return;
        StartCoroutine(PairRoutine(side));
    }

    public void Login() {
        if (id != -1)
            return;
        StartCoroutine(LoginRoutine("", ""));
    }

    public void Register() {
        if (id != -1)
            return;
        StartCoroutine(RegisterRoutine("", ""));
    }

    public void ChangeToMap() {
        if (id == -1)
            return;
        SceneManager.LoadScene(mapSceneName);
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
                // Switch off highlight effect
                for (int i = 0; i < spawnTiles.Length; i++)
                    spawnTiles[i].GetComponent<TileMouseHandle>().SwitchHighlight(false);
                buttons[objectIndex].GetComponent<ButtonController>().Highlight(false);
                
                // Send request to server
                StartCoroutine(RequestSpawnRoutine(tileMouseHandle.gameObject.name, objectIndex));
                objectIndex = nullObjectIndex;
                ToggleInput(false);
            }
        }
    }

    // This will spawn opponent's unit in response to server
    public void SpawnEnemyObject(string tileName, int objectIndex) {
        GameObject tile = FindTileByName(tileName);
        GameObject unit = Instantiate(enemyPrefabs[objectIndex], tile.transform.position, Quaternion.identity);
        activeUnits.Add(unit);
    }

    // This will send Gameover request to server
    public void RequestGameOver(bool win) {
        StartCoroutine(RequestGameoverRoutine(win));
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

    private void Gameover(bool win) {
        if (gameover)
            return;
        gameover = true;
        StopCoroutine(PollRoutine());
        audioSource.clip = gameoverAudio;
        audioSource.Play();
        attackList.SetActive(false);
        defendList.SetActive(false);
        timerText.gameObject.SetActive(false);
        if (isAttacker)
            gameoverImage.GetComponent<Image>().sprite = win ? attackWin : attackLose;
        else
            gameoverImage.GetComponent<Image>().sprite = win ? defendWin : defendLose;
        gameoverImage.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(ReturnMenuRoutine());
    }

    private GameObject FindTileByName(string name) {
        for (int i = 0; i < spawnTiles.Length; i++)
            if (spawnTiles[i].name == name)
                return spawnTiles[i];
        for (int i = 0; i < enemyTiles.Length; i++)
            if (enemyTiles[i].name == name)
                return enemyTiles[i];
        for (int i = 0; i < otherTiles.Length; i++)
            if (otherTiles[i].name == name)
                return otherTiles[i];
        return null;
    }

    private void ToggleInput(bool on) {
        for (int i = 0; i < buttons.Length; i++)
            if (!buttons[i].GetComponent<ButtonController>().isCooldown)
                buttons[i].interactable = on;
        for (int i = 0; i < spawnTiles.Length; i++)
            spawnTiles[i].layer = on ? 0 : 2;
    }

    IEnumerator LoginRoutine(string name, string password) {
        WWW www = new WWW(url + "Login.php?name=" + name + "&password=" + password);
        yield return www;
        if (www.text != "NO") {
            id = int.Parse(www.text);
        }
    }

    IEnumerator RegisterRoutine(string name, string password) {
        WWW www = new WWW(url + "Register.php?name=" + name + "&password=" + password);
        yield return www;
        if (www.text != "NO")
        {
            id = int.Parse(www.text);
        }
    }

    IEnumerator PairRoutine(string side) {
        while (true) {
            yield return new WaitForSeconds(pairTryRate);
            WWW www = new WWW(url + "Pair.php?id=" + id + "&roundID=" + roundID + "&side=" + side);
            yield return www;

            if (www.text != "NO") {
                PairInfo pair = JsonUtility.FromJson<PairInfo>(www.text);
                roundID = pair.id;
                if (pair.attackerID > 0 && pair.defenderID > 0) {
                    if (pair.attackerID == id) {
                        enemyID = pair.defenderID;
                        isAttacker = true;
                    }
                    else {
                        enemyID = pair.attackerID;
                        isAttacker = false;
                    }
                    SceneManager.LoadScene(gameSceneName);
                    break;
                }
            }
        }
    }

    IEnumerator RetrieveUnitsRoutine() {
        // Send HTTP request
        WWW www = new WWW(url + "Units.php?id=" + id + "&side=" + (isAttacker ? "attacker" : "defender"));
        yield return www;

        // Parse JSON
        UnitProp[] temp = JsonUtility.FromJson<UnitPropList>(www.text).propList;
        unitName = new string[4];
        remainingUnitNumber = new int[4];
        unitCooldownTime = new float[4];
        for (int i=0; i<temp.Length; i++) {
            unitName[i] = temp[i].name;
            remainingUnitNumber[i] = temp[i].number;
            unitCooldownTime[i] = temp[i].cooldown;
        }

        // Assign button values
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.GetComponent<ButtonController>().SetValue(unitName[i], remainingUnitNumber[i], unitCooldownTime[i]);

        // Allow to start game
        timeOfStart = Time.time;
        StartCoroutine(PollRoutine());
        Time.timeScale = 1f;
        ready = true;
    }

    IEnumerator RequestSpawnRoutine(string tileName, int objectIndex) {
        WWW www = new WWW(url + "SpawnRequest.php?id=" + id + "&roundID=" + roundID + "&side=" + (isAttacker? "attacker" : "defender") + "&tileName=" + tileName + "&index=" + objectIndex);
        yield return www;
        ToggleInput(true);
        if (www.text == "YES") {
            TileMouseHandle tileMouseHandle = FindTileByName(tileName).GetComponent<TileMouseHandle>();
            GameObject tile = spawnTiles[tileMouseHandle.tileIndex];
            GameObject unit = Instantiate(prefabs[objectIndex], tile.transform.position, Quaternion.identity);
            activeUnits.Add(unit);
            remainingUnitNumber[objectIndex]--;
            if (!unit.GetComponentInChildren<Unit>().canMove || unit.GetComponentInChildren<Unit>().speed == 0) {
                tileMouseHandle.isOccupied = true;
                unit.GetComponentInChildren<Unit>().SetAssignedTile(tileMouseHandle);
            }
            buttons[objectIndex].GetComponent<ButtonController>().Cooldown();
        }
    }

    IEnumerator RequestGameoverRoutine(bool win) {
        WWW www = new WWW(url + "Gameover.php?id=" + id + "&roundID=" + roundID + "&win=" + win);
        yield return www;
        if (www.text == "YES")
            Gameover(win);
    }

    IEnumerator PollRoutine() {
        while (true) {
            yield return new WaitForSeconds(pollRate);
            WWW www = new WWW(url + "Poll.php?id=" + id + "&roundID=" + roundID);
            yield return www;
            print("Poll: " + www.text);
            if (www.text.StartsWith("SPAWN")) {
                string json = www.text.Split('_')[1];
                SpawnRequest[] spawnList = JsonUtility.FromJson<SpawnRequestList>(json).spawnList;
                for (int i=0; i<spawnList.Length; i++)
                    SpawnEnemyObject(spawnList[i].tileName, spawnList[i].index);
            }
            else if (www.text.StartsWith("GAMEOVER")) {
                if (!gameover && !isAttacker)
                    Gameover(true);
            }
        }
    }

    IEnumerator ReturnMenuRoutine() {
        yield return new WaitForSeconds(returnMenuTime);
        SceneManager.LoadScene(menuSceneName);
    }
}

[System.Serializable]
class PairInfo {
    public int id;
    public int attackerID;
    public int defenderID;

    public PairInfo(int id) {
        this.id = id;
    }
}

[System.Serializable]
class UnitProp {
    public string name;
    public int number;
    public float cooldown;

    public UnitProp(string name, int number, float cooldown) {
        this.name = name;
        this.number = number;
        this.cooldown = cooldown;
    }
}

[System.Serializable]
class UnitPropList {
    public UnitProp[] propList;
}

[System.Serializable]
class SpawnRequest {
    public int index;
    public string tileName;

    public SpawnRequest(string tileName, int index) {
        this.index = index;
        this.tileName = tileName;
    }
}

[System.Serializable]
class SpawnRequestList {
    public SpawnRequest[] spawnList;
}
