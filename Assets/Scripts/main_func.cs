using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_func : MonoBehaviour
{

    // ゲームオブジェクト
    public GameObject wall;
    public GameObject floor;
    public GameObject goal;
    private GameObject ball;
    private Ending_func ending;

    // 迷路製作用
    private createMaze maze;
    private createMaze.MazeData[,] mazeMap;
    private createMaze.MazeData[,] floorMap;

    // 迷路のサイズ
    public int width = 13;
    public int height = 13;

    public Vector3 rotation_speed = new Vector3(0.1f, 0.2f, 0.1f);
    private Vector2 lastMousePosition;

    // Start is called before the first frame update
    void Start()
    {

        ending = new Ending_func();

        // 初期化
        width = input_data.w;
        height = input_data.h;
        maze = new createMaze(width, height);
        int w = maze.getWidth();
        int h = maze.getHeight();
        mazeMap = new createMaze.MazeData[w, h];
        floorMap = new createMaze.MazeData[w, h];

        // 迷路マップ作成
        maze.makeMaze();
        mazeMap = maze.getMazeMap();
        floorMap = maze.getFloorMap();
        Vector2Int start_pos = maze.getStart();
        Vector2Int goal_pos = maze.getGoal();
        float wall_height = wall.transform.localScale.y / 2;
        float floor_height = floor.transform.localScale.y / 2;
        Vector2 board_size = new Vector2(wall.transform.localScale.x * w / 2, wall.transform.localScale.y * h / 2);


        // オブジェクト配置
        for (int j = 0; j < h; j++)
        {
            for(int i = 0; i < w; i++)
            {
                // 壁の配置
                if (mazeMap[i, j] == createMaze.MazeData.wall)
                {
                    var wall_obj = Instantiate(wall, new Vector3(i - board_size.x, wall_height, j - board_size.y), Quaternion.identity);
                    wall_obj.transform.parent = this.transform;
                }
                else
                {
                    // 床面の配置
                    var floor_obj = Instantiate(floor, new Vector3(i - board_size.x, -floor_height, j - board_size.y), Quaternion.identity);
                    floor_obj.transform.parent = this.transform;
                }       
            }
        }
        // ゴールを設置
        var clear_obj = Instantiate(goal, new Vector3(goal_pos.x - board_size.x, wall_height, goal_pos.y - board_size.y), Quaternion.identity);
        clear_obj.transform.parent = this.transform;

        ball = GameObject.FindGameObjectWithTag("ball");
        ball.transform.position = new Vector3(start_pos.x - board_size.x, 0.5f, start_pos.y - board_size.y);
    }

    // Update is called once per frame
    void Update()
    {
        // マウスによる操作
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var x = (Input.mousePosition.y - lastMousePosition.y);
            var z = (lastMousePosition.x - Input.mousePosition.x);

            var Angle = Vector3.zero;
            Angle.x = x * rotation_speed.x;
            Angle.z = z * rotation_speed.z;
            

            this.transform.Rotate(Angle);
            lastMousePosition = Input.mousePosition;
        }

        // 台から落ちたら終了
        if(ball.transform.position.y < -5)
        {
            ending.goGameOver();
        }
    }
}
