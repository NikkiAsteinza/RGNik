using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static RGDisplayCell;

public class RGNetkworkAutomata : RGNikObject
{
    [SerializeField] RGNikEnums.WallType type;
    [SerializeField] NetworkDisplay networkDisplay;
    [SerializeField] Material walkableMaterial;
    [SerializeField] Material notWalkableMaterial;
    public Material selectedMaterial;

    float pNotWalkableZone = 3;
    float pWalkableZone = 7;

    public int[,] grid;
    int[,] oldGrid;

    bool firstExecution;
    bool fillingProbGrid;
    bool executingAutomata;
    bool updatingDisplay;

    int iterationCounter = 0;
    int width;
    int height;

    bool stop = false;
    public NetworkDisplay Display { get { return networkDisplay; } }

    public void NetworkInit()
    {
        InitGrids();
        //PrintGrid();
        oldGrid = grid;
        PrintGrid(true);
        ExecuteAutomata();
        PrintGrid(true);
        UpdateNetworkDisplay();
        AutomataLoop();
    }

    private void FillProbabilisticGrid()
    {
        print("<b>[" + iterationCounter + "]Rellenando grid de probabilidades</b>");
        fillingProbGrid = true;
        for (int i = 0; i < networkDisplay.columns; i++)
        {
            for (int j = 0; j < networkDisplay.rows; j++)
            {
                float prob = Random.Range(0, pWalkableZone);
                if (prob >= pNotWalkableZone)
                    grid[i, j] = 0;

                else
                    grid[i, j] = 1;
            }
        }
        print("[" + iterationCounter + "]..finalizando la asignación de las probabilidades");
        fillingProbGrid = false;
    }
    private void PrintGrid(bool onlyNew = false)
    {
        if (!onlyNew)
        {
            print("<b>Old grid</b>");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < networkDisplay.columns; i++)
            {
                for (int j = 0; j < networkDisplay.rows; j++)
                {
                    sb.Append(oldGrid[i, j]);
                    sb.Append('\t');
                }
                sb.AppendLine();
            }

            Debug.Log(sb.ToString());
        }

        print("<b> grid</b>");
        StringBuilder sb2 = new StringBuilder();
        for (int i = 0; i < networkDisplay.columns; i++)
        {
            for (int j = 0; j < networkDisplay.rows; j++)
            {
                sb2.Append(grid[i, j]);
                sb2.Append('\t');
            }
            sb2.AppendLine();
        }
        Debug.Log(sb2.ToString());
    }
    private void InitGrids()
    {
        width = networkDisplay.columns - 1;
        height = networkDisplay.rows - 1;
        grid = new int[networkDisplay.columns, networkDisplay.rows];
        oldGrid = new int[networkDisplay.columns, networkDisplay.rows];
        print("<b>Elementos del grid:</b>" + networkDisplay.columns * networkDisplay.rows);
        FillProbabilisticGrid();
        PrintGrid();
        UpdateNetworkDisplay();

    }

    private void UpdateNetworkDisplay()
    {
        print("<b>[" + iterationCounter + "]Actualizando celdas del Network Display</b>");

        updatingDisplay = true;
        for (int i = 0; i < networkDisplay.columns; i++)
        {
            for (int j = 0; j < networkDisplay.rows; j++)
            {
                int walkable = grid[i, j];
                Material targetMaterial = walkable == 0 ? walkableMaterial : notWalkableMaterial;
                int index = Get1DArrayIndex(i, j);
                networkDisplay.TwoDPlanes.ToArray()[index].GetComponent<Renderer>().material = targetMaterial;

                RGDisplayCell cell = networkDisplay.TwoDPlanes.ToArray()[index].GetComponent<RGDisplayCell>();
                if (!cell.isInitialized) cell.InitCell(i, j);
                
            }
        }
        new WaitForSeconds(5);
        //print("<b>[" + iterationCounter + "] finalizando la actualización visual de las celdas</b>");
        //updatingDisplay = false;
    }

    private int Get1DArrayIndex(int i, int j)
    {
        return ((networkDisplay.columns) * j) + i;
    }

    private bool GridsAreEqual()
    {

        bool areEquals = true;
        for (int i = 0; i < networkDisplay.columns; i++)
        {
            for (int j = 0; j < networkDisplay.rows; j++)
            {
                if (grid[i, j] != oldGrid[i, j])
                    areEquals = false;
            }
        }
        print("<b>Grids are equal:</b>" + (oldGrid.Cast<string>() == grid.Cast<string>()));
        print("<b>Grids are equals:</b>" + areEquals);
        return areEquals;
    }
    public void AutomataLoop()
    {

        while (!GridsAreEqual())
        {

            iterationCounter++;
            oldGrid = grid;
            ExecuteAutomata();
            PrintGrid();
            UpdateNetworkDisplay();

        }
        for (int i = 0; i < networkDisplay.columns; i++)
        {
            for (int j = 0; j < networkDisplay.rows; j++)
            {

                int index = Get1DArrayIndex(i, j);

                RGDisplayCell cell = networkDisplay.TwoDPlanes.ToArray()[index].GetComponent<RGDisplayCell>();
                if (cell.cellType == CellType.Lateral || cell.cellType == CellType.Corner) 
                {
                    grid[i, j] = 0;
                    cell.UpdateCell(grid[i, j]);
                }
                
            }
        }
        UpdateNetworkDisplay();

    }

    public void ExecuteAutomata()
    {
        print("<b>[" + iterationCounter + "] Ejecutando el autómata celular sobre el grid</b>");
        executingAutomata = true;

        for (int i = 0; i < networkDisplay.columns; i++)
        {
            for (int j = 0; j < networkDisplay.rows; j++)
            {
                int index = Get1DArrayIndex(i, j);

                RGDisplayCell cell = networkDisplay.TwoDPlanes.ToArray()[index].GetComponent<RGDisplayCell>();
                
               
               

                if (grid[i, j] == 1)
                {
                    if (cell.SameNeighborhoods > (cell.totalNeighborhood / 2))
                    {
                        grid[i, j] = 1;
                    }
                    else
                        grid[i, j] = 0;
                    cell.UpdateCell(grid[i, j]);
                }
                else if (grid[i, j] == 0)
                {
                    if (cell.SameNeighborhoods > (cell.totalNeighborhood / 2))
                    {
                        grid[i, j] = 0;
                    }
                    else
                        grid[i, j] = 1;
                    cell.UpdateCell(grid[i, j]);
                }




             
            }

        }

        //Puede ser el updatenetworkdisplay una corutina?
        //print("<b[" + iterationCounter + "]...finalizando la ejecución del autómata</b>");
        //executingAutomata = false;
    }
}
