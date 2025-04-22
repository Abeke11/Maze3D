using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class LevelResult
{
    public int levelIndex;
    public float bestTime;
}

[System.Serializable]
class ProgressData
{
    public List<LevelResult> results = new List<LevelResult>();
}

public class ProgressService : IProgressService
{
    const string FileName = "progress.json";
    readonly string _path;
    readonly Dictionary<int, float> _bestTimes = new Dictionary<int, float>();

    public ProgressService()
    {
        _path = Path.Combine(Application.persistentDataPath, FileName);
        Load();
    }

    void Load()
    {
        if (!File.Exists(_path)) return;
        try
        {
            var json = File.ReadAllText(_path);
            var data = JsonUtility.FromJson<ProgressData>(json);
            _bestTimes.Clear();
            foreach (var r in data.results)
                _bestTimes[r.levelIndex] = r.bestTime;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ProgressService load error: {e}");
        }
    }

    void Save()
    {
        var data = new ProgressData();
        foreach (var kv in _bestTimes)
            data.results.Add(new LevelResult { levelIndex = kv.Key, bestTime = kv.Value });
        try
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_path, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ProgressService save error: {e}");
        }
    }

    public float GetBestTime(int index)
    {
        return _bestTimes.TryGetValue(index, out var t) ? t : -1f;
    }

    public void RecordResult(int index, float time)
    {
        if (time < 0) return;
        if (!_bestTimes.ContainsKey(index) || time < _bestTimes[index])
        {
            _bestTimes[index] = time;
            Save();
        }
    }
}
