using UnityEngine;
using System.Collections;

public class PathData
{
    private System.Collections.Generic.Queue<Transform> primPath;
    public System.Collections.Generic.Queue<Transform> PrimPath
    {
        get
        {
            return primPath;
        }
    }
    private System.Collections.Generic.Queue<Transform> darkPrimPath;
    public System.Collections.Generic.Queue<Transform> DarkPrimPath
    {
        get
        {
            return darkPrimPath;
        }
    }

    #region Singleton Implementation
    private static PathData instance = null;
    private static object lockObject = new object();
    private PathData()
    {
        primPath = new System.Collections.Generic.Queue<Transform>();
        darkPrimPath = new System.Collections.Generic.Queue<Transform>();
    }
    public static PathData Instance
    {
        get
        {
            lock(lockObject)
            {
                if(instance == null)
                {
                    instance = new PathData();
                }

                return instance;
            }
        }
    }
    #endregion

}
