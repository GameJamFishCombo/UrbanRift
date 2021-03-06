using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState {Spawning, Waiting, Counting}
    static int waveNum = 1;

    [System.Serializable]
    public class Wave
    {
        public string name;
        [Header("Enemy Prefabs to spawn")]
        public Transform[] enemy;
        [Header("Number of enemies per type")]
        public int[] Enemies;
        public float rate;
    }
    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    public float waveCountdown;
    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.Counting;
    public Grid grid;
    public Animator playerAnimation;
    public GameObject transition;
    public GameObject mainMusicObj;
    AudioSource mainMusic;

    void Start() {
        StartCoroutine(SetWaveNumber());
        waveCountdown = timeBetweenWaves;
        playerAnimation = GameObject.Find("Jam_Idle").GetComponent<Animator>();
        mainMusic = mainMusicObj.GetComponent<AudioSource>();
    }

    void Update() {
        if(state == SpawnState.Waiting) {
            if(!EnemyIsAlive()) {WaveCompleted();}
            else {return;}
        } 
        
        if(waveCountdown <= 0) {
            if(state != SpawnState.Spawning) {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        } else {
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SetWaveNumber(){
        yield return new WaitForSeconds(timeBetweenWaves);
    }

    void WaveCompleted() {
        Debug.Log("Wave completed!");
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        //if the next wave is out of bounds of array
        //basically, final wave was completed, trigger cutscene
        Debug.Log("Next Wave #: " + nextWave);
        if(nextWave + 1 >= waves.Length) {
            waveCountdown = 1000f;
            Debug.Log("All Waves Completed");
            string currSceneName = SceneManager.GetActiveScene().name;
            // Debug.Log("Scene Name: " + currSceneName);

            if(currSceneName == "RecordRoom") {
                StartCoroutine(LoadScene("Cutscene3"));
            } else {
                StartCoroutine(LoadScene("Shop"));
            }
        } else {
            nextWave++;
        }
    }

    IEnumerator LoadScene(string scene){
        playerAnimation.SetBool("Victory",true);
        mainMusic.Stop();
        AudioManager.PlaySound("Victory");
        yield return new WaitForSeconds(3f);
        transition.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene);
    }

    bool EnemyIsAlive() {
        searchCountdown -= Time.deltaTime;

        if(searchCountdown <= 0f) {
            searchCountdown = 1f;

            if(GameObject.FindGameObjectWithTag("Enemy") == null) { return false; }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave wave) {
        if(wave.enemy.Length != wave.Enemies.Length) {
            Debug.LogError("Unable to Spawn Wave: " + wave.name + ". Enemy types array and enemy count array sizes do not match.");
        } else if(wave.enemy.Length == wave.Enemies.Length) {
            Debug.Log("Spawning Wave: " + wave.name);
            state = SpawnState.Spawning;

            for(int i = 0; i < wave.enemy.Length; i++) {
                for(int j = 0; j < wave.Enemies[i]; j++) {
                    yield return new WaitForSeconds(1f / wave.rate);
                    grid.SpawnEnemy(wave.enemy[i]);
                    
                }
            }

            Debug.Log("Done Spawning");
            state = SpawnState.Waiting;
            yield break;
        }
    }
}
