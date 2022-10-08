using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{    
    private int _enemyCount;
    private int _allyCount;
    [SerializeField] private static GameObject ALLY;
    [SerializeField] private static GameObject ENEMY;
    private static string GAME_SCENE = "Game";
    [SerializeField] private int _spawnRate = 1;
    private List<Spawn> _allySpawns;
    private List<Spawn> _enemySpawns;
    public int Round { private set; get; }
    private bool _spawningAllies = false;
    private bool _spawningEnemies = false;

    public static GameManager Instance; //Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

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
