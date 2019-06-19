using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPiece : MonoBehaviour {


    public GameObject originObject;

    public GameObject canvasPrefab;

    private GameObject actualCanvas;

    private GameObject showPiece;

    public GameObject lPrefab;

    public GameObject tPrefab;

    public GameObject zPrefab;

    public GameObject linePrefab;

    public GameObject cubePrefab;

    public GameObject cornerPrefab;

    private TMPro.TextMeshProUGUI textComponent; 

    public static int nextPiece;

    private Vector3 showPositionOffset;
    private Vector3 showRotationOffset;


    // Use this for initialization
    void Start () {

        actualCanvas = Instantiate(canvasPrefab);
        actualCanvas.transform.position = originObject.transform.position;
        actualCanvas.transform.rotation = originObject.transform.rotation;
        textComponent = actualCanvas.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        textComponent.SetText("Next Piece");
        showPositionOffset = new Vector3(1, 0, 1);
        showRotationOffset = new Vector3(0, -8, 0);
        setNextPiece();
        

	}
	
	// Update is called once per frame
	void Update () {
        
    }

    //Used to get a random piece and show it in the canvas as well as destroy the old showpiece
    public void setNextPiece()
    {
        nextPiece = Random.Range(1, 7);
        GameObject tempPiece = showPiece;
        switch(nextPiece)
        {
            case 1:
                showPiece = Instantiate(tPrefab);
                break;

            case 2:
                showPiece = Instantiate(lPrefab);
                break;

            case 3:
                showPiece = Instantiate(linePrefab);
                break;

            case 4:
                showPiece = Instantiate(zPrefab);
                break;
            case 5:
                showPiece = Instantiate(cubePrefab);
                break;
            case 6: 
                showPiece = Instantiate(cornerPrefab);
                break;

        }

        
        showPiece.transform.position = originObject.transform.position + showPositionOffset;
        showPiece.transform.rotation = originObject.transform.rotation;
        showPiece.transform.Rotate(showRotationOffset);
        Destroy(tempPiece);

    }

    //Called by FieldScript to get the next Piece to spawn, also sets the new next piece for display by calling setNextPiece
    public int getNextPiece()
    {
        int tempPiece = nextPiece;
        setNextPiece();
        return tempPiece;
    }


    


}
