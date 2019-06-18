using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour {



    public AudioClip restartNoise;
    private GameObject audioSource;
    // Use this for initialization
    void Start () {
        audioSource = GameObject.FindGameObjectWithTag("audio");
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //Copied from fieldscript because Im too lazy to make an audio script
    void playPieceSound(AudioClip tempClip)
    {
        AudioSource tempSource = audioSource.GetComponent<AudioSource>();

        tempSource.PlayOneShot(tempClip, 1.5f);
    }


    //Called when restart button is pressed
    private void OnTriggerEnter(Collider collision)
    {


        
        if (collision.gameObject.tag == "Controller")
        {
            playPieceSound(restartNoise);
            Invoke("FullyRestart", 0.5f);
        }



    }

    
    void FullyRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
