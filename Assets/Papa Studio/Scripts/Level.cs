using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Level : MonoBehaviour
{
    
    Transform StartLine;
    public int LevelNumber;
    [Header("Road")]
    public GameObject _StartBlock;
    public GameObject Block1;
    public GameObject Block2;
    public GameObject Block3;
    public GameObject Block4;
    public GameObject Block5;

    public GameObject EndBlock;
    GameObject Empty;
#if UNITY_EDITOR
    // Update is called once per frame
    public void StartBlock()
    {

        GameObject Track = PrefabUtility.InstantiatePrefab(_StartBlock) as GameObject;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);


    }

    public void block1()
    {


        GameObject Track = PrefabUtility.InstantiatePrefab(Block1) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }

    public void block2()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block2) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }


    public void block3()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(Block3) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }


    public void block4()
    {

        GameObject Track = PrefabUtility.InstantiatePrefab(Block4) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }

    public void block5()
    {

        GameObject Track = PrefabUtility.InstantiatePrefab(Block5) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }





    public void endblock()
    {
        GameObject Track = PrefabUtility.InstantiatePrefab(EndBlock) as GameObject;
        Track.transform.position = StartLine.transform.position;
        StartLine = Track.transform.GetChild(0).transform;
        Track.transform.SetParent(Empty.transform);
    }
#endif
    public void CreateLevelEmpty()
    {
        Empty = new GameObject("Level_" + LevelNumber);

    }
}
