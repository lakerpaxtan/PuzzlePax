using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public class FieldScript : MonoBehaviour
{

    public static PieceObject[,,] actualField;
    public static bool[,,] boolField;
    public static int sizeDim;
    public GameObject cubePref;
    public GameObject cubePrefOutline;
    //public GameObject randomOrigin;
    
    public AudioClip pieceMovementSound;
    public AudioClip pieceLockedSound;
    public AudioClip pieceRotateSound;
    public GameObject audioSource;
    public AudioClip floorRemovedSound;
    public AudioClip rejectedSound;
    public AudioClip holdSound;
    public GameObject textObject;
    public Material lMaterial;
    public Material lineMaterial;
    public Material zMaterial;
    public Material lMaterialST;
    public Material lineMaterialST;
    public Material zMaterialST;
    public Material tMaterialST;
    public float currentSpeed;
    public float currentDecrement;
    public float currentDecrementRate;
    public float minimumSpeed;
    //public static GameObject spherePref2;


    private bool shouldSpawn;
    private Vector3 spawnTop;
    private PieceObject currentPiece;
    private static HashSet<PieceObject> allPieces;
    private int currentScore;
    private int currentPieces;
    private RandomPiece randomScript;
    private HoldPiece holdScript;
    private bool hasHeldBool;
    
    public static int yOffset;



    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;

    public SteamVR_Action_Boolean leftAction;
    public SteamVR_Action_Boolean downAction;
    public SteamVR_Action_Boolean backwardAction;
    public SteamVR_Action_Boolean rightAction;
    public SteamVR_Action_Boolean forwardAction;
    public SteamVR_Action_Boolean holdAction;
    public SteamVR_Action_Boolean rotateX;
    public SteamVR_Action_Boolean rotateY;
    public SteamVR_Action_Boolean rotateZ;
    public SteamVR_Action_Boolean rotateXNeg;
    public SteamVR_Action_Boolean rotateYNeg;
    public SteamVR_Action_Boolean rotateZNeg;
    public SteamVR_Action_Vector2 trackpadLeftPosition;
    public SteamVR_Action_Vector2 trackpadRightPosition;


    public SteamVR_Action_Boolean leftActionJoy;
    //public SteamVR_Action_Boolean downActionJoy;
    public SteamVR_Action_Boolean backwardActionJoy;
    public SteamVR_Action_Boolean rightActionJoy;
    public SteamVR_Action_Boolean forwardActionJoy;

    public SteamVR_Action_Boolean rotateXJoy;
    public SteamVR_Action_Boolean rotateZJoy;
    public SteamVR_Action_Boolean rotateZNegJoy;
    public SteamVR_Action_Boolean rotateXNegJoy;
    public SteamVR_Action_Vector2 leftJoyPosition;
    public SteamVR_Action_Vector2 rightJoyPosition;
    public SteamVR_Action_Boolean instantPlacement;


    // Use this for initialization
    void Start()
    {
        hasHeldBool = false;
        yOffset = 5;
        randomScript = this.gameObject.GetComponent<RandomPiece>();
        holdScript = this.gameObject.GetComponent<HoldPiece>();
        currentSpeed = 2.5f;
        currentScore = 0;
        currentPieces = 0;
        allPieces = new HashSet<PieceObject>();
        sizeDim = 7;
        actualField = new PieceObject[sizeDim, sizeDim + yOffset, sizeDim];
        boolField = new bool[sizeDim, sizeDim + yOffset, sizeDim];
        shouldSpawn = true;
        spawnTop = new Vector3((int)Mathf.Floor(sizeDim / 2), sizeDim - 2 + yOffset, (int)Mathf.Floor(sizeDim / 2));
        setBoolFieldToAllFalse();

        InvokeRepeating("decreaseCurrentSpeed", 0f, currentDecrementRate);
        Invoke("timedDownMovement", currentSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        
        //Used to call the canvas updater for score and block numbers
        updateCanvas();

       //Used to trigger spawn function on boolean swap
        if (shouldSpawn)
        {
            spawnRandom();
        }


        //Debug.Log("XPos1: " + leftJoyPosition.GetAxis(leftHand).x + "XPos2: " + leftJoyPosition.GetAxis(leftHand).y);



        if (Input.GetButtonDown("Drop") || instantPlacement.GetStateDown(rightHand) || instantPlacement.GetStateDown(leftHand))
        {
            Debug.Log("moving all the day down");
            moveCurrentPieceAllTheWayDown();

        }


        //Button to hold a piece; Currently assigned to right trigger
        if (holdAction.GetStateDown(rightHand) || Input.GetButtonDown("Hold"))
        {
            if (!hasHeldBool)
            {
                playPieceSound(holdSound);
                hasHeldBool = true;
                holdCurrentPiece();
               

            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }

        //Button to move Piece leftwards
        if ( leftActionJoy.GetStateDown(leftHand)|| Input.GetButtonDown("Left") || (leftAction.GetStateDown(leftHand) && trackpadLeftPosition.GetAxis(leftHand).x < -0.5f && trackpadLeftPosition.GetAxis(leftHand).y < 0.45f && trackpadLeftPosition.GetAxis(leftHand).y > -0.45f))
        {
            if (currentPiece.possibleLeftwardMovement())
            {
                playPieceSound(pieceMovementSound);
                movePieceLeft();
                reflectionUpdate();
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }

        //Button to move piece Rightwards
        if (rightActionJoy.GetStateDown(leftHand) || Input.GetButtonDown("Right") || (rightAction.GetStateDown(leftHand) && trackpadLeftPosition.GetAxis(leftHand).x > 0.5f && trackpadLeftPosition.GetAxis(leftHand).y < 0.45f && trackpadLeftPosition.GetAxis(leftHand).y > -0.45f))
        {
            if (currentPiece.possibleRightwardMovement())
            {
                playPieceSound(pieceMovementSound);
                movePieceRight();
                reflectionUpdate();
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }


        //Button to move piece Forwards
        if (forwardActionJoy.GetStateDown(leftHand) || Input.GetButtonDown("Forward") || (forwardAction.GetStateDown(leftHand) && trackpadLeftPosition.GetAxis(leftHand).y > 0.5f && trackpadLeftPosition.GetAxis(leftHand).x < 0.45f && trackpadLeftPosition.GetAxis(leftHand).x > -0.45f))
        {
            if (currentPiece.possibleForwardMovement())
            {
                playPieceSound(pieceMovementSound);
                movePieceForward();
                reflectionUpdate();
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }


        //Button to move piece backwards
        if (backwardActionJoy.GetStateDown(leftHand) || Input.GetButtonDown("Backward") || (backwardAction.GetStateDown(leftHand) && trackpadLeftPosition.GetAxis(leftHand).y < -0.5f && trackpadLeftPosition.GetAxis(leftHand).x < 0.45f && trackpadLeftPosition.GetAxis(leftHand).x > -0.45f))
        {
            if (currentPiece.possibleBackwardMovement())
            {
                playPieceSound(pieceMovementSound);
                movePieceBackward();
                reflectionUpdate();
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }


        //Button to accelerate tetris piece downward movement by one click; Currently left trigger 
        if (Input.GetButtonDown("Down") || downAction.GetStateDown(leftHand) || downAction.GetStateDown(rightHand))
        {

            if (currentPiece.possibleDownwardMovement())
            {
                //Debug.Log("can move piece down");
                playPieceSound(pieceMovementSound);
                movePieceDown(currentPiece);
                reflectionUpdate();
            }
            else
            {
                //Debug.Log("cant move piece down");
                Debug.Log("input cant move down");
                lockPieceAndSwapSpawn();
            }
        }

        //Button for Rotating Y, Currently right grip
        if (Input.GetButtonDown("rotateY") || rotateY.GetStateDown(rightHand))
        {
            if (currentPiece.possibleRotateYAxis(1))
            {
                //Debug.Log("rotateY");
                rotateYAxis(1);
                reflectionUpdate();
                playPieceSound(pieceRotateSound);
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }




        //Button for Rotating Z, Currently right Button
        if ((rotateZJoy.GetStateDown(rightHand) && rightJoyPosition.GetAxis(rightHand).x > 0.5f && rightJoyPosition.GetAxis(rightHand).y < 0.45f && rightJoyPosition.GetAxis(rightHand).y > -0.45f) || Input.GetButtonDown("rotateZ") || (rotateZ.GetStateDown(rightHand) && trackpadRightPosition.GetAxis(rightHand).x > 0.5f && trackpadRightPosition.GetAxis(rightHand).y < 0.45f && trackpadRightPosition.GetAxis(rightHand).y > -0.45f))
        {
            if (currentPiece.possibleRotateZAxis(1))
            {
                //Debug.Log("rotateZ");
                playPieceSound(pieceRotateSound);
                rotateZAxis(1);
                reflectionUpdate();
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }



        //Button for Rotating X, Currently Top Button
        if ((rotateXJoy.GetStateDown(rightHand) && rightJoyPosition.GetAxis(rightHand).y > 0.5f && rightJoyPosition.GetAxis(rightHand).x < 0.45f && rightJoyPosition.GetAxis(rightHand).x > -0.45f) || Input.GetButtonDown("rotateX") || (rotateX.GetStateDown(rightHand) && trackpadRightPosition.GetAxis(rightHand).y > 0.5f && trackpadRightPosition.GetAxis(rightHand).x < 0.45f && trackpadRightPosition.GetAxis(rightHand).x > -0.45f))
        {   
            if (currentPiece.possibleRotateXAxis(1))
            {
                //Debug.Log("rotateX");
                playPieceSound(pieceRotateSound);
                rotateXAxis(1);
                reflectionUpdate();
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }




        //Button for Rotating YNeg, Currently left grip
        if (rotateYNeg.GetStateDown(leftHand))
        {
            if (currentPiece.possibleRotateYAxis(-1))
            {
                //Debug.Log("IT WORKED SUPPOSEDLY");
                //Debug.Log("rotateYNeg");
                playPieceSound(pieceRotateSound);
                rotateYAxis(-1);
                reflectionUpdate();
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }




        //Button for Rotating ZNeg, Currently left Button
        if ((rotateZNegJoy.GetStateDown(rightHand) && rightJoyPosition.GetAxis(rightHand).x < -0.5f && rightJoyPosition.GetAxis(rightHand).y < 0.45f && rightJoyPosition.GetAxis(rightHand).y > -0.45f) || rotateZNeg.GetStateDown(rightHand) && (trackpadRightPosition.GetAxis(rightHand).x < -0.5f && trackpadRightPosition.GetAxis(rightHand).y < 0.45f && trackpadRightPosition.GetAxis(rightHand).y > -0.45f))
        {
            if (currentPiece.possibleRotateZAxis(-1))
            {
                //Debug.Log("rotateZNeg");
                playPieceSound(pieceRotateSound);
                rotateZAxis(-1);
                reflectionUpdate();
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }




        //Button for Rotating XNeg, Currently Bottom Button
        if ((rotateXNegJoy.GetStateDown(rightHand) && rightJoyPosition.GetAxis(rightHand).y < -0.5f && rightJoyPosition.GetAxis(rightHand).x < 0.45f && rightJoyPosition.GetAxis(rightHand).x > -0.45f) || rotateXNeg.GetStateDown(rightHand) && (trackpadRightPosition.GetAxis(rightHand).y < -0.5f && trackpadRightPosition.GetAxis(rightHand).x < 0.45f && trackpadRightPosition.GetAxis(rightHand).x > -0.45f))
        {
            if (currentPiece.possibleRotateXAxis(-1))
            {
                //Debug.Log("rotateXNeg");
                playPieceSound(pieceRotateSound);
                rotateXAxis(-1);
                reflectionUpdate();
            }
            else
            {
                playPieceSound(rejectedSound);
            }
        }


    }


    void reflectionUpdate()
    {
        if(currentPiece == null || currentPiece.reflectionObjects == null)
        {
            return;
        }
        var currActualList = currentPiece.getActualObjects();
        var currReflectionNode = currentPiece.reflectionObjects.First;
        var currActualNode = currActualList.First;

        GameObject currReflectionValue;
        GameObject currActualValue;

        //Debug.Log(tempList.Count);
        while (currReflectionNode != null)
        {
            currReflectionValue = currReflectionNode.Value;
            currActualValue = currActualNode.Value;
            //Debug.Log("recursion tick");
            //removePiece(currNode.Value);
            //Destroy(currValue);
            currReflectionValue.transform.position = currActualValue.transform.position;

            currReflectionNode = currReflectionNode.Next;
            currActualNode = currActualNode.Next;
        }

        reflectionGravityCheck();
    }

    void createReflectionPiece(LinkedList<Vector3> piecePositions, PieceObject tempPiece)
    {
        LinkedList<Vector3> newList = new LinkedList<Vector3>();

        foreach (var tempVector in piecePositions)
        {
            newList.AddLast(new Vector3(tempVector.x, tempVector.y - 1, tempVector.z ));
        }

        tempPiece.reflectionObjects = createPieceGameObject(newList, true, tempPiece.getPieceName());

        var reflectionPieces = tempPiece.reflectionObjects;

        assignReflectionMaterials(reflectionPieces, tempPiece.getPieceName());
        reflectionGravityCheck();

       


    }

    void assignReflectionMaterials(LinkedList<GameObject> reflectionObjects, string pieceName)
    {
        Material reflectionMat = tMaterialST;

        switch (pieceName)
        {
            case ("lPiece"):
                reflectionMat = lMaterialST;
                break;

            case ("tPiece"):
                reflectionMat = tMaterialST;
                break;

            case ("linePiece"):
                reflectionMat = lineMaterialST;
                break;

            case ("zPiece"):
                reflectionMat = zMaterialST;
                break;
        }

        var currNode = reflectionObjects.First;

        GameObject currValue;

        //Debug.Log(tempList.Count);
        while (currNode != null)
        {
            currValue = currNode.Value;
            //Debug.Log("recursion tick");
            //removePiece(currNode.Value);
            currValue.GetComponent<MeshRenderer>().material = reflectionMat;
            currNode = currNode.Next;
        }


    }


    //Function called when you want to hold a piece. Currently used a lot of functionality copied from spawnrandom 
    void holdCurrentPiece()
    {
        var tempPiece = currentPiece;
        removeReflection();
        removePiece(currentPiece);

        LinkedList<Vector3> tempSet2 = new LinkedList<Vector3>();
        //string tempString;

        var tempString = holdScript.holdCurrentPieceAndReturnOldPiece(tempPiece.getPieceName());
        if(tempString == "nothingHere")
        {
            booleanSpawnSwap();
            return;
        }
        else if(tempString == "lPiece")
        {
            tempSet2 = lPieceSet();
        }
        else if (tempString == "zPiece")
        {
            tempSet2 = zPieceSet();
        }
        else if (tempString == "linePiece")
        {
            tempSet2 = linePieceSet();
        }
        else if (tempString == "tPiece")
        {
            tempSet2 = tPieceSet();
        }

        if (lossCheck(tempSet2))
        {
            Debug.Log("you dead mate");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        else
        {
            PieceObject newObject = createPiece(tempSet2, true, tempString);
            currentPiece = newObject;
            createReflectionPiece(tempSet2, currentPiece);
        }



    }


    //Silly function used to update the canvas instead of just calling an update function whenever it needs to change b/c I'm lazy
    void updateCanvas()
    {
        var tempText = textObject.GetComponent<TMPro.TextMeshProUGUI>();
        tempText.SetText("Score: {0} \n\nBlocks: {1}", currentScore, currentPieces);
        
    }


    void removeFullWallZ(int zVal)
    {
        playPieceSound(floorRemovedSound);
        //currentScore += 1000;
        //Debug.Log("removing full wall Z");
        LinkedList<PieceObject> tempList = new LinkedList<PieceObject>();
        foreach (PieceObject element in allPieces)
        {
            if (element.isFullyPartOfWallZ(zVal, sizeDim, yOffset))
            {
                //Debug.Log("fully part of wall tick");
                //removePiece(element);
                tempList.AddLast(element);
            }
            else if (element.isPartiallyPartOfWallZ(zVal, sizeDim, yOffset))
            {
                //Debug.Log("partially part of wall tick");
                removeBlockPositions(element);
                removePieceFromWallZ(element, zVal);
                addBlockPositions(element);
            }
        }

        var currNode = tempList.First;

        //Debug.Log(tempList.Count);
        while (currNode != null)
        {
            //Debug.Log("recursion tick");
            removePiece(currNode.Value);
            currNode = currNode.Next;
        }

        gravityCheck();


    }


    bool isFloating(GameObject tempObject)
    {
        
        if(boolField[(int)Mathf.Round(tempObject.transform.position.x), (int)Mathf.Round(tempObject.transform.position.y) - 1, (int) Mathf.Round(tempObject.transform.position.z)])
        {
            return false;
        }

        return true; 


    }

    void gravityCheck()
    {
        //Debug.Log("grav check");
        bool somethingMoved = true;

        while (somethingMoved)
        {
            //Debug.Log("while tick");
            somethingMoved = false;
            foreach (PieceObject elementPiece in allPieces)
            {
                //Debug.Log("piece tick");
                //Debug.Log(elementPiece.getActualObjects().Count);
                if (elementPiece.possibleDownwardMovement())
                {
                    //Debug.Log("moved down");
                    somethingMoved = true;
                    movePieceDown(elementPiece);
                }
            }
        }


    }


    void moveCurrentPieceAllTheWayDown()
    {
        bool somethingMoved = true;

        while (somethingMoved)
        {
            //Debug.Log("while tick");
            somethingMoved = false;

            if (currentPiece.possibleDownwardMovement())
            {

                //Debug.Log("moved down");
                somethingMoved = true;
                movePieceDown(currentPiece);
            }

        }

        lockPieceAndSwapSpawn();

    }

    void reflectionGravityCheck()
    {
        //Debug.Log("grav check");
        bool somethingMoved = true;

        while (somethingMoved)
        {
            //Debug.Log("while tick");
            somethingMoved = false;
            
            if (currentPiece.possibleReflectionDownward())
            {
                
                //Debug.Log("moved down");
                somethingMoved = true;
                currentPiece.moveReflectionDownward();
            }
            
        }
        return;


    }



    void removeFullWallX(int xVal)
    {
        playPieceSound(floorRemovedSound);
        //currentScore += 1000;
        Debug.Log("removing full wall X");
        LinkedList<PieceObject> tempList = new LinkedList<PieceObject>();
        foreach (PieceObject element in allPieces)
        {
            if (element.isFullyPartOfWallX(xVal, sizeDim, yOffset))
            {
                Debug.Log("fully part of wall tick");
                //removePiece(element);
                tempList.AddLast(element);
            }
            else if (element.isPartiallyPartOfWallX(xVal, sizeDim, yOffset))
            {
                Debug.Log("partially part of wall tick");
                removeBlockPositions(element);
                removePieceFromWallX(element, xVal);
                addBlockPositions(element);
            }
        }

        
        var currNode = tempList.First;
        
        //Debug.Log(tempList.Count);
        while (currNode != null)
        {
            Debug.Log("recursion tick");
            removePiece(currNode.Value);
            currNode = currNode.Next;

        }

        gravityCheck();



    }





    //Removes an entire floor according to tetris rules 
    void removeFullFloor(int floorVal)
    {
        playPieceSound(floorRemovedSound);
        //currentScore += 1000;
        Debug.Log("removing full floor");
        foreach (PieceObject element in allPieces)
        {

            if (element.isTouchingFloor(floorVal))
            {
                removeBlockPositions(element);
                //Debug.Log("element is touching floor");
                removePieceFromFloor(element, floorVal);
                addBlockPositions(element);
            }
            else
            {
                Debug.Log("element is not touching floor");
                if (element.isAboveFloorVal(floorVal))
                {
                    removeBlockPositions(element);
                    element.moveDownOnePosition();
                    addBlockPositions(element);
                }
                
            }
        }

    }

    
    //Never called. Just used to test stuff
    void testLinkedList(LinkedList<GameObject> tempList)
    {
        var currNode = tempList.First;
        while (currNode != null)
        {
            Debug.Log("one tick");
            currNode = currNode.Next;
        }
    }


    //Confusing Name; Takes in a piece that is intersecting a floor, edits it so that everything on that floor is removed and everything else is moved down according to gravity 
    void removePieceFromFloor(PieceObject tempPiece, int floorVal)
    {
        
        var tempList = tempPiece.getActualObjects();
        var currNode = tempList.First;
        GameObject currObject;
        //Debug.Log(tempList.Count);
        while (currNode != null)
        {
            //Debug.Log("one Node");
            currObject = currNode.Value;
            currNode = currNode.Next;
            //Debug.Log(currNode);
            if (currObject.transform.position.y == floorVal)
            {
                //Debug.Log("0 y value");
                tempPiece.removeObjectFromActualObjects(currObject);
                Destroy(currObject);
            }
            else
            {
                if(currObject.transform.position.y > floorVal)
                {
                    currObject.transform.Translate(new Vector3(0, -1, 0), Space.World);
                }
                
                
            }
          
        }
        
    }

    void removePieceFromWallX(PieceObject tempPiece, int xVal)
    {

        var tempList = tempPiece.getActualObjects();
        var currNode = tempList.First;
        GameObject currObject;
        //bool tempBool;
        //tempBool = false;
        Debug.Log(tempList.Count);

        while (currNode != null)
        {
            //Debug.Log("one Node");
            currObject = currNode.Value;
            currNode = currNode.Next;
            //Debug.Log(currNode);
            if (currObject.transform.position.y <= sizeDim + yOffset - 4 && currObject.transform.position.x == xVal)
            {
                Debug.Log("gameobject is part of wall and not above");
                //Debug.Log("0 y value");
                tempPiece.removeObjectFromActualObjects(currObject);
                Destroy(currObject);


            }
            

        }

        
    }

    void removePieceFromWallZ(PieceObject tempPiece, int zVal)
    {

        var tempList = tempPiece.getActualObjects();
        var currNode = tempList.First;
        GameObject currObject;
        //bool tempBool;
        //tempBool = false;
        Debug.Log(tempList.Count);

        while (currNode != null)
        {
            Debug.Log("recursion tick2");
            //Debug.Log("one Node");
            currObject = currNode.Value;
            currNode = currNode.Next;
            //Debug.Log(currNode);
            
            if (currObject.transform.position.y <= sizeDim + yOffset - 4 && currObject.transform.position.z == zVal)
            {
                Debug.Log("gameobject is part of wall and not above");
                //Debug.Log("0 y value");
                tempPiece.removeObjectFromActualObjects(currObject);
                Destroy(currObject);

                
            }
            

            


        }

        

       
    }


    

    //checks to see if the yVal level is full
    bool isFullFloor(int yVal)
    {
        
        for (int x = 0; x < sizeDim; x++)
        {
            for (int z = 0; z < sizeDim; z++)
            {
                if (boolField[x, yVal, z] == false)
                {
                    
                    return false; 
                }
            }
        }
        return true;
    }

    //checks to see if zVal wall is full
    bool isFullWallZ(int zVal)
    {
        for (int x = 0; x < sizeDim; x++)
        {
            for (int y = 0; y < sizeDim-3 + yOffset; y++)
            {
                if (boolField[x, y, zVal] == false)
                {

                    return false;
                }
            }
        }
        return true;
    }


    //checks to see if xVal wall is full
    bool isFullWallX(int xVal)
    {
        for (int z = 0; z < sizeDim; z++)
        {
            for (int y = 0; y < sizeDim-3 + yOffset; y++)
            {
                if (boolField[xVal, y, z] == false)
                {

                    return false;
                }
            }
        }
        return true;
    }

    //Used to play sounds given an audio clip
    void playPieceSound(AudioClip tempClip)
    {
        AudioSource tempSource = audioSource.GetComponent<AudioSource>();

        tempSource.PlayOneShot(tempClip, 1.5f);
    }

    //Used to remove the oueline from the highlighted piece 
    void removeOutline(PieceObject tempPiece, string tempString)
    {

        var tempSet = tempPiece.getActualObjects();
        removePiece(tempPiece);
        //allPieces.Remove(tempPiece);
        createPiece(getVector3Set(tempPiece.getActualObjects()), false, tempString);
    }


    //Returns the vector3 set of all points in the piece from the list of gameobjects
    LinkedList<Vector3> getVector3Set(LinkedList<GameObject> tempObjects)
    {
        var tempSet = new LinkedList<Vector3>();
        foreach (var element in tempObjects)
        {
            tempSet.AddLast(new Vector3(element.transform.position.x, element.transform.position.y, element.transform.position.z));
        }
        return tempSet;
    }


    //Given a set of points, create a pieceobject in that spot that will be put into the arrays and bookkeeping currectly
    PieceObject createPiece(LinkedList<Vector3> tempSet, bool outlineBool, string tempString)
    {
        currentPieces += 1;
        PieceObject newPiece = new PieceObject(createPieceGameObject(tempSet, outlineBool, tempString), tempString);
        addBlockPositions(newPiece);
        allPieces.Add(newPiece);
        return newPiece;
    }

    //Takes in a piece object and completely removes from exists (including gameobjects and arrays etc)
    void removePiece(PieceObject tempPiece)
    {
        
        currentPieces -= 1;
        allPieces.Remove(tempPiece);
        removeBlockPositions(tempPiece);
        foreach (var tempObject in tempPiece.getActualObjects())
        {
            Destroy(tempObject);
        }

    }

    void removeReflection()
    {
        foreach (var element in currentPiece.reflectionObjects)
        {
            Destroy(element);
        }
    }


    //See name
    void movePieceDown(PieceObject tempPiece)
    {

        removeBlockPositions(tempPiece);
        tempPiece.moveDownOnePosition();
        addBlockPositions(tempPiece);



    }

    //See name
    void movePieceLeft()
    {
        removeBlockPositions(currentPiece);
        currentPiece.moveLeftOnePosition();
        addBlockPositions(currentPiece);

    }

    //See name
    void movePieceRight()
    {
        removeBlockPositions(currentPiece);
        currentPiece.moveRightOnePosition();
        addBlockPositions(currentPiece);

    }

    //See name
    void movePieceForward()
    {
        removeBlockPositions(currentPiece);
        currentPiece.moveForwardOnePosition();
        addBlockPositions(currentPiece);

    }

    //See name
    void movePieceBackward()
    {
        removeBlockPositions(currentPiece);
        currentPiece.moveBackwardOnePosition();
        addBlockPositions(currentPiece);

    }



    //Rotates the piece along the Y Axis
    void rotateYAxis(int direction)
    {
        removeBlockPositions(currentPiece);
        currentPiece.rotateYAxis(direction);
        addBlockPositions(currentPiece);
    }

    //Rotates the piece along the Z Axis
    void rotateZAxis(int direction)
    {
        removeBlockPositions(currentPiece);
        currentPiece.rotateZAxis(direction);
        addBlockPositions(currentPiece);
    }


    //Rotates the piece along the X Axis
    void rotateXAxis(int direction)
    {
        removeBlockPositions(currentPiece);
        currentPiece.rotateXAxis(direction);
        addBlockPositions(currentPiece);
    }






    //Used at the beginning because c# is garbo
    void setBoolFieldToAllFalse()
    {
        for (int i = 0; i < sizeDim; i++)
        {
            for (int j = 0; j < sizeDim + yOffset; j++)
            {
                for (int k = 0; k < sizeDim; k++)
                {
                    boolField[i, j, k] = false;
                }
            }
        }
    }





    //Used everywhere to take in a piece object and remove it from boolean arrays and piece object arrays 
    void removeBlockPositions(PieceObject tempObject)
    {
        var tempSet = tempObject.getActualObjects();
        foreach (var element in tempSet)
        {
            actualField[(int)Mathf.Round(element.transform.position.x), (int)Mathf.Round(element.transform.position.y), (int)Mathf.Round(element.transform.position.z)] = null;
            boolField[(int)Mathf.Round(element.transform.position.x), (int)Mathf.Round(element.transform.position.y), (int)Mathf.Round(element.transform.position.z)] = false;
        }
    }


    //Used everywhere to take in a piece object and add it to the boolean arrays and piece object arrays
    void addBlockPositions(PieceObject tempObject)
    {
        var tempSet = tempObject.getActualObjects();
        foreach (var element in tempSet)
        {
            actualField[(int)Mathf.Round(element.transform.position.x), (int)Mathf.Round(element.transform.position.y), (int)Mathf.Round(element.transform.position.z)] = tempObject;
            boolField[(int)Mathf.Round(element.transform.position.x), (int)Mathf.Round(element.transform.position.y), (int)Mathf.Round(element.transform.position.z)] = true;
        }
    }

    
    //Used by creategamepiece to make the actual cube
    GameObject createIndividualCube(Vector3 tempVec)
    {
        var currentCube = Instantiate(cubePrefOutline);
        cubePrefOutline.transform.position = tempVec;
        return currentCube;
    }




    //the main function that gets called whenever a piece needs to be spawned. Triggered by spawn bool being true
    void spawnRandom()
    {
        //Debug.Log(actualField);
        shouldSpawn = false;
        int rand = randomScript.getNextPiece();
        Debug.Log("spawn rand" );
        //Debug.Log(rand);

        LinkedList<Vector3> tempSet2 = new LinkedList<Vector3>();
        //Vector3 tempVector;
        string tempString;

        switch (rand)
        {


            //t piece
            case 1:

                tempSet2 = tPieceSet();
                tempString = "tPiece";
                break;
            //l piece
            case 2:
                tempSet2 = lPieceSet();
                tempString = "lPiece";
                break;

            case 3:
                tempSet2 = linePieceSet();
                tempString = "linePiece";
                break;

            case 4:
                tempSet2 = zPieceSet();
                tempString = "zPiece";
                break;

            default:
                tempString = "broken";
                break;

        }

        if (lossCheck(tempSet2))
        {
            Debug.Log("you dead mate");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        else
        {
            PieceObject newObject = createPiece(tempSet2, true, tempString);
            currentPiece = newObject;
            createReflectionPiece(tempSet2, currentPiece);
        }



    }

    //returns a set of vector points for an l piece
    LinkedList<Vector3> lPieceSet()
    {
        LinkedList<Vector3> tempSet2 = new LinkedList<Vector3>();
        Vector3 tempVector;
        tempSet2.AddLast(spawnTop);
        tempVector = new Vector3(spawnTop.x, spawnTop.y, spawnTop.z + 1);
        tempSet2.AddLast(tempVector);
        tempVector = new Vector3(spawnTop.x - 1, spawnTop.y, spawnTop.z);
        tempSet2.AddLast(tempVector);
        tempVector = new Vector3(spawnTop.x - 2, spawnTop.y, spawnTop.z);
        tempSet2.AddLast(tempVector);

        return tempSet2;

    }

    //returns a set of vector points for a z piece
    LinkedList<Vector3> zPieceSet()
    {
        LinkedList<Vector3> tempSet2 = new LinkedList<Vector3>();
        Vector3 tempVector;
        tempSet2.AddLast(spawnTop);
        tempVector = new Vector3(spawnTop.x + 1, spawnTop.y, spawnTop.z);
        tempSet2.AddLast(tempVector);
        tempVector = new Vector3(spawnTop.x, spawnTop.y, spawnTop.z - 1);
        tempSet2.AddLast(tempVector);
        tempVector = new Vector3(spawnTop.x - 1, spawnTop.y, spawnTop.z - 1);
        tempSet2.AddLast(tempVector);

        return tempSet2;

    }

    //returns a set of vector points for a line piece
    LinkedList<Vector3> linePieceSet()
    {
        LinkedList<Vector3> tempSet2 = new LinkedList<Vector3>();
        Vector3 tempVector;
        tempSet2.AddLast(spawnTop);
        tempVector = new Vector3(spawnTop.x - 1, spawnTop.y, spawnTop.z);
        tempSet2.AddLast(tempVector);
        tempVector = new Vector3(spawnTop.x - 2, spawnTop.y, spawnTop.z);
        tempSet2.AddLast(tempVector);
        tempVector = new Vector3(spawnTop.x - 3, spawnTop.y, spawnTop.z);
        tempSet2.AddLast(tempVector);

        return tempSet2;

    }


    //returns a set of vector points for a t piece
    LinkedList<Vector3> tPieceSet()
    {
        LinkedList<Vector3> tempSet2 = new LinkedList<Vector3>();
        Vector3 tempVector;
        tempSet2.AddLast(spawnTop);
        tempVector = new Vector3(spawnTop.x, spawnTop.y, spawnTop.z + 1);
        tempSet2.AddLast(tempVector);
        tempVector = new Vector3(spawnTop.x - 1, spawnTop.y, spawnTop.z);
        tempSet2.AddLast(tempVector);
        tempVector = new Vector3(spawnTop.x + 1, spawnTop.y, spawnTop.z);
        tempSet2.AddLast(tempVector);

        return tempSet2;

    }

    //Silly function that doesn't need to exist 
    void booleanSpawnSwap()
    {
        shouldSpawn = true;
    }


    //Used to decrement speed
    void decreaseCurrentSpeed()
    {
        if (currentSpeed > minimumSpeed)
        {
            currentSpeed -= currentDecrement;
        }
        
    }


    //Called every XXX to move current piece down
    void timedDownMovement()
    {
        if (currentPiece != null)
        {
            if (currentPiece.possibleDownwardMovement())
            {
                //Debug.Log("can move piece down");
                movePieceDown(currentPiece);
            }
            else
            {
                //Debug.Log("cant move piece down");
                lockPieceAndSwapSpawn();
            }
        }
        else
        {
            Debug.Log("current piece is null");
        }

        Invoke("timedDownMovement", currentSpeed);
    }



    //Used to create the actual gameobject cubes from a set of points and piece type
    LinkedList<GameObject> createPieceGameObject(LinkedList<Vector3> tempSet, bool outlineBool, string tempString)
    {
        LinkedList<GameObject> returnSet = new LinkedList<GameObject>();
        foreach (var element in tempSet)
        {
            GameObject currentCube;
            if (outlineBool)
            {
                currentCube = Instantiate(cubePrefOutline);
            }
            else
            {
                currentCube = Instantiate(cubePref);  
            }
            currentCube.transform.position = new Vector3(element.x, element.y, element.z);
            returnSet.AddLast(currentCube);
            if (tempString == "lPiece")
            {
                currentCube.GetComponent<MeshRenderer>().material = lMaterial; 
            }
            if (tempString == "linePiece")
            {
                currentCube.GetComponent<MeshRenderer>().material = lineMaterial;
            }
            if(tempString == "zPiece")
            {
                currentCube.GetComponent<MeshRenderer>().material = zMaterial;
            }






        }

        return returnSet;

    }





    //locks the current piece in its spot, swaps it for no outline piece, swaps spawn boolean
    void lockPieceAndSwapSpawn()
    {

        removeReflection();
        removeOutline(currentPiece, currentPiece.getPieceName());
        hasHeldBool = false;
        playPieceSound(pieceLockedSound);
        currentPiece = null;
        bool oneLoop = true;
        int comboCounter = 0;
        while (oneLoop)
        {
            oneLoop = false;

            for (int i = 0; i < sizeDim + yOffset; i++)
            { 
                if (isFullFloor(i))
                {
                    comboCounter += 1;
                    removeFullFloor(i);
                    Debug.Log("done floor " + i);
                    oneLoop = true;
                }

            }

            for (int i = 0; i < sizeDim; i++)
            {
                if (isFullWallX(i))
                {
                    comboCounter += 1;
                    removeFullWallX(i);
                    Debug.Log("done wallX" + i);
                    oneLoop = true;
                }

            }

            for (int i = 0; i < sizeDim; i++)
            {
                if (isFullWallZ(i))
                {
                    comboCounter += 1;
                    removeFullWallZ(i);
                    Debug.Log("done wallZ" + i);
                    oneLoop = true;
                }

            }




        }
        if(comboCounter == 1)
        {
            currentScore += 1000;
        }
        if (comboCounter == 2)
        {
            currentScore += 5000;
        }
        if (comboCounter == 3)
        {
            currentScore += 10000;
        }
        if (comboCounter == 4)
        {
            currentScore += 25000;
        }
        if (comboCounter == 5)
        {
            currentScore += 100000;
        }


        shouldSpawn = true;

    }




    //Used to see if the tempSet of block positions you pass in will cause you to lose 
    bool lossCheck(LinkedList<Vector3> tempSet)
    {
        bool currentBool = false;

        foreach (Vector3 element in tempSet)
        {
            //Debug.Log("x:" + element.x + "y:" + element.y + "z:" + element.z);
            if (FieldScript.boolField[(int)element.x, (int)element.y, (int)element.z])
            {
                currentBool = true;
            }
        }


        return currentBool;
    }




    //Irrelevant functions used to get rid of outline because I am bad at scripting ;( 
    public static GameObject getStupidSphere()
    {
        var tempObject = (GameObject)Instantiate(Resources.Load("sphere"));
        return tempObject;

    }

    public static void destroyStupidSphere(GameObject stupidSphere)
    {
        Destroy(stupidSphere);
    }


}
