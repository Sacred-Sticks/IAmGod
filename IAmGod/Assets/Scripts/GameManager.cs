using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private int round;

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

    public void Start()
    {
        AllyList = new List<Targetable>();
        foreach(Spawn s in AllySpawns) {
            AllyList.Add(s.gameObject.GetComponent<Targetable>());
        }
        EnemyList = new List<Targetable>();
        _enemyChanceAccumulator = new List<int>();
        foreach (EnemyChance ec in ENEMIES) {
            _totalOdds += ec.odds;
            _enemyChanceAccumulator.Add(_totalOdds);
        }           

        Round = 0;
        EndRound();
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
            StartCoroutine(HandleSpawn(GetSpawnAmount(true) / _allySpawns.Count, s, true));

        foreach (Spawn s in _enemySpawns)
            StartCoroutine(HandleSpawn(GetSpawnAmount(false) / _enemySpawns.Count, s, false));
    }
    private IEnumerator HandleSpawn(int amount, Spawn s, bool ally)
    {
        for(int i = 0; i < amount; i++) {
            System.Random rand = new System.Random();
            if (ally) {
                AllyList.Add(Instantiate(ALLY, s.SpawnPoint.transform));
                if(AllyList[AllyList.Count-1] is Character c) {
                    c.InitHome(s.transform);
                    c.gameObject.transform.SetParent(null);
                }
                    

            } else {
                int pull = rand.Next(_totalOdds) + 1; //Really dumb system for relative probability spawning, work though, I think
                for(int j = 0; j < _enemyChanceAccumulator.Count; j++) {
                    if (pull <= _enemyChanceAccumulator[j]) {
                        EnemyList.Add(Instantiate(ENEMIES[j].enemy, s.SpawnPoint.transform));
                        if (EnemyList[EnemyList.Count - 1] is Character c)
                            c.InitTarget();
                        break;
                    }                      
                }
            }
            yield return new WaitForSeconds(ally ? .3f : .6f);
        }
        if (ally)
            _spawningAllies = false;
        else
            _spawningEnemies = false;
    }

    public void Death(Targetable t)
    {
        if (t.Ally)
        {
            Spawn s = t.gameObject.GetComponent<Spawn>();
            if (s != null)
                AllySpawns.Remove(s);
            AllyList.Remove(t);
        }            
        else {
            EnemyList.Remove(t);
            KillCount++;
        }
        if (AllyList.Count <= 0 || AllySpawns.Count <= 0)
            EndGame();
        if (EnemyList.Count <= 5 && (!_spawningEnemies && !_spawningAllies))
            EndRound();
    }
    private void EndRound()
    {
        Round += 1;
        if (Round > MaxRounds)
            EndGame();
        Spawn();
    }
    private void EndGame()
    {
        SceneManager.LoadScene(GAME_SCENE);        
    }
    [System.Serializable]
    private class EnemyChance
    {
        public Character enemy;
        public int odds;
    }
}
