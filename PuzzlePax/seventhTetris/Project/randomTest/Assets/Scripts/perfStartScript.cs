using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perfStartScript : MonoBehaviour {

    public GameObject perfPref;
    public GameObject otherSelection;
    public GameObject GameManager;
    public AudioClip startNoise;
    public AudioSource audioSource;
    public GameObject restartPref;
    public GameObject parentObject;




    private FieldScript fieldScript;
    // Use this for initialization
    void Start()
    {
        fieldScript = GameManager.GetComponent<FieldScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Too lazy to make audio script
    void playPieceSound(AudioClip tempClip)
    {
        AudioSource tempSource = audioSource.GetComponent<AudioSource>();

        tempSource.PlayOneShot(tempClip, 1.5f);
    }

    //Called when controller hits button
    private void OnTriggerEnter(Collider collision)
    {

        
        //Debug.Log("collided perf");
        if (collision.gameObject.tag == "Controller")
        {
            fieldScript.cubePref = perfPref;
            fieldScript.enabled = true;
            playPieceSound(startNoise);
            var temp = Instantiate(restartPref);
            temp.transform.position = this.transform.position;
            temp.transform.position = temp.transform.position + new Vector3(-1, 2, 0);
            temp.transform.rotation = this.transform.rotation;
            temp.transform.Rotate(new Vector3(0, 0, 90));

            Destroy(otherSelection);
            Destroy(parentObject);
        }



    }
}
