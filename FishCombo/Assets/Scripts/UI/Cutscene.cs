using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Cutscene : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator[] animators;
    public AudioSource paper;
    public int sceneNum = 0;

    public float exitTimer = 3f;
    public float exitTime = 3f;

    public GameObject transitionOut;
    public bool cutsceneOver;
    public string levelToLoad;
    public Animator credits;

    public AudioSource track;
    void Start(){
        StartCoroutine(FadeMusic.StartFade(track, 4f, 1f));
    }

    void Update(){
        if(cutsceneOver)
            exitTime -= Time.deltaTime;

        if(exitTime <=0){
            //go to credits or end application
            SceneManager.LoadScene(levelToLoad);
        }
        

    }

    void PlayScene(){
        if(sceneNum < animators.Length) {
            if(credits != null)
            credits.Play("Credits");
            animators[sceneNum].Play("In");
        } else{
                StartCoroutine(FadeMusic.StartFade(track, 1.7f, 0f));
                transitionOut.SetActive(true);
                cutsceneOver = true;
        }
    }

    public void Skipscene(){
        AudioManager.PlaySound("PageTurn");
        sceneNum++;
        PlayScene();
    }
}
