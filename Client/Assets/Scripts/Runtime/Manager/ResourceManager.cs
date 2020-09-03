using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceManager : Singleton<ResourceManager>
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        return default(T);
    }
}
