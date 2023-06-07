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
        // �ǂƒʘH�Ɋւ�����
        wall = 0,
        path = 1,

        // ���Ɋւ�����
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

    // �c�Ɖ��̃I�u�W�F�N�g��(���ƍ���)
    private int width;
    private int height;

    private Vector2Int start_pos;
    private Vector2Int goal_pos;

    // �g�p�ł���N�_�̃��X�g
    private List<Vector2Int> startCells;

    // ���H�̃}�b�v
    private MazeData[,] maze_map;
    private MazeData[,] floor_map;

    public createMaze(int w, int h)
    {
        if (w < 5 || h < 5) throw new ArgumentOutOfRangeException();
        if (w % 2 == 0) w++;
        if (h % 2 == 0) h++;
        width = w;
        height = h;

        // �}�b�v�̐���
        maze_map = new MazeData[width, height];
        floor_map = new MazeData[width, height];
        startCells = new List<Vector2Int>();
    }

    // Start is called before the first frame update
    void Start() {    }

    // Update is called once per frame
    void Update(){�@}

    /*
     * @brief  ���H�̃}�b�v���쐬
     */
    public void makeMaze()
    {
        // �}�b�v�̏������i�O���ȊOwall�ɂ���j
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

        // �X�^�[�g�n�_�����߂�
        sysrandom rand = new sysrandom();
        do
        {
            start_pos.x = rand.Next(width - 2) + 1;
            start_pos.y = rand.Next(width - 2) + 1;
        } while (start_pos.x % 2 == 0 || start_pos.y % 2 == 0);

        // ���H�̒T�����s��
        goal_pos = digMaze(start_pos);

        // �O����wall�ɖ߂�
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
     * @brief  ���H���@��
     * 
     * @param  _start[in]  �ŏ��̋N�_���W
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

            //���̎��_�Ō@�����p���Ȃ���΃��[�v�𔲂���
            if (directions.Count == 0) break;

            //���[�v�������Ȃ���Γ����Z�b�g�iSetPath�͌�قǍ쐬�j
            setPath(x, y);
            
            //�@����p�������_���Ŏ擾
            int directionIndex = rand.Next(directions.Count);
            
            //�擾�������p�ɂ���Č@��i�߂�
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

            //���̋N�_���擾
            Vector2Int pos = getStartPos();

            //�����N�_���c���Ă����digMaze�֐������g�ŌĂяo��
            if (pos.x != -1)
            {
                mGoal = digMaze(pos);
            }
        }

        return mGoal;
    }

    /*
     * @brief  �i�߂������T��
     * 
     * @param  _x[in]  ���݂�x���W
     * @param  _y[in]  ���݂�y���W
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
     * @brief  �ʘH����違�N�_���̋L�^
     * 
     * @param  _x[in]  �ʘH�Ƃ���x���W
     * @param  _y[in]  �ʘH�Ƃ���y���W
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
     * @brief  �N�_�ʒu���擾����
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
 * @brief  �쐬�������H�̃}�b�v��n��
 */
    public MazeData[,] getMazeMap()
    {
        return maze_map;
    }

    /*
     * @brief  �쐬����floor�̃}�b�v��n��
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
