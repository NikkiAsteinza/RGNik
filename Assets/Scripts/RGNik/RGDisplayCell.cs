using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RGDisplayCell : RGNikObject
{
    [SerializeField] GameObject displayPanel;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] TextMeshProUGUI typeText;
    public enum Neighborhoods { UpLeft, Up, UpRight, Right, DownRight, Down, DownLeft, Left }
    public enum CellType { Middle, Corner, Lateral}

    [SerializeField]
    private RGNetkworkAutomata parentNetwork;
    [SerializeField]
    private NetworkDisplay parent;
    public CellType cellType;



    new protected  float movementDuration = 60;
    public bool isInitialized;
    public int Value;
    public int SameNeighborhoods;
    public int GridPositionY;
    public int GridPositionX;
    public bool isOnXLeftLimit;
    public bool isOnXRightLimit;
    public bool isOnYUpLimit;
    public bool isOnYDownLimit;
    public bool isOnCorner;
    public int totalNeighborhood;
    public void Awake()
    {
        parent = GetComponentInParent<NetworkDisplay>();

        parentNetwork = parent.gameObject.GetComponentInParent<RGNetkworkAutomata>();
        this.transform.gameObject.tag = "DisplayCell";

        displayPanel = this.transform.GetChild(0).gameObject;
        typeText = displayPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        valueText = displayPanel.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

    }
    private void Start()
    {

    }
    public void InitCell(int x , int y)
    {
        //if (RGNik.instance.DebugCells == true) 
        //{
        //    displayPanel.SetActive(true);
        //}
        GridPositionX = x;
        GridPositionY = y;

        GetCellType(GridPositionX, GridPositionY);
        SetNeighborsToCheck();
        GetMooreNeighborhood();
        GetSameNeighbohoods();
        //parentNetwork.grid[GridPositionX, GridPositionY] = Value;
        UpdateValueText();
        isInitialized = true;
    }

    public void UpdateValueText()
    {
        valueText.text = Value.ToString();
    }

    public void UpdateCell(int value) 
    {
        //int sameNeighborhood = GetSameValueNeighborhoodCount();

        //if (sameNeighborhood > (totalNeighborhood / 2)) 
        //{
        //    parentNetwork.grid[GridPositionX, GridPositionY] = Value == 0 ? 0 : 1;
        //}
        Value = value;
        UpdateValueText();
    }



    Dictionary<Neighborhoods, Vector2> NeighborhoodRerefence = new Dictionary<Neighborhoods, Vector2>();
    private int GetSameNeighbohoods() 
    {
        int count = 0;
        foreach (KeyValuePair<Neighborhoods, Vector2> key in NeighborhoodRerefence) 
        {
            try
            {
                var equal = parentNetwork.grid[(int)key.Value.x, (int)key.Value.y] == parentNetwork.grid[GridPositionX,GridPositionY];
                count = equal? count+1:count;
            }
            catch (IndexOutOfRangeException) 
            {
            
            }
        }
        SameNeighborhoods = count;
        return SameNeighborhoods;
    }
    private void GetMooreNeighborhood()
    {
        //arriba
        NeighborhoodRerefence.Add(Neighborhoods.Up, new Vector2(GridPositionX, GridPositionY + 1));
        // arriba derecha
        NeighborhoodRerefence.Add(Neighborhoods.UpRight, new Vector2(GridPositionX + 1, GridPositionY + 1));
        // arriba izquierda
        NeighborhoodRerefence.Add(Neighborhoods.UpLeft, new Vector2(GridPositionX - 1, GridPositionY + 1));
        // derecha
        NeighborhoodRerefence.Add(Neighborhoods.Right, new Vector2(GridPositionX + 1, GridPositionY));
        //abajo derecha
        NeighborhoodRerefence.Add(Neighborhoods.DownRight, new Vector2(GridPositionX + 1, GridPositionY - 1));
        // abajo izquierda
        NeighborhoodRerefence.Add(Neighborhoods.DownLeft, new Vector2(GridPositionX - 1, GridPositionY - 1));
        // abajo
        NeighborhoodRerefence.Add(Neighborhoods.Down, new Vector2(GridPositionX + 0, GridPositionY - 1));
        // izquierda
        NeighborhoodRerefence.Add(Neighborhoods.Left, new Vector2(GridPositionX + 0, GridPositionY - 1));
    }

    private void GetCellType(int x, int y) 
    {
        string text = "";
        isOnXLeftLimit = x == 0 ;
        isOnXRightLimit = x == parent.columns - 1;
        isOnYUpLimit = y == 0 ;
        isOnYDownLimit = y == (parent.rows-1);

        isOnCorner = (x == (parent.columns - 1) && y == (parent.rows - 1)) || (x == 0 && y == 0) || (x == (parent.columns - 1) && y == 0) || (x == 0 && y == (parent.rows - 1));

        if (isOnXLeftLimit || isOnYUpLimit || isOnXRightLimit || isOnYDownLimit)
        {
            if (isOnCorner) 
            {
                cellType = CellType.Corner;
                text = "Corner";
            }
            else 
            {
                cellType = CellType.Lateral;
                text = "Lateral";
            }
        }
        else 
        {
            cellType = CellType.Middle;
            text = "Middle";
        }


        typeText.text = text;
    }

    public void SetNeighborsToCheck()
    {
           switch (cellType) 
            {
                case CellType.Corner:
                    totalNeighborhood = 3;
                    break;
                case CellType.Lateral:
                    totalNeighborhood = 5;
                    break;
                case CellType.Middle:
                    totalNeighborhood = 8;
                    break;
            }
        }
    
    //
    public void OnClick() 
    {
        this.transform.GetComponent<Renderer>().material = parentNetwork.selectedMaterial;
    }
}
