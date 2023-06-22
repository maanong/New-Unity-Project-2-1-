using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public static int maxStageCount;

    [SerializeField]
    private TileMap2D tileMap2D;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private StageUI stageUI;

    private void Awake()
    {
        Maploader maploader = new Maploader();

        int index = PlayerPrefs.GetInt("StageIndex") + 1;

        string currentStage = index < 10 ? $"Stage{index}" : $"Stage{index}";

        MapData mapData = maploader.Load(currentStage);

        tileMap2D.GenrateTilemap(mapData);

        playerController.Setup(mapData.playerPosition, mapData.mapSize.y);

        cameraController.Setup(mapData.mapSize.x, mapData.mapSize.y);

        stageUI.UpdateTextStage(currentStage);
    }

    public void GameClear()
    {
        int index = PlayerPrefs.GetInt("StageIndex");

        if (index < maxStageCount - 1)
        {
            index++;
            PlayerPrefs.SetInt("StageIndex", index);
            SceneLoader.LoadScene();
        }
        else
        {
            SceneLoader.LoadScene("Intro");
        }
    }
}
