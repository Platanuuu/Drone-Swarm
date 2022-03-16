using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//设置单例模式，不太懂原理，运用到AStarMgr上
public class BaseManager <T>where T:new()
{
       private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
            instance = new T();
        return instance;
    }
}
public class GameManager
{
 
    
}