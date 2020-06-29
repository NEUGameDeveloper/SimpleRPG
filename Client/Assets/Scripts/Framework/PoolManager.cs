using System.IO;
using System.Collections.Generic;
using UnityEngine;

public sealed class PoolManager : Singleton<PoolManager>
{
    private sealed class Pool
    {
        private GameObject m_CacheRoot = null;
        private GameObject m_Prefab = null;
        private readonly Stack<GameObject> m_Cache = new Stack<GameObject>();
        private readonly HashSet<int> m_UsedSet = new HashSet<int>();

        public Pool(string path)
        {
            m_CacheRoot = new GameObject(Path.GetFileName(path));
            m_CacheRoot.transform.SetParent(s_PoolRoot.transform, false);
            m_CacheRoot.transform.localPosition = Vector3.zero;
            m_Prefab = ResourceManager.Instance.Load<GameObject>(path);
        }

        public GameObject Get()
        {
            GameObject go = null;
            if (m_Cache.Count > 0)
            {
                go = m_Cache.Pop();
            }
            else
            {
                go = Object.Instantiate(m_Prefab);
            }
            m_UsedSet.Add(go.GetInstanceID());
            go.transform.SetParent(null);
            go.SetActive(true);
            return go;
        }

        public void Recycle(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(m_CacheRoot.transform);
            m_Cache.Push(go);
            m_UsedSet.Remove(go.GetInstanceID());
        }

        public bool Contains(int instanceID)
        {
            return m_UsedSet.Contains(instanceID);
        }

        public void Clear()
        {
            while (m_Cache.Count > 0)
            {
                var go = m_Cache.Pop();
                Object.Destroy(go);
            }
            m_CacheRoot.transform.DetachChildren();
            m_UsedSet.Clear();
        }
    }

    private static GameObject s_PoolRoot = null;
    private readonly Dictionary<string, Pool> m_PoolDict = new Dictionary<string, Pool>();

    protected override void OnInit()
    {
        s_PoolRoot = new GameObject("PoolRoot");
        s_PoolRoot.transform.position = new Vector3(0, -1000, 0);
        Object.DontDestroyOnLoad(s_PoolRoot);
    }

    public GameObject Get(string path)
    {
        if (!m_PoolDict.TryGetValue(path, out var pool))
        {
            pool = new Pool(path);
            m_PoolDict.Add(path, pool);
        }
        return pool.Get();
    }

    public void Recycle(GameObject go)
    {
        if (go == null)
            return;
        var instanceID = go.GetInstanceID();
        foreach (var item in m_PoolDict)
        {
            if (item.Value.Contains(instanceID))
            {
                item.Value.Recycle(go);
                return;
            }

        }
        Object.Destroy(go);
    }

    public void Clear()
    {
        foreach (var entry in m_PoolDict)
        {
            entry.Value.Clear();
        }
        m_PoolDict.Clear();
    }

    protected override void OnDeInit()
    {
        Object.Destroy(s_PoolRoot);
    }
}
