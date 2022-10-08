using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{    
    private int _enemyCount;
    private int _allyCount;
    [SerializeField] private GameObject ALLY;
    [SerializeField] private GameObject ENEMY;
    [SerializeField] private string GAME_SCENE = "Game";

    [SerializeField] private List<Spawn> _allySpawns; //Initialize these with starting ally and enemy spawns
    [SerializeField] private List<Spawn> _enemySpawns;

    [SerializeField] private int _spawnRate = 1;    

    public int Round { private set; get; }
    private bool _spawningAllies = false;
    private bool _spawningEnemies = false;

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
            Instantiate(ally ? ALLY : ENEMY, s.SpawnPoint.transform);
            yield return new WaitForSeconds(.1f);
        }
        if (ally)
            _spawningAllies = false;
        else
            _spawningEnemies = false;
    }

    public void Death(bool ally)
    {
        if (ally)
            _allyCount -= 1;
        else
            _enemyCount -= 1;
        if (_allyCount <= 0)
            EndGame();
        if (_enemyCount <= 5 && (!_spawningEnemies && !_spawningAllies))
            EndRound();
    }
    private void EndRound()
    {
        Round += 1;
        Spawn();
    }
    private void EndGame()
    {
        SceneManager.LoadScene(GAME_SCENE);        
    }
}
