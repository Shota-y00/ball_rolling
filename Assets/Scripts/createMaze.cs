using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//using unirandom = UnityEngine.Random;
using sysrandom = System.Random;

public class createMaze
{
    public enum MazeData 
    {
        // 壁と通路に関する種類
        wall = 0,
        path = 1,

        // 床に関する種類
        floor = 2,
        accel = 3
    }

    public enum Direction
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }

    // 縦と横のオブジェクト数(幅と高さ)
    private int width;
    private int height;

    private Vector2Int start_pos;
    private Vector2Int goal_pos;

    // 使用できる起点のリスト
    private List<Vector2Int> startCells;

    // 迷路のマップ
    private MazeData[,] maze_map;
    private MazeData[,] floor_map;

    public createMaze(int w, int h)
    {
        if (w < 5 || h < 5) throw new ArgumentOutOfRangeException();
        if (w % 2 == 0) w++;
        if (h % 2 == 0) h++;
        width = w;
        height = h;

        // マップの生成
        maze_map = new MazeData[width, height];
        floor_map = new MazeData[width, height];
        startCells = new List<Vector2Int>();
    }

    // Start is called before the first frame update
    void Start() {    }

    // Update is called once per frame
    void Update(){　}

    /*
     * @brief  迷路のマップを作成
     */
    public void makeMaze()
    {
        // マップの初期化（外周以外wallにする）
        for(int j = 0; j < height; j++)
        {
            for(int i = 0; i < width; i++)
            {
                if(i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    maze_map[i, j] = MazeData.path;
                }
                else
                {
                    maze_map[i, j] = MazeData.wall;
                }
                floor_map[i, j] = MazeData.floor;
            }
        }

        // スタート地点を決める
        sysrandom rand = new sysrandom();
        do
        {
            start_pos.x = rand.Next(width - 2) + 1;
            start_pos.y = rand.Next(width - 2) + 1;
        } while (start_pos.x % 2 == 0 || start_pos.y % 2 == 0);

        // 迷路の探索を行う
        goal_pos = digMaze(start_pos);

        // 外周をwallに戻す
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                {
                    maze_map[i, j] = MazeData.wall;
                }
            }
        }
    }

    /*
     * @brief  迷路を掘る
     * 
     * @param  _start[in]  最初の起点座標
     */
    private Vector2Int digMaze(Vector2Int _start)
    {
        int x = _start.x;
        int y = _start.y;
        Vector2Int mGoal = new Vector2Int(x, y);
        sysrandom rand = new sysrandom();

        while (true)
        {
            List<Direction> directions = searchDirections(x, y);

            //この時点で掘れる方角がなければループを抜ける
            if (directions.Count == 0) break;

            //ループが抜けなければ道をセット（SetPathは後ほど作成）
            setPath(x, y);
            
            //掘る方角をランダムで取得
            int directionIndex = rand.Next(directions.Count);
            
            //取得した方角によって掘り進める
            switch (directions[directionIndex])
            {
                case Direction.UP:
                    setPath(x, --y);
                    setPath(x, --y);
                    break;
                case Direction.RIGHT:
                    setPath(++x, y);
                    setPath(++x, y);
                    break;
                case Direction.DOWN:
                    setPath(x, ++y);
                    setPath(x, ++y);
                    break;
                case Direction.LEFT:
                    setPath(--x, y);
                    setPath(--x, y);
                    break;
            }

            //次の起点を取得
            Vector2Int pos = getStartPos();

            //もし起点が残っていればdigMaze関数を自身で呼び出す
            if (pos.x != -1)
            {
                mGoal = digMaze(pos);
            }
        }

        return mGoal;
    }

    /*
     * @brief  進める方向を探す
     * 
     * @param  _x[in]  現在のx座標
     * @param  _y[in]  現在のy座標
     */
    private List<Direction> searchDirections(int _x, int _y)
    {
        List<Direction> direction = new List<Direction>();

        if (maze_map[_x, _y - 1] == MazeData.wall && maze_map[_x, _y - 2] == MazeData.wall)
        {
            direction.Add(Direction.UP);
        }
        if (maze_map[_x + 1, _y] == MazeData.wall && maze_map[_x + 2, _y] == MazeData.wall)
        {
            direction.Add(Direction.RIGHT);
        }
        if (maze_map[_x, _y + 1] == MazeData.wall && maze_map[_x, _y + 2] == MazeData.wall)
        {
            direction.Add(Direction.DOWN);
        }
        if (maze_map[_x - 1, _y] == MazeData.wall && maze_map[_x - 2, _y] == MazeData.wall)
        {
            direction.Add(Direction.LEFT);
        }

        return direction;
    }

    /*
     * @brief  通路を作る＆起点候補の記録
     * 
     * @param  _x[in]  通路とするx座標
     * @param  _y[in]  通路とするy座標
     */
    private void setPath(int _x, int _y)
    {
        maze_map[_x, _y] = MazeData.path;
        if (_x % 2 == 1 && _y % 2 == 1)
        {
            startCells.Add(new Vector2Int(_x, _y));
        }
    }

    /*
     * @brief  起点位置を取得する
     */
    private Vector2Int getStartPos()
    {
        sysrandom rand = new sysrandom();
        if (startCells.Count == 0) return new Vector2Int(-1, -1);
        int idx = rand.Next(startCells.Count);
        Vector2Int pos = startCells[idx];
        startCells.RemoveAt(idx);
        return pos;
    }

    /*
 * @brief  作成した迷路のマップを渡す
 */
    public MazeData[,] getMazeMap()
    {
        return maze_map;
    }

    /*
     * @brief  作成したfloorのマップを渡す
     */
    public MazeData[,] getFloorMap()
    {
        return floor_map;
    }

    public int getWidth()
    {
        return width;
    }

    public int getHeight()
    {
        return height;
    }

    public Vector2Int getStart()
    {
        return start_pos;
    }

    public Vector2Int getGoal()
    {
        return goal_pos;
    }
}
