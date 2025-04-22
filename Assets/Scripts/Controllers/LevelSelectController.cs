using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] LevelDatabaseSO levelDatabase;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonsContainer;

    IProgressService _progress;

    [Inject]
    public void Construct(IProgressService progressService)
    {
        _progress = progressService;
    }

    void Start()
    {
        for (int i = 0; i < levelDatabase.levels.Length; i++)
        {
            int idx = i;
            var btnObj = Instantiate(buttonPrefab, buttonsContainer);

            var texts = btnObj.GetComponentsInChildren<TMP_Text>();
            var labelText = texts.Length > 0 ? texts[0] : null;
            var bestText = texts.Length > 1 ? texts[1] : null;

            if (labelText != null)
                labelText.text = $"Level {idx + 1}";

            float best = _progress.GetBestTime(idx);
            if (bestText != null)
            {
                bestText.text = best > 0
                    ? $"{best:F1} sec"
                    : "—";
            }

            var btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() =>
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main"));
            btn.onClick.AddListener(() => LevelLoader.SelectedIndex = idx);
        }
    }
}
