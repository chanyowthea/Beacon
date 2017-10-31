using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Handle
{
	public static void SetLayerIncludeChildren(this Transform trans, int layer)
	{
		trans.gameObject.layer = layer;
		for (int i = 0, count = trans.childCount; i < count; i++)
		{
			trans.GetChild(i).SetLayerIncludeChildren(layer);
		}
	}

	public static List<T> GetAllComponentsInChildren<T>(this Transform trf) where T : Component
	{
		List<T> list = null;
		T t = trf.GetComponent<T>();
		if (t != null)
		{
			if (list == null) { list = new List<T>(); }
			list.Add(t);
		}
		for (int i = 0; i < trf.childCount; i++)
		{
			List<T> childrenList = trf.GetChild(i).GetAllComponentsInChildren<T>();
			if (childrenList == null) { continue; }

			if (list == null) { list = new List<T>(); }
			list.AddRange(childrenList);
		}
		return list;
	}
}