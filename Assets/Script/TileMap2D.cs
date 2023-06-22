using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap2D : MonoBehaviour
{
    [Header("Common")]
    [SerializeField]
    private StageController stageController;
    [SerializeField]
    private StageUI stageUI;

    [Header("Tile")]
    [SerializeField]
    private GameObject tilePrefab;

    [Header("Item")]
    [SerializeField]
    private GameObject ItemPrefab;

    private int maxCoinCount = 0;
    private int curreuntCoinCount = 0;

    public void GenrateTilemap(MapData mapdata)
    {
        int width = mapdata.mapSize.x;
        int height = mapdata.mapSize.y;

        for (int y = 0; y < height; ++y)
        {
            for (int x=0; x< width; ++x)
            {
                int index = y * width + x;

                if(mapdata.mapData[index] == (int)TileType.Empty)
                {
                    continue;
                }

                Vector3 position = new Vector3(-(width * 0.5f - 0.5f) + x, (height * 0.5f - 0.5f) - y);

                if (mapdata.mapData[index] > (int)TileType.Empty && mapdata.mapData[index] < (int)TileType.LastIndex)
                {
                    SpawnTile((TileType)mapdata.mapData[index], position);
                }
                else if (mapdata.mapData[index] == (int)ItemType.Coin)
                {
                    SpawnItem(position);
                }
            }
        }

        curreuntCoinCount = maxCoinCount;
        stageUI.UpdateCoin(curreuntCoinCount);
    }

    private void SpawnTile(TileType tileType, Vector3 position)
    {
        GameObject clone = Instantiate(tilePrefab, position, Quaternion.identity);

        clone.name = "Tile";
        clone.transform.SetParent(transform);

        Tile tile = clone.GetComponent<Tile>();
        tile.Setup(tileType);

    }

    private void SpawnItem(Vector3 position)
    {
        GameObject clone = Instantiate(ItemPrefab, position, Quaternion.identity);

        clone.name = "Item";
        clone.transform.SetParent(transform);
        maxCoinCount ++;
    }

    public void GetCoin(GameObject coin)
    {
        curreuntCoinCount --;
        stageUI.UpdateCoin(curreuntCoinCount);
        coin.GetComponent<Item>().Exit();

        if (curreuntCoinCount == 0)
        {
            stageController.GameClear();
        }
    }
}
