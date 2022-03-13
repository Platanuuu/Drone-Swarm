using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;//需要引入Pathfinding
public class Pathfind : MonoBehaviour {

    public GameObject target;  //目标物
    private Seeker seeker1;
    private Path path1;  //路径
    private float nextwaypointDistance=1; //距离下一个点的距离
    private int currentwaypoint = 0; //当前路点
    public float speed = 100;
    public float rotspeed = 60; //旋转速度
    // Use this for initialization

    void Start () {
        seeker1 = this.GetComponent<Seeker>();
        seeker1.pathCallback += OnpathCompeleter;
        seeker1.StartPath(transform.position,target.transform.position);
    }
    // Update is called once per frame

    void Update () {
        if (path1==null) {
            print("为空了");
            return;
        }

        if (currentwaypoint>=path1.vectorPath.Count) {
            return;
        }

        Vector3 dir = (path1.vectorPath[currentwaypoint] - transform.position).normalized;
        dir *= speed * Time.deltaTime;
        transform.Translate(dir,Space.World);
        Quaternion targetrotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation,targetrotation,Time.deltaTime*rotspeed);

        if (Vector3.Distance(transform.position,path1.vectorPath[currentwaypoint]) < nextwaypointDistance){
            currentwaypoint++;
            return;
        }
    }

    private void OnpathCompeleter(Path p) {

        if (!p.error) {
            path1=p;
            currentwaypoint = 0;
        }
    }

}
