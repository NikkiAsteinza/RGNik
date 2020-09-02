using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class NetworkDisplay : MonoBehaviour
{
    [SerializeField] GameObject DisplayCube;
    float x=1;
    float y=1;
    float height=0.3f;
    
    [HideInInspector]
    private RGNetkworkAutomata ParentNetwork;
    public List<GameObject> TwoDPlanes = new List<GameObject>();

    public int columns;
    public int rows;
    private void Awake()
    {
        ParentNetwork = GetComponentInParent<RGNetkworkAutomata>();
    }

    public void Init(float realWidth,float realDepth)
    {
        this.transform.localScale = new Vector3(x,height,y);

        columns = (int)realWidth ;
        rows = (int)realDepth ;

        

        float halfWidth =  realWidth / 2;
        print("Half  real width:" + halfWidth);
        float halfDepth = realDepth / 2;

       

        float widthRest = realWidth % 2;
        print("Wisth rest:" + widthRest);

        float depthRest = realDepth % 2;
        print("Depth rest" + depthRest);

        float planeWidth = x / realWidth;

        float planeDepth =y / realDepth;
        print("Plane dimensions:" + planeWidth + "|" + planeDepth);


        print("instanciado cubitos");
        for (float i = -halfWidth; i < halfWidth; i++)
        {
            for (float j = -halfDepth; j <halfDepth; j++)
            {
                float posX = i+(halfWidth*planeWidth) ;

                float posY = j+(halfDepth*planeDepth);


                GameObject displayPanel = Instantiate(DisplayCube);
                displayPanel.transform.SetParent(this.transform);
                displayPanel.AddComponent<RGNikObject>();
                displayPanel.AddComponent<RGDisplayCell>();
                Vector3 planePosition = CalculatePlanePosition(posX, posY, height);
       
                Vector3 planeScale = new Vector3(planeWidth, 0.1f,planeDepth );
                displayPanel.GetComponent<RGNikObject>().ScaleAndTranslate(planePosition, planeScale);

                TwoDPlanes.Add(displayPanel);
            }
          
        }
        print("Numero de cubos instanciados:" + TwoDPlanes.Count);
    }
    public Vector3 CalculatePlanePosition(float x, float y, float height)
    {


        return new Vector3(x, height, y);
    }
}
