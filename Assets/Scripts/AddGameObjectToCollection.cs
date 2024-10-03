using System;
using ScriptableObjectArchitecture;
using UnityEngine;

public class AddGameObjectToCollection : MonoBehaviour {
    [SerializeField] private GameObjectCollection gameObjectCollection;

    private void Awake() {
        gameObjectCollection.Add(gameObject);
    }

    private void OnDestroy() {
        gameObjectCollection.Remove(gameObject);
    }
}