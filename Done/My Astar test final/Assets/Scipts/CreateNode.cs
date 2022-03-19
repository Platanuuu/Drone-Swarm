using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNode : MonoBehaviour
{
    public Material red;
    public Material yellow;
    public Material green;
    public Material white;

    public Material transparent;

    int n = 0;

    //创建一个小球走
    public GameObject sphere;

    public float movespeed;

    List<AStarNode> list;//存储最短路径

    //左上角第一个立方体位置
    public int beginX;
    public int beginY;

    public int beginZ;
    //其他立方体的偏移
    public int offsetX;
    public int offsetY;
    public int offsetZ;
    //存立方体名字
    private Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();

    public int mapH;
    public int mapW;
    public int mapZ;
    private Vector3 beginpos = Vector3.right * -1;//开始给他一个为负的坐标点
    private Vector3 endpos = Vector3.right * -1;

    // Start is called before the first frame update
    void Start()
    {
        AStarMgr.GetInstance().InitMapInfo(mapW, mapH, mapZ);
        for (int i = 0; i < mapW; ++i)
        {
            for (int k = 0; k < mapZ; ++k)
            {


                for (int j = 0; j < mapH; ++j)
                {
                    //创建立方体
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY, beginZ + k * offsetZ);
                    obj.name = i + "_" + j + "_" + k;
                    cubes.Add(obj.name, obj);//存储立方体名字到字典
                                             //得到初始化的结点
                    AStarNode node = AStarMgr.GetInstance().nodes[i, j, k];
                    if (node.type == E_Node_Type.Stop)
                    {
                        obj.GetComponent<MeshRenderer>().material = red;
                    }
                    else
                    {
                        obj.GetComponent<MeshRenderer>().material = transparent;
                    }
                }
            }
        }
        /*
                beginpos.x = 2;
                beginpos.y = 1;
                endpos.x = 4;
                endpos.y = 4;
                List<AStarNode> list = AStarMgr.GetInstance().FindPath(beginpos, endpos);//寻路
                if (list != null)
                {
                    for (int i = 0; i < list.Count; ++i)
                    {
                        cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = green;
                    }
                }
        */

    }

    // Update is called once per frame
    void Update()
    {
        //按下鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out info, 1000))
            {
                //清理上一次的绿色立方体，变成白色


                if (beginpos == Vector3.right * -1)
                {
                    string[] strs = info.collider.gameObject.name.Split('_');//得到立方体信息
                    sphere.transform.position = cubes[strs[0] + "_" + strs[1] + "_" + strs[2]].transform.position;
                    beginpos = new Vector3(int.Parse(strs[0]), int.Parse(strs[1]), int.Parse(strs[2]));//得到行列位置，即开始点的位置
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;

                }
                //有起点了，就点出终点并开始寻路
                else
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                    Vector3 endpos = new Vector3(int.Parse(strs[0]), int.Parse(strs[1]), int.Parse(strs[2]));

                    list = AStarMgr.GetInstance().FindPath(beginpos, endpos);//寻路
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; ++i)
                        {
                            cubes[list[i].x + "_" + list[i].y + "_" + list[i].z].GetComponent<MeshRenderer>().material = green;
                        }
                    }
                }

            }
        }
        if (list != null && n < list.Count)
        {

            Vector3 target = cubes[list[n].x + "_" + list[n].y + "_" + list[n].z].transform.position;
            if (sphere.transform.position == target)
            {
                n++;
            }

            sphere.transform.position = Vector3.MoveTowards(sphere.transform.position, target, movespeed * Time.deltaTime);
        }
    }
}
