using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _buttontxt;
    [SerializeField] Button _toggleButton;
    [SerializeField] GameObject _prefab;
    [SerializeField] float _interval;
    [SerializeField] float _destroyDelay;
    [SerializeField] float _randomSphereRadius;
    [SerializeField] Transform _center;
    [SerializeField] bool _log;
    
    
    bool _isOn;
    Coroutine _routine;
    
    void UpdateTxt() => _buttontxt.text = _isOn ? "ON" : "OFF";

    void Reset()
    {
        _buttontxt = GetComponentInChildren<TextMeshProUGUI>();
        _prefab = null;
        _interval = 0.2f;
        _randomSphereRadius = 1f;
        _center = transform;
    }

    void Start()
    {
        _toggleButton.onClick.AddListener(Toggle);
        UpdateTxt();
    }

    void OnDestroy()
    {
        _toggleButton.onClick.RemoveListener(Toggle);
    }

    public void Toggle()
    {
        _isOn = !_isOn;
        UpdateTxt();

        if (_isOn)
        {

            _routine = StartCoroutine(SpawnRoutine());
        }
        else
        {
            if(_routine!=null) StopCoroutine(_routine);
            _routine = null;
        }


        IEnumerator SpawnRoutine()
        {
            var w = new WaitForSeconds(_interval);
            while (true)
            {
                var randomPoint = _center.position + (Random.insideUnitSphere * _randomSphereRadius);
                var go = Instantiate(_prefab, randomPoint, Quaternion.identity);
                if(_log) Debug.Log("Create");

                if (Mathf.Approximately(0f, _destroyDelay) == false && _destroyDelay > 0f)
                {
                    StartCoroutine(WaitAndRelease(go));
                }
                yield return w;
            }
        }
        
        IEnumerator WaitAndRelease(GameObject go)
        {
            yield return new WaitForSeconds(_destroyDelay);
            if(_log) Debug.Log("Destroy");
            Destroy(go);
        }
    }
}
