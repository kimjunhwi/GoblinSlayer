using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	const int DefaultPreLoadCount = 10;

	public static PoolManager Instance { get; private set; }

	readonly Dictionary<string, Queue<GameObject>> poolMap = new Dictionary<string, Queue<GameObject>>();

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		PreLoad("Slime");
	}

	public static GameObject Get(string name)
	{
		return Instance.Get_(name);
	}

	public static void Return(GameObject go, float timer = 0f)
	{
		Instance.Return_(go, timer);
	}

	GameObject Get_(string name)
	{
		GameObject go = null;
		if (!poolMap.ContainsKey(name))
		{
			poolMap[name] = new Queue<GameObject>();
		}
		else if (poolMap[name].Count > 0)
		{
			go = poolMap[name].Dequeue();			
		}

		if (go == null)
		{
			string path = GetPathByName(name);
			if (string.IsNullOrEmpty(path))
			{
				Debug.LogErrorFormat("[Error] path not found : {0}", path);
				return null;
			}

			go = Instantiate(Resources.Load<GameObject>(path));
			go.transform.SetParent(transform);
		}
		go.SetActive(true);
		var pool = go.GetComponent<IPoolObject>();
		if (pool != null)
		{
			pool.OnReset();
		}
		return go;
	}

	void Return_(GameObject go, float timer = 0f)
	{
		StartCoroutine(ReturnProcess(go, timer));
	}

	IEnumerator ReturnProcess(GameObject go, float timer = 0f)
	{
		if (timer > 0f)
			yield return new WaitForSeconds(timer);

		go.SetActive(false);
		var removeCloneName = go.name.Replace("(Clone)", "");

		if (!poolMap.ContainsKey(removeCloneName))
		{
			poolMap[removeCloneName] = new Queue<GameObject>();
		}
		poolMap[removeCloneName].Enqueue(go);
		yield return null;
	}

	private void PreLoad(string name, int count = DefaultPreLoadCount)
	{
		List<GameObject> list = new List<GameObject>();

		// 최소한 1개 이상 만든다.
		count = Mathf.Max(count, 1);
		for (int i = 0; i < count; i++)
		{
			var go = Get(name);
			if (go == null)
				return;
			list.Add(go);
		}

		for (int i = 0; i < list.Count; i++)
		{
			Return(list[i]);
		}
	}

	private static string GetPathByName(string name)
	{
		return "Prefabs/" + name;
	}
}

public interface IPoolObject
{
	void OnReset();
}