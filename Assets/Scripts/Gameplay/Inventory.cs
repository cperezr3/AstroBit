using System.Collections.Generic;
using UnityEngine;

// Inventario minimalista y generico: solo cuenta items por id (string). Sin peso, slots,
// estadisticas ni equipamiento -- lo suficiente para que la mision de almacenamiento
// (Prompt 20) pueda llevar la cuenta de los modulos de RAM recogidos.
public class Inventory : MonoBehaviour
{
    private static Inventory _instance;
    public static Inventory Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("Inventory");
                _instance = go.AddComponent<Inventory>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private readonly Dictionary<string, int> counts = new Dictionary<string, int>();

    public void AddItem(string id, int amount = 1)
    {
        counts[id] = GetItemCount(id) + amount;
    }

    public bool RemoveItem(string id, int amount = 1)
    {
        int current = GetItemCount(id);
        if (current < amount) return false;
        counts[id] = current - amount;
        return true;
    }

    public bool HasItem(string id, int amount = 1) => GetItemCount(id) >= amount;

    public int GetItemCount(string id) => counts.TryGetValue(id, out int value) ? value : 0;
}
