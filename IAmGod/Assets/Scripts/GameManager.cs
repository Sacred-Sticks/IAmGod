using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{    
    public int KillCount { get; private set; }

    [SerializeField] private List<EnemyChance> ENEMIES;
    private int _totalOdds = 0;
    private List<int> _enemyChanceAccumulator;
    [SerializeField] private Character ALLY;
    [SerializeField] private string GAME_SCENE = "Game";

    public List<Spawn> AllySpawns { get { return _allySpawns; } private set { _allySpawns = value; } }
    [SerializeField] private List<Spawn> _allySpawns; //Initialize these with starting ally and enemy spawns
    [SerializeField] private List<Spawn> _enemySpawns;

    [SerializeField] private int _spawnRate = 1;    

    public int MaxRounds { get { return maxRounds; } private set { maxRounds = value; } }
    [SerializeField] private int maxRounds;

    public int Round { get { return round; } private set { round = value; } }
    [SerializeField] private int round = 0;

    private bool _spawningAllies = false;
    private bool _spawningEnemies = false;

    public List<Targetable> AllyList { private set; get; }
    public List<Targetable> EnemyList { private set; get; }

    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    public void OnEnable() {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        AllyList = new List<Targetable>();
        EnemyList = new List<Targetable>();
        _allySpawns = new List<Spawn>();
        _enemySpawns = new List<Spawn>();
        List<Spawn> findSpawns = new List<Spawn>();
        findSpawns.AddRange(FindObjectsOfType<Spawn>());
        foreach (Spawn s in findSpawns) {
            Targetable t = s.gameObject.GetComponent<Targetable>();
            if(t != null)
                _allySpawns.Add(s);
            else
                _enemySpawns.Add(s);
            
        }
        foreach (Spawn s in AllySpawns) //Add spawns to ally targets
            AllyList.Add(s.gameObject.GetComponent<Targetable>());

        _enemyChanceAccumulator = new List<int>();
        foreach (EnemyChance ec in ENEMIES) { //count up odds and add to array for inital enemy array
            _totalOdds += ec.odds;
            _enemyChanceAccumulator.Add(_totalOdds);
        }
        Spawn();
    }

    private int GetSpawnAmount(bool ally)
    {
        System.Random rand = new System.Random();
        float mult = (float)rand.NextDouble();

        if(ally)
            return (int)(_spawnRate * .5f * (0.5f + mult));
        else
            return (int)(_spawnRate * (0.5f + mult));
    }
    private void UpdateOdds(List<EnemyChance> updatedEnemies)
    {
        ENEMIES = updatedEnemies;
        _enemyChanceAccumulator = new List<int>();
        foreach (EnemyChance ec in ENEMIES) {
            _totalOdds += ec.odds;
            _enemyChanceAccumulator.Add(_totalOdds);
        }
    }
    private void Spawn()
    {
        _spawningAllies = true;
        _spawningEnemies = true;
        foreach (Spawn s in _allySpawns)
            StartCoroutine(HandleAllySpawn(GetSpawnAmount(true) / _allySpawns.Count, s));
        foreach (Spawn s in _enemySpawns)
            StartCoroutine(HandleEnemySpawn(GetSpawnAmount(false) / _enemySpawns.Count, s));
    }
    private IEnumerator HandleAllySpawn(int amount, Spawn s)
    {
        Character spawned = null;
        for (int i = 0; i < amount; i++) {
            spawned = Instantiate(ALLY, s.SpawnPoint.transform);
            AllyList.Add(spawned);
            spawned.InitHome(s.transform);
            spawned.gameObject.transform.SetParent(null);
            yield return new WaitForSeconds(.3f);
        }
        _spawningAllies = false;
    }
    private IEnumerator HandleEnemySpawn(int amount, Spawn s) {
        Character spawned = null;
        int pull = 0;
        System.Random rand = new System.Random();
        for (int i = 0; i < amount; i++) {            
            pull = rand.Next(_totalOdds) + 1;
            for (int j = 0; j < _enemyChanceAccumulator.Count; j++) {
                if (pull <= _enemyChanceAccumulator[j]) {
                    spawned = Instantiate(ENEMIES[j].enemy, s.SpawnPoint.transform);
                    EnemyList.Add(spawned);
                    spawned.InitTarget();
                    break;
                }
            }
            yield return new WaitForSeconds(.6f);
        }
        _spawningEnemies = false;
    }
    public void Death(Targetable t)
    {
        if (!t.Ally) {
            EnemyList.Remove(t);
            KillCount++;            
        } else {
            Spawn s = t.gameObject.GetComponent<Spawn>();
            if (s != null)
                AllySpawns.Remove(s);
            AllyList.Remove(t);
        }
        CheckWinLoseConditions();
    }
    private void CheckWinLoseConditions() {
        if (AllyList.Count <= 0 || AllySpawns.Count <= 0)
            EndGame();
        if (EnemyList.Count <= 0 && !_spawningEnemies && !_spawningAllies)
            EndRound();
    }
    private void EndRound()
    {
        Round += 1;
        if (Round > MaxRounds)
            EndGame();
        _spawnRate = (int)(_spawnRate * 1.1f);
        SceneManager.LoadScene(GAME_SCENE);
        //Augment enemy list here, maybe with table TODO
    }
    private void EndGame()
    {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        SceneManager.LoadScene("MainMenu");        
    }
    [System.Serializable]
    private class EnemyChance
    {
        public Character enemy;
        public int odds;
    }
}
