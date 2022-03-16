using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum E_Node_Type
{
    //可以走
    Walk,
    //不可以
    Stop,
}
//AStar格子结点
public class AStarNode 
{
    //节点坐标
    public int x;
    public int y;

   //寻路消耗
   public float f;
   //距离起点的距离
   public float g;
   //距离终点的估算距离
   public float h;
   //父节点
   public AStarNode father;
   //结点是否能走
   public E_Node_Type type;

//构造函数传入坐标和类型
    public AStarNode( int x, int y ,E_Node_Type type)
    {
        this.x=x;
        this.y=y;
        this.type=type;
    }

}
