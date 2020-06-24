using System;

public class Singleton<T> where T : Singleton<T>
{
    private static T s_Instance = null;
    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = Activator.CreateInstance<T>();
                s_Instance.OnInit();
            }
            return s_Instance;
        }
    }

    public static void DestroyInstance()
    {
        if (s_Instance != null)
        {
            s_Instance.OnDeInit();
            s_Instance = null;
        }
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnDeInit()
    {
    }
}
