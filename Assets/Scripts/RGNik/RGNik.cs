using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class RGNik : MonoBehaviour
{
    private static MonoBehaviour _instance;
    public static MonoBehaviour instance { get { return _instance; } }

    [Header("Admin player reference")]
    [SerializeField] Admin admin;

    [Header("Input canvas references")]
    [SerializeField] TMPro.TMP_InputField width;
    [SerializeField] TMPro.TMP_InputField height;

    [Header("Six sides network automatas references")]
    [SerializeField] RGNetkworkAutomata floor;
    [SerializeField] RGNetkworkAutomata roof;
    [SerializeField] RGNetkworkAutomata leftSidewall;
    [SerializeField] RGNetkworkAutomata rightSideWall;
    [SerializeField] RGNetkworkAutomata frontSideWall;
    [SerializeField] RGNetkworkAutomata backSideWall;

    [Header("Room height")]
    [SerializeField] float floorHeight=0.3f;

  
    float roomHeight=3;
    int realWidth;
    int realDepth;

    public bool DebugCells;
    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        floor.gameObject.SetActive(true);
    }
    public void AdjustRoom() 
    {
        realWidth = int.Parse(width.text);
        realDepth = int.Parse(height.text);
        SetFloor();

        admin.Translate(new Vector3(admin.transform.position.x, admin.transform.position.y + floorHeight, admin.transform.position.z));


        SetSideWalls();
        new WaitForSeconds(5);
        SetFrontAndBackWalls();
        new WaitForSeconds(5);
        SetRoof();

        new WaitForSeconds(5);
        print("display");
        // floor.Display.SetDimensions(floorDimensions.x, floorDimensions.z, floorHeight * 2);
        floor.Display.Init(realWidth, realDepth);
        new WaitForSeconds(5);
        floor.NetworkInit();
    }
    public void SetFloor() 
    {
        Vector3 floorDimensions = new Vector3(realWidth, floorHeight, realDepth);
        floor.ScaleAndTranslate( Vector3.zero, floorDimensions);


    }
    public void SetRoof()
    {
        roof.gameObject.SetActive(true);
        Vector3 roofDimensions = new Vector3(realWidth, floorHeight, realDepth);
        roof.ScaleAndTranslate(new Vector3(0, roomHeight,0), roofDimensions);
    }
    private float GetSideWallsXPosition() 
    {
        float xPos;
        float halfWidth = (float)realWidth/2.00f;
        print("halfWidth:"+halfWidth);
        xPos = halfWidth + (floorHeight / 2);
        return xPos;
    }

    public void SetSideWalls()
    {

        Vector3 sideWallDimensions = SideWallDimension();
        float rotation = 90;
        LeftWallSettup(sideWallDimensions,rotation);
        
        
       RightWallSettup(sideWallDimensions, rotation);
        
    }

    private void RightWallSettup(Vector3 sideWallDimensions, float rotation)
    {
        rightSideWall.gameObject.SetActive(true);

        Vector3 rightWallPosition = new Vector3(GetSideWallsXPosition(), (roomHeight / 2f) + (floorHeight / 2), 0);

        Vector3 rightRotation = leftSidewall.transform.rotation.eulerAngles;

        rightRotation.y = rotation;
        rightSideWall.TranslateScaleRotate(rightWallPosition, sideWallDimensions, rightRotation);


    }

    private void LeftWallSettup(Vector3 sideWallDimensions, float rotation)
    {
        leftSidewall.gameObject.SetActive(true);

        Vector3 leftWallPosition = new Vector3(-GetSideWallsXPosition(), (roomHeight / 2f) + (floorHeight / 2), 0);
        Vector3 leftRotation = leftSidewall.transform.rotation.eulerAngles;
        
        leftRotation.y = rotation;
        leftSidewall.TranslateScaleRotate(leftWallPosition, sideWallDimensions, leftRotation);
    }

    private Vector3 SideWallDimension()
    {
        return new Vector3(realDepth, roomHeight, floorHeight);
    }
    private float GetFrontWallsZPosition()
    {
        float zPos;
        float halfDepth = (float)realDepth / 2.00f;
        //print("halfDepth:" + halfDepth);
        zPos = halfDepth + (floorHeight / 2);
        return zPos;
    }
    public void SetFrontAndBackWalls()
    {
        frontSideWall.gameObject.SetActive(true);

        Vector3 frontBackWallsDimensions = GetFrontWallsDimensions();

        Vector3 frontWallPosition = new Vector3(0, (roomHeight / 2f) + (floorHeight / 2), GetFrontWallsZPosition());
        frontSideWall.ScaleAndTranslate(frontWallPosition, frontBackWallsDimensions);

        backSideWall.gameObject.SetActive(true);
        Vector3 backWallPosition = new Vector3(0, (roomHeight / 2f) + (floorHeight / 2), -GetFrontWallsZPosition());
        backSideWall.ScaleAndTranslate(backWallPosition, frontBackWallsDimensions);
    }

    private Vector3 GetFrontWallsDimensions()
    {
        return new Vector3((realWidth), roomHeight, floorHeight);
    }
}
