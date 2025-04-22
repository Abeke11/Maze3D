
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] LevelDatabaseSO levelDatabase;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonsContainer;

    void Start()
    {
        for (int i = 0; i < levelDatabase.levels.Length; i++)
        {
            int idx = i;
            var btnObj = Instantiate(buttonPrefab, buttonsContainer);
            var txt = btnObj.GetComponentInChildren<TMP_Text>();
            txt.text = $"Level {i + 1}";
            var btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => LevelLoader.LoadLevel(idx));
        }
    }
}
