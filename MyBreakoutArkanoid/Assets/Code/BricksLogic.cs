using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyQRLIB;

public class BricksLogic : MonoBehaviour
{
    #region Singleton

    private static BricksLogic _instance;

    public static BricksLogic Instance => _instance;

    public static event Action OnLevelLoaded;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private int maxRows = 33;
    private int maxCols = 33;
    private GameObject bricksContainer;
    private float initialBrickSpawnPositionX = -3.2f;
    private float initialBrickSpawnPositionY = 4.7f;
    private float shiftAmount = 0.16f;

    public Brick brickPrefab;

    public Sprite[] Sprites;

    public Color[] BrickColors;

    public List<Brick> RemainingBricks { get; set; }

    public List<int[,]> LevelsData { get; set; }

    public int InitialBricksCount { get; set; }

    public int CurrentLevel;

    private void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");

        this.GenerateBricks();
    }

    public void LoadNextLevel()
    {
        this.CurrentLevel++;

        if (this.CurrentLevel >= this.LevelsData.Count)
        {
            GameLogic.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
        }
    }

    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();
    }

    private void ClearRemainingBricks()
    {
        foreach (Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    private void GenerateBricks()
    {
        this.RemainingBricks = new List<Brick>();
        //int[,] currentLevelData = this.LevelsData[this.CurrentLevel];
        bool[,] currentLevelData =  MyQRLIB.GenQR.Generate("ajksdkasjfhksdfhkjasddsfgagadfgdfgadgdfgafgadfg");
        this.maxRows = currentLevelData.GetLength(0);
        this.maxCols = currentLevelData.GetLength(1);

        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0;

        for (int row = 0; row < this.maxRows; row++)
        {
            for (int col = 0; col < this.maxCols; col++)
            {
                bool brickOn = currentLevelData[row, col];

                if (brickOn)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    //newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], this.BrickColors[brickType], brickType);

                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmount;
                if (col + 1 == this.maxCols)
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmount;
        }

        this.InitialBricksCount = this.RemainingBricks.Count;
        OnLevelLoaded?.Invoke();
    }

}
