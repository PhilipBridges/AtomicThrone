using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Nav : MonoBehaviour
{
    static NavMeshSurface2d nav;
    // Start is called before the first frame update
    void Start()
    {
       nav = GetComponent<NavMeshSurface2d>();
        //nav.BuildNavMesh(); 
    }

    public static IEnumerator Generate()
    {
        nav.BuildNavMesh();
        yield return new WaitForSeconds(.3f);
    }

    public static void LoadNav()
    {
        nav.BuildNavMesh();
    }

}
