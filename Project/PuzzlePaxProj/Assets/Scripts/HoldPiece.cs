using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPiece : MonoBehaviour
{


    public GameObject originObject;

    public GameObject canvasPrefab;

    private GameObject actualCanvas;

    private GameObject showPiece;

    public GameObject lPrefab;

    public GameObject tPrefab;

    public GameObject zPrefab;

    public GameObject linePrefab;

    public static string heldPiece;

    private TMPro.TextMeshProUGUI textComponent;

    public static int nextPiece;

    private Vector3 showPositionOffset;
    private Vector3 showRotationOffset;


    // Use this for initialization
    void Start()
    {

        actualCanvas = Instantiate(canvasPrefab);
        actualCanvas.transform.position = originObject.transform.position;
        actualCanvas.transform.rotation = originObject.transform.rotation;
        textComponent = actualCanvas.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        textComponent.SetText("Held Piece");
        showPositionOffset = new Vector3(0.5f, 0, 0.5f);
        showRotationOffset = new Vector3(0, -8, 0);
        heldPiece = "nothingHere";

        showPiece = new GameObject("nothingHere");
        //setNextPiece();


    }

    // Update is called once per frame
    void Update()
    {

    }

    //Updates the display piece on call by using current heldPiece name 
    void updateCurrentDisplay()
    {
        GameObject tempPiece = showPiece;
        switch (heldPiece)
        {
            case "tPiece":
                showPiece = Instantiate(tPrefab);
                break;

            case "lPiece":
                showPiece = Instantiate(lPrefab);
                break;

            case "linePiece":
                showPiece = Instantiate(linePrefab);
                break;

            case "zPiece":
                showPiece = Instantiate(zPrefab);
                break;

        }


        showPiece.transform.position = originObject.transform.position + showPositionOffset;
        showPiece.transform.rotation = originObject.transform.rotation;
        showPiece.transform.Rotate(showRotationOffset);
        Destroy(tempPiece);

    }


    //Takes in name of new piece to be held and returns old piece that was being held; Key point is that we only store strings here
    public string holdCurrentPieceAndReturnOldPiece(string toBeHeldPiece)
    {

        

        string tempHeld = heldPiece;


        heldPiece = toBeHeldPiece;

        updateCurrentDisplay();

        return tempHeld;
    }

    





}
