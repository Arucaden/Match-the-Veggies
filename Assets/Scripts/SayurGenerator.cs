using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SayurGenerator : MonoBehaviour
{
    [SerializeField]
    List<SayurController> _sayurPrefabs = new();
    [SerializeField]
    List<SayurContainer> _sayurContainerPrefabs = new();
    [SerializeField]
    Transform[] _sayurSpawnPoints;
    [SerializeField]
    Transform[] _sayurContainerSpawnPoints;
    List<SayurContainer> _sayurToGenerate = new();
    [SerializeField]
    KelinciController _kelinciController;
    [SerializeField]
    Transform _basketTransform;
    int _roundCount = 0;
    int _sayurMatchedCount = 0;
    [SerializeField]
    Transform _cartTransform;
    List<SayurController> _generatedSayur = new();


    public void Start()
    {
        GenerateSayur();
    }

    public void NextRound()
    {
        _roundCount++;
        if (_roundCount >= 3)
        {
            _kelinciController.Invoke("JumpingAnimation", 1f);

            foreach (SayurController sayur in _generatedSayur)
            {
                sayur.Invoke("PouringAnimation", 0.5f);
                sayur.OnSayurMatch -= OnSayurMatch;
            }
            Invoke("ClearGeneratedSayur", 0.5f);

            _kelinciController.Invoke("PouringAnimation", 0.5f);
            Invoke("BackToMainMenu", 1.5f);
            return;
        }

        foreach (SayurController sayur in _generatedSayur)
        {
            sayur.Invoke("PouringAnimation", 0.5f);
            sayur.OnSayurMatch -= OnSayurMatch;
        }
        Invoke("ClearGeneratedSayur", 0.5f);

        Invoke("GenerateSayur", 0.5f);
        _kelinciController.Invoke("PouringAnimation", 0.5f);
    }

    void GenerateSayur()
    {
        _sayurToGenerate.Clear();
        _sayurToGenerate.AddRange(_sayurContainerPrefabs);

        for (int i = 0; i < _sayurSpawnPoints.Length; i++)
        {
            SayurContainer sayurContainer = Instantiate(_sayurToGenerate[Random.Range(0, _sayurToGenerate.Count)], _sayurContainerSpawnPoints[i].position, Quaternion.identity);
            _sayurToGenerate.Remove(_sayurToGenerate.Find(s => s.SayurEnum == sayurContainer.SayurEnum));


            foreach (SayurController sayurPrefab in _sayurPrefabs)
            {
                if (sayurPrefab.SayurEnum == sayurContainer.SayurEnum)
                {
                    SayurController sayur = Instantiate(sayurPrefab, _sayurSpawnPoints[i].position, Quaternion.identity);
                    _kelinciController.RegisterSayur(sayur);
                    sayur.SetBasketTransform(_basketTransform);
                    sayur.OnSayurMatch += OnSayurMatch;
                    _generatedSayur.Add(sayur);
                    sayur.SetCartTransform(_cartTransform);
                    break;
                }
            }
        }
    }

    public void OnSayurMatch()
    {
        _sayurMatchedCount++;
        if (_sayurMatchedCount == 3)
        {
            NextRound();
            _sayurMatchedCount = 0;
        }
    }

    public void ClearGeneratedSayur()
    {
        _generatedSayur.Clear();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
