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

    // Prompt 28: unico punto de reinicio para "Nueva Partida"/"Reiniciar" (ver GameStateManager).
    public void ResetState()
    {
        counts.Clear();
    }

    // Fase 2 (Prompt 35, 9.1): solo lectura para SaveManager, sin exponer el diccionario interno.
    public IReadOnlyDictionary<string, int> GetAllCounts() => counts;

    public void RestoreState(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, int>> savedCounts)
    {
        counts.Clear();
        if (savedCounts == null) return;
        foreach (var kv in savedCounts) counts[kv.Key] = kv.Value;
    }
}
