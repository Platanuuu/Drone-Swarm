using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Astar寻路管理器
public class AStarMgr : BaseManager<AStarMgr>
{
    //设置单例模式

    //地图的宽高
    private int mapW;
    private int mapH;

    private int mapZ;

    //地图格子容器
    public AStarNode[,,] nodes;
    //开启列表
    private List<AStarNode> openlist = new List<AStarNode>();//建立开启列表并初始化
    private List<AStarNode> closelist = new List<AStarNode>();

    //初始化地图信息
    public void InitMapInfo(int w, int h, int z)
    {
        //记录宽高
        this.mapW = w;
        this.mapH = h;
        this.mapZ = z;
        //分配内存
        nodes = new AStarNode[w, h, z];
        //随机创建不可走地方
        for (int i = 0; i < w; ++i)
        {
            for (int k = 0; k < z; ++k)
            {
                for (int j = 0; j < h; ++j)
                {
                    AStarNode node = new AStarNode(i, j, k, Random.Range(0, 100) < 20 ? E_Node_Type.Stop : E_Node_Type.Walk);
                    nodes[i, j, k] = node;
                }
            }
        }


    }
    //寻路
    public List<AStarNode> FindPath(Vector3 startpos, Vector3 endpos)
    {
        //实际情况传入的可能是浮点数，此处省略换算过程
        //判断结点是否合法
        if (startpos.x < 0 || startpos.x > mapW || startpos.y < 0 || startpos.y > mapH ||
        endpos.x < 0 || endpos.x > mapW || endpos.y < 0 || endpos.y > mapH || startpos.z < 0 || startpos.z > mapZ || endpos.z < 0 || endpos.z > mapZ)
            return null;
        //从矩阵中取出这两个点
        AStarNode start = nodes[(int)startpos.x, (int)startpos.y, (int)startpos.z];
        AStarNode end = nodes[(int)endpos.x, (int)endpos.y, (int)endpos.z];
        //判断是否obstacle
        if (start.type == E_Node_Type.Stop || end.type == E_Node_Type.Stop)
            return null;
        //清空上一次的关闭列表
        openlist.Clear();
        closelist.Clear();

        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        //将开始点放入关闭类别，list自带方法
        closelist.Add(start);
        while (true)
        {/*
            for (int i = -1; i <= 1; ++i)
            {
                for (int j = -1; j <= 1; ++j)
                {
                    for (int k = -1; k <= 1; ++k)
                    {
                        if (i != 0 || j != 0 || k != 0)
                        {
                            float f1 = Mathf.Sqrt(i + j + k);
                            NodeToOpen(start.x + i, start.y + j, start.z + k, f1, start, end);
                        }
                    }
                }
            }*/
            //将邻近的点放入open
            NodeToOpen(start.x - 1, start.y - 1, start.z, 1.4f, start, end);
            NodeToOpen(start.x, start.y - 1, start.z, 1, start, end);
            NodeToOpen(start.x - 1, start.y, start.z, 1, start, end);
            NodeToOpen(start.x + 1, start.y - 1, start.z, 1.4f, start, end);
            NodeToOpen(start.x + 1, start.y, start.z, 1, start, end);
            NodeToOpen(start.x, start.y + 1, start.z, 1, start, end);
            NodeToOpen(start.x + 1, start.y + 1, start.z, 1.4f, start, end);
            NodeToOpen(start.x - 1, start.y + 1, start.z, 1.4f, start, end);

            NodeToOpen(start.x - 1, start.y - 1, start.z - 1, 1.7f, start, end);
            NodeToOpen(start.x, start.y - 1, start.z - 1, 1.4f, start, end);
            NodeToOpen(start.x - 1, start.y, start.z - 1, 1.4f, start, end);
            NodeToOpen(start.x + 1, start.y - 1, start.z - 1, 1.7f, start, end);
            NodeToOpen(start.x + 1, start.y, start.z - 1, 1.4f, start, end);
            NodeToOpen(start.x, start.y + 1, start.z - 1, 1.4f, start, end);
            NodeToOpen(start.x + 1, start.y + 1, start.z - 1, 1.7f, start, end);
            NodeToOpen(start.x - 1, start.y + 1, start.z - 1, 1.7f, start, end);

            NodeToOpen(start.x - 1, start.y - 1, start.z + 1, 1.7f, start, end);
            NodeToOpen(start.x, start.y - 1, start.z + 1, 1.4f, start, end);
            NodeToOpen(start.x - 1, start.y, start.z + 1, 1.4f, start, end);
            NodeToOpen(start.x + 1, start.y - 1, start.z + 1, 1.7f, start, end);
            NodeToOpen(start.x + 1, start.y, start.z + 1, 1.4f, start, end);
            NodeToOpen(start.x, start.y + 1, start.z + 1, 1.4f, start, end);
            NodeToOpen(start.x + 1, start.y + 1, start.z + 1, 1.7f, start, end);
            NodeToOpen(start.x - 1, start.y + 1, start.z + 1, 1.7f, start, end);

            NodeToOpen(start.x, start.y, start.z - 1, 1, start, end);
            NodeToOpen(start.x, start.y, start.z + 1, 1, start, end);

            //选出消耗(g)最小的点，list自带排序
            openlist.Sort(sortOpen);

            //把最小的点找出并放入关闭列表,并从open移除
            closelist.Add(openlist[0]);
            start = openlist[0];//将其作为新的起点
            openlist.RemoveAt(0);
            //死路判断
            if (openlist.Count == 0)
            {
                return null;
            }

            if (start == end)
            {
                //开始回溯路径
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while (end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                //把路径弄成正向的
                path.Reverse();
                return path;
            }

        }
    }
    private void NodeToOpen(int x, int y, int z, float g, AStarNode father, AStarNode end)
    {//将点放入openlist

        if (x < 0 || x >= mapW || y < 0 || y >= mapH || z < 0 || z >= mapZ)
            return;//保证xy不超过边界
        AStarNode node = nodes[x, y, z];
        if (node == null || node.type == E_Node_Type.Stop || closelist.Contains(node)
        || openlist.Contains(node))
            return;

        //计算f=g+h

        node.father = father;
        node.g = father.g + g;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y) + Mathf.Abs(end.z - node.z);
        node.f = node.g + node.h;
        //加入openlist
        openlist.Add(node);
    }

    private int sortOpen(AStarNode a, AStarNode b)
    {
        //制定排序规则
        if (a.f > b.f)
            return 1;
        else
            return -1;
    }

}
