using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Maploader
{
    public MapData Load(string fileName)
    {
        if (fileName.Contains(".json") == false)
        {
            fileName += ".json";
        }

        fileName = Path.Combine(Application.streamingAssetsPath, fileName);

        string dataASJson = File.ReadAllText(fileName);

        MapData mapData = new MapData();
        mapData = JsonConvert.DeserializeObject<MapData>(dataASJson);

        return mapData;
    }
}
