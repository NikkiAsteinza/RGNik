//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using UnityEngine;

//public class RGNeighborhood : MonoBehaviour
//{
//   // public enum Neighborhoods { UpLeft, Up, UpRight, Right, DownRight, Down, DownLeft, Left }
//    private static RGNeighborhood _instance;
//    public static RGNeighborhood instance { get { return _instance; } }
//    //To do singleton
//    Vector2[] neiborhood;
//    Dictionary<Neighborhoods, Vector2> NeighborhoodRerefence;

//    private List<Vector2> GetMooreNeighborhood(int x, int y) 
//    {
//        //arriba
//        neiborhood[0] = new Vector2(0,1);
//        NeighborhoodRerefence.Add(Neighborhoods.Up, neiborhood[0]);
//        // arriba derecha
//        neiborhood[1] = new Vector2(1,1);
//        NeighborhoodRerefence.Add(Neighborhoods.UpRight, neiborhood[1]);
//        // arriba izquierda
//        neiborhood[2] = new Vector2(-1,1);
//        NeighborhoodRerefence.Add(Neighborhoods.UpLeft, neiborhood[2]);
//        // derecha
//        neiborhood[3] = new Vector2(1,0);
//        NeighborhoodRerefence.Add(Neighborhoods.Right, neiborhood[3]);
//        //abajo derecha
//        neiborhood[4] = new Vector2(1,-1);
//        NeighborhoodRerefence.Add(Neighborhoods.DownRight, neiborhood[4]);
//        // abajo izquierda
//        neiborhood[5] = new Vector2(-1,-1);
//        NeighborhoodRerefence.Add(Neighborhoods.DownLeft, neiborhood[5]);
//        // abajo
//        neiborhood[6] = new Vector2(0,-1);
//        NeighborhoodRerefence.Add(Neighborhoods.Down, neiborhood[6]);
//        // izquierda
//        neiborhood[7] = new Vector2(0,-1);
//        NeighborhoodRerefence.Add(Neighborhoods.Up, neiborhood[7]);

//        return neiborhood.ToList();
//    }
//    public int GetSameValueNeighborhoodCount(List<Vector2> neighborhoodList, int cellValue) 
//    {
//        foreach (Tuple<Neighborhoods,Vector2> tuple in NeighborhoodRerefence ) 
//        {
            
//        }
//        return 0;
//    }
    
//    public void Awake()
//    {
//        if (_instance == null)
//        {
//            _instance = this;
//            DontDestroyOnLoad(this.gameObject);
//        }
//        else
//            Destroy(this);
//    }
//}
