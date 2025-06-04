using System.Collections.Generic;
using UnityEngine;

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

    void Start()
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
                    break;
                }
            }
        }
    }
}
