using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceObject {


    
    private LinkedList<GameObject> actualObjects;
    
    private string pieceName;

    public LinkedList<GameObject> reflectionObjects;

    
	



    //constructor
    public PieceObject(LinkedList<GameObject> tempObject, string tempName)
    {
        
        actualObjects = tempObject;
        pieceName = tempName;
    }

    //Returns the list of gameobjects attached to the piece
    public LinkedList<GameObject> getActualObjects()
    {
        return this.actualObjects;
    }

    //Returns piece type name
    public string getPieceName()
    {
        return this.pieceName;
    }

    public bool isFullyPartOfWallX(int xVal, int sizeDim, int yOffset)
    {
        

        foreach(GameObject element in actualObjects)
        {
            if(element.transform.position.x != xVal || (element.transform.position.y > sizeDim + yOffset - 4))
            {
                return false;
            }
        }

        return true;

        
    }

    public bool isFullyPartOfWallZ(int zVal, int sizeDim, int yOffset)
    {


        foreach (GameObject element in actualObjects)
        {
            if (element.transform.position.z != zVal || (element.transform.position.y > sizeDim + yOffset - 4))
            {
                return false;
            }
        }

        return true;


    }

    public bool isPartiallyPartOfWallZ(int zVal, int sizeDim, int yOffset)
    {


        foreach (GameObject element in actualObjects)
        {
            if (element.transform.position.z == zVal)
            {
                return true;
            }
        }

        return false;


    }


    public bool isPartiallyPartOfWallX(int xVal, int sizeDim, int yOffset)
    {


        foreach (GameObject element in actualObjects)
        {
            if (element.transform.position.x == xVal)
            {
                return true;
            }
        }

        return false;


    }

    //Checks to see if piece intersects floor at all
    public bool isTouchingFloor(int floorVal)
    {
        bool tempBool = false; 

        foreach(GameObject element in actualObjects)
        {
            if(element.transform.position.y == floorVal)
            {
                tempBool = true;
            }
        }
        return tempBool;
    }


    //Takes in a floor value and checks to see if the piece is entirely above it 
    public bool isAboveFloorVal(int floorVal)
    {
        foreach (GameObject element in actualObjects)
        {
            if (element.transform.position.y < floorVal)
            {
                return false;
            }
        }
        return true;
    }

    //Removing gameObject from list of gameobjects assosciated with piece; Used for remove floors 
    public void removeObjectFromActualObjects(GameObject tempObject)
    {
        actualObjects.Remove(tempObject);
    }


    public void rotateZAxis(int direction)
    {
        var anchorTransform = actualObjects.First.Value.transform;
        //Debug.Log("anchor: " + anchorTransform.position);
        //printCurrentPositions();

        foreach (var element in actualObjects)
        {
            if (element.transform != anchorTransform)
            {
                if (element.transform.position.x == 0)
                {
                    //Debug.Log("x if");
                    anchorTransform.position = new Vector3( anchorTransform.position.x + 1,  anchorTransform.position.y,  anchorTransform.position.z);
                    element.transform.position = new Vector3( element.transform.position.x + 1,  element.transform.position.y,  element.transform.position.z);
                    element.transform.RotateAround(anchorTransform.position, Vector3.forward, direction * -90f);
                    anchorTransform.position = new Vector3( anchorTransform.position.x - 1,  anchorTransform.position.y,  anchorTransform.position.z);
                    element.transform.position = new Vector3( element.transform.position.x - 1,  element.transform.position.y,  element.transform.position.z);
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);

                }
                else if (element.transform.position.z == 0)
                {
                    //Debug.Log("z if");
                    anchorTransform.position = new Vector3( anchorTransform.position.x,  anchorTransform.position.y,  anchorTransform.position.z + 1);
                    element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z + 1);
                    element.transform.RotateAround(anchorTransform.position, Vector3.forward, direction * -90f);
                    anchorTransform.position = new Vector3(anchorTransform.position.x, anchorTransform.position.y, anchorTransform.position.z - 1);
                    element.transform.position = new Vector3(element.transform.position.x, element.transform.position.y, element.transform.position.z - 1);
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);
                }

                else if (element.transform.position.y == 0)
                {
                    //Debug.Log("y if");
                    anchorTransform.position = new Vector3( anchorTransform.position.x,  anchorTransform.position.y + 1,  anchorTransform.position.z);
                    element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y + 1,  element.transform.position.z);
                    element.transform.RotateAround(anchorTransform.position, Vector3.forward, direction * -90f);
                    anchorTransform.position = new Vector3( anchorTransform.position.x,  anchorTransform.position.y - 1,  anchorTransform.position.z);
                    element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y - 1,  element.transform.position.z);
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);
                }
                else
                {
                    //Debug.Log("no if");
                    //Debug.Log("anchor: " + anchorTransform.position);
                    //Debug.Log("before: " + element.transform.position);
                    element.transform.RotateAround(anchorTransform.position, Vector3.forward, direction * -90f);
                    //Debug.Log("afterP: " + element.transform.position);
                    
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);
                    //Debug.Log("afterR: " + element.transform.position);
                    //Debug.Log(element.transform.position + "after");
                }

                element.transform.position = new Vector3(Mathf.RoundToInt( element.transform.position.x), Mathf.RoundToInt(element.transform.position.y), Mathf.RoundToInt(element.transform.position.z));
                anchorTransform.transform.position = new Vector3(Mathf.RoundToInt(anchorTransform.transform.position.x), Mathf.RoundToInt(anchorTransform.transform.position.y), Mathf.RoundToInt(anchorTransform.transform.position.z));

                //printCurrentPositions();

            }
        }
    }



    public void rotateYAxis(int direction)
    {
        var anchorTransform = actualObjects.First.Value.transform;

        foreach (var element in actualObjects)
        {
            if (element.transform != anchorTransform)
            {
                if (element.transform.position.x == 0)
                {
                    anchorTransform.position = new Vector3(anchorTransform.position.x + 1, anchorTransform.position.y, anchorTransform.position.z);
                    element.transform.position = new Vector3(element.transform.position.x + 1, element.transform.position.y, element.transform.position.z);
                    element.transform.RotateAround(anchorTransform.position, Vector3.up, direction * 90f);
                    anchorTransform.position = new Vector3(anchorTransform.position.x - 1, anchorTransform.position.y, anchorTransform.position.z);
                    element.transform.position = new Vector3(element.transform.position.x - 1,  element.transform.position.y,  element.transform.position.z);
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);

                }
                else if (element.transform.position.z == 0)
                {
                    anchorTransform.position = new Vector3( anchorTransform.position.x ,  anchorTransform.position.y,  anchorTransform.position.z + 1);
                    element.transform.position = new Vector3( element.transform.position.x ,  element.transform.position.y,  element.transform.position.z + 1);
                    element.transform.RotateAround(anchorTransform.position, Vector3.up, direction * 90f);
                    anchorTransform.position = new Vector3( anchorTransform.position.x ,  anchorTransform.position.y,  anchorTransform.position.z-1);
                    element.transform.position = new Vector3( element.transform.position.x ,  element.transform.position.y,  element.transform.position.z -1);
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);
                }
                else if (element.transform.position.y == 0)
                {
                    anchorTransform.position = new Vector3( anchorTransform.position.x,  anchorTransform.position.y +1,  anchorTransform.position.z );
                    element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y +1,  element.transform.position.z );
                    element.transform.RotateAround(anchorTransform.position, Vector3.up, direction * 90f);
                    anchorTransform.position = new Vector3( anchorTransform.position.x,  anchorTransform.position.y-1,  anchorTransform.position.z );
                    element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y-1,  element.transform.position.z );
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);
                }
                else
                {
                    element.transform.RotateAround(anchorTransform.position, Vector3.up, direction * 90f);
                   
                    //Debug.Log(element.transform.position + "after");
                }
                element.transform.position = new Vector3(Mathf.RoundToInt(element.transform.position.x), Mathf.RoundToInt(element.transform.position.y), Mathf.RoundToInt(element.transform.position.z));
                anchorTransform.transform.position = new Vector3(Mathf.RoundToInt(anchorTransform.transform.position.x), Mathf.RoundToInt(anchorTransform.transform.position.y), Mathf.RoundToInt(anchorTransform.transform.position.z));
            }
        }
    }

    public void rotateXAxis(int direction)
    {
        var anchorTransform = actualObjects.First.Value.transform;

        foreach (var element in actualObjects)
        {
            if (element.transform != anchorTransform)
            { 
                if (element.transform.position.x == 0)
                {
                    anchorTransform.position = new Vector3( anchorTransform.position.x + 1,  anchorTransform.position.y,  anchorTransform.position.z);
                    element.transform.position = new Vector3( element.transform.position.x + 1,  element.transform.position.y,  element.transform.position.z);
                    element.transform.RotateAround(anchorTransform.position, Vector3.right, direction * 90f);
                    anchorTransform.position = new Vector3( anchorTransform.position.x - 1,  anchorTransform.position.y,  anchorTransform.position.z);
                    element.transform.position = new Vector3( element.transform.position.x - 1,  element.transform.position.y,  element.transform.position.z);
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);
                }
                else if (element.transform.position.z == 0)
                {
                    anchorTransform.position = new Vector3( anchorTransform.position.x,  anchorTransform.position.y,  anchorTransform.position.z + 1);
                    element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z + 1);
                    element.transform.RotateAround(anchorTransform.position, Vector3.right, direction * 90f);
                    anchorTransform.position = new Vector3( anchorTransform.position.x,  anchorTransform.position.y,  anchorTransform.position.z - 1);
                    element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z - 1);
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);
                }
                else if (element.transform.position.y == 0)
                {
                    anchorTransform.position = new Vector3( anchorTransform.position.x,  anchorTransform.position.y + 1,  anchorTransform.position.z);
                    element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y + 1,  element.transform.position.z);
                    element.transform.RotateAround(anchorTransform.position, Vector3.right, direction * 90f);
                    anchorTransform.position = new Vector3( anchorTransform.position.x,  anchorTransform.position.y - 1,  anchorTransform.position.z);
                    element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y - 1,  element.transform.position.z);
                    //element.transform.position = new Vector3( element.transform.position.x,  element.transform.position.y,  element.transform.position.z);
                }
                else
                {
                    element.transform.RotateAround(anchorTransform.position, Vector3.right, direction * 90f);
                   
                    //Debug.Log(element.transform.position + "after");
                }
                element.transform.position = new Vector3(Mathf.RoundToInt(element.transform.position.x), Mathf.RoundToInt(element.transform.position.y), Mathf.RoundToInt(element.transform.position.z));
                anchorTransform.transform.position = new Vector3(Mathf.RoundToInt(anchorTransform.transform.position.x), Mathf.RoundToInt(anchorTransform.transform.position.y), Mathf.RoundToInt(anchorTransform.transform.position.z));
            }
        }
    }







    public bool possibleRotateYAxis(int direction)
    {
        bool currentBool = true;
        var anchorTransform = actualObjects.First.Value.transform;

        var newSet = new LinkedList<Vector3>();
        newSet.AddLast(anchorTransform.position);

        var tempObject = FieldScript.getStupidSphere();
        var newTransform = tempObject.transform;
        
        foreach (var element in actualObjects)
        {
            tempObject.transform.position = element.transform.position;
            if (newTransform.position != anchorTransform.position)
            {
                newTransform.RotateAround(anchorTransform.position, Vector3.up, direction * 90f);
                newSet.AddLast(newTransform.position);
            }

        }
        //setting current position to false
        foreach (var element in actualObjects)
        {
            
            FieldScript.boolField[(int)element.transform.position.x, (int)element.transform.position.y, (int)element.transform.position.z] = false;
        }
        //checking if overlaps 
        foreach (var element in newSet)
        {

            if( (element.x < 0 || element.x >= FieldScript.sizeDim) || (element.y <= 0 || element.y >= FieldScript.sizeDim + FieldScript.yOffset) || (element.z <= 0 || element.z >= FieldScript.sizeDim))
            {
                currentBool = false;
            }
            else if(FieldScript.boolField[(int)element.x, (int)element.y, (int)element.z])
            {
                currentBool = false;
                
            }
        }
        //setting back to normal 
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int)element.transform.position.x, (int)element.transform.position.y, (int)element.transform.position.z] = true;
        }

        FieldScript.destroyStupidSphere(tempObject);
        return currentBool;
    }


  



    public bool possibleRotateXAxis(int direction)
    {
        bool currentBool = true;
        var anchorTransform = actualObjects.First.Value.transform;

        var newSet = new LinkedList<Vector3>();
        newSet.AddLast(anchorTransform.position);


        var tempObject = FieldScript.getStupidSphere();
        var newTransform = tempObject.transform;

        foreach (var element in actualObjects)
        {
            tempObject.transform.position = element.transform.position;
            if (newTransform.position != anchorTransform.position)
            {
                newTransform.RotateAround(anchorTransform.position, Vector3.right, direction * 90f);
                newSet.AddLast(newTransform.position);
            }

        }
        //setting current position to false
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int)element.transform.position.x, (int)element.transform.position.y, (int)element.transform.position.z] = false;
        }
        //checking if overlaps 
        foreach (var element in newSet)
        {
            if ((element.x < 0 || element.x >= FieldScript.sizeDim) || (element.y <= 0 || element.y >= FieldScript.sizeDim + FieldScript.yOffset) || (element.z < 0 || element.z >= FieldScript.sizeDim))
            {
                currentBool = false;
            }
            else if (FieldScript.boolField[(int)element.x, (int)element.y, (int)element.z])
            {
                currentBool = false;   
            }


        }
        //setting back to normal 
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int)element.transform.position.x, (int)element.transform.position.y, (int)element.transform.position.z] = true;
        }
        FieldScript.destroyStupidSphere(tempObject);
        return currentBool;
    }


    
    public bool possibleRotateZAxis(int direction)
    {
        bool currentBool = true;
        var anchorTransform = actualObjects.First.Value.transform;

        var newSet = new LinkedList<Vector3>();
        newSet.AddLast(anchorTransform.position);


        var tempObject = FieldScript.getStupidSphere();
        var newTransform = tempObject.transform;

        foreach (var element in actualObjects)
        {
            tempObject.transform.position = element.transform.position;
            if (newTransform.position != anchorTransform.position)
            {
                newTransform.RotateAround(anchorTransform.position, Vector3.forward, direction * -90f);
                newSet.AddLast(newTransform.position);
            }
        }
        //setting current position to false
        foreach (var element in actualObjects)
        { 
            FieldScript.boolField[(int)element.transform.position.x, (int)element.transform.position.y, (int)element.transform.position.z] = false;
        }
        //checking if overlaps 
        foreach (var element in newSet)
        {
            if ((element.x < 0 || element.x >= FieldScript.sizeDim) || (element.y <= 0 || element.y >= FieldScript.sizeDim + FieldScript.yOffset) || (element.z < 0 || element.z >= FieldScript.sizeDim))
            {
                currentBool = false;
            }
            else if (FieldScript.boolField[(int)element.x, (int)element.y, (int)element.z])
            {
                currentBool = false;
            }
        }
        //setting back to normal 
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int)element.transform.position.x, (int)element.transform.position.y, (int)element.transform.position.z] = true;
        }
        //Debug.Log("finshed checker" + currentBool);
        FieldScript.destroyStupidSphere(tempObject);
        return currentBool;
    }



    



    public void moveDownOnePosition()
    {
       

        foreach (GameObject element in actualObjects)
        {
            element.transform.Translate(new Vector3(0, -1, 0), Space.World);

        }
    }








    public void moveLeftOnePosition()
    {
        

        foreach (GameObject element in actualObjects)
        {
            element.transform.Translate(new Vector3(-1, 0, 0), Space.World);

        }
    }







    public void moveRightOnePosition()
    {
        //printCurrentPositions();

        foreach (GameObject element in actualObjects)
        {
            element.transform.Translate(new Vector3(1, 0, 0), Space.World);

        }
    }








    public void moveForwardOnePosition()
    {
        

        foreach (GameObject element in actualObjects)
        {
            element.transform.Translate(new Vector3(0, 0, 1), Space.World);

        }
    }







    public void moveBackwardOnePosition()
    {
        

        foreach (GameObject element in actualObjects)
        {
            
            element.transform.Translate(new Vector3(0, 0, -1), Space.World);
            

        }
    }


    public void moveReflectionDownward()
    {
        foreach (GameObject element in reflectionObjects)
        {
            element.transform.Translate(new Vector3(0, -1, 0), Space.World);

        }
    }

    public bool possibleReflectionDownward()
    {
        var currentBool = true;

        foreach (var element in reflectionObjects)
        {
            if (element.transform.position.y <= 0)
            {
                return false;
            }

        }

        //setting current  actual position to false
        foreach (var element in actualObjects)
        {

            FieldScript.boolField[(int)Mathf.Round(element.transform.position.x), (int)Mathf.Round(element.transform.position.y), (int)Mathf.Round(element.transform.position.z)] = false;
        }


        foreach (var element in reflectionObjects)
        {
            
            if (FieldScript.boolField[(int)Mathf.Round(element.transform.position.x), (int)Mathf.Round(element.transform.position.y) - 1, (int)Mathf.Round(element.transform.position.z)] )
            {
                currentBool = false; 
            }
        }

        //setting back to normal 
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int)Mathf.Round(element.transform.position.x), (int)Mathf.Round(element.transform.position.y), (int)Mathf.Round(element.transform.position.z)] = true;
        }

        return currentBool;
    }




    public bool possibleDownwardMovement()
    {
        bool currentBool = true;

        //checking if out of range
        foreach (var element in actualObjects)
        {
            if ( element.transform.position.y <= 0 )
            {
                currentBool = false;
                return currentBool;
            }
            
        }

        //setting current position to false
        foreach (var element in actualObjects)
        {
            
            FieldScript.boolField[(int) Mathf.Round(element.transform.position.x),(int) Mathf.Round(element.transform.position.y),(int) Mathf.Round(element.transform.position.z)] = false;
        }

        //checking if overlaps downwards
        foreach (var element in actualObjects)
        {
            if (FieldScript.boolField[(int) Mathf.Round(element.transform.position.x), (int) Mathf.Round(element.transform.position.y) - 1, (int) Mathf.Round(element.transform.position.z)])
            {
                currentBool = false;
            }
        }

        //setting back to normal 
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int) Mathf.Round(element.transform.position.x), (int) Mathf.Round(element.transform.position.y), (int) Mathf.Round(element.transform.position.z)] = true;
        }

        return currentBool;


    }











    public bool possibleForwardMovement()
    {
        bool currentBool = true;

        //checking if out of range
        foreach (var element in actualObjects)
        {
            if ( element.transform.position.z >= FieldScript.sizeDim -1)
            {
                currentBool = false;
                return currentBool;
            }

        }

        //setting current position to false
        foreach (var element in actualObjects)
        {

            FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z] = false;
        }

        //checking if overlaps downwards
        foreach (var element in actualObjects)
        {
            if (FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z +1])
            {
                currentBool = false;
            }
        }

        //setting back to normal 
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z] = true;
        }

        return currentBool;


    }












    public bool possibleBackwardMovement()
    {
        bool currentBool = true;

        //checking if out of range
        foreach (var element in actualObjects)
        {
            if ( element.transform.position.z <= 0 )
            {
                currentBool = false;
                return currentBool;
            }

        }

        //setting current position to false
        foreach (var element in actualObjects)
        {

            FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z] = false;
        }

        //checking if overlaps downwards
        foreach (var element in actualObjects)
        {
            if (FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z - 1])
            {
                currentBool = false;
            }
        }

        //setting back to normal 
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z] = true;
        }

        return currentBool;


    }





    //Debug Function
    public void printCurrentPositions()
    {
        Debug.Log("start");
        foreach(var element in actualObjects)
        {
            Debug.Log(element.transform.position);
        }
        Debug.Log("end");
    }







    public bool possibleLeftwardMovement()
    {
        //printCurrentPositions();
        bool currentBool = true;

        //checking if out of range
        foreach (var element in actualObjects)
        {
            if (element.transform.position.x <= 0 )
            {
                currentBool = false;
                return currentBool;
            }

        }

        //setting current position to false
        foreach (var element in actualObjects)
        {

            FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z] = false;
        }

        //checking if overlaps downwards
        foreach (var element in actualObjects)
        {
            //Debug.Log("element x test: " + ((int) element.transform.position.x - 1) + "element y test: " + (int) element.transform.position.y + "element z test: " + (int) element.transform.position.z);
            if (FieldScript.boolField[(int) element.transform.position.x - 1, (int) element.transform.position.y, (int) element.transform.position.z])
            {
                currentBool = false;
            }
        }

        //setting back to normal 
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z] = true;
        }

        return currentBool;


    }










    public bool possibleRightwardMovement()
    {
        bool currentBool = true;

        //checking if out of range
        foreach (var element in actualObjects)
        {
            if (element.transform.position.x >= FieldScript.sizeDim-1 )
            {
                currentBool = false;
                return currentBool;
            }

        }

        //setting current position to false
        foreach (var element in actualObjects)
        {

            FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z] = false;
        }

        //checking if overlaps downwards
        foreach (var element in actualObjects)
        {
            if (FieldScript.boolField[(int) element.transform.position.x + 1, (int) element.transform.position.y, (int) element.transform.position.z])
            {
                currentBool = false;
            }
        }

        //setting back to normal 
        foreach (var element in actualObjects)
        {
            FieldScript.boolField[(int) element.transform.position.x, (int) element.transform.position.y, (int) element.transform.position.z] = true;
        }

        return currentBool;


    }





}
