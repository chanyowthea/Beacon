using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


#if UNITY_EDITOR
using UnityEditor; 

[ExecuteInEditMode]
public class SetTextType : MonoBehaviour
{
	void Reset()
	{
		Font font = (Font)AssetDatabase.LoadAssetAtPath("Assets/Beacon/Resources/" + "青鸟华光简隶变" + ".ttf", typeof(Font));
		Debug.LogError(""); 
		var canvases = FindObjectsOfType<Canvas>(); 
		for (int i = 0, count = canvases.Length; i < count; ++i)
		{
			List<Text> list = Handle.GetAllComponentsInChildren<Text>(canvases[i].transform); 
			if (list == null)
			{
				continue; 
			}
			Text[] texts = list.ToArray(); 
			for (int j = 0, length = texts.Length; j < length; ++j)
			{
				texts[j].font = font; 
			}
		}
		Debug.Log(font); 
		//        Export(); 
	}

//	public static void Export ()
//	{       
//		//UIRoot ur = UIRoot.list [0];
//		ExportPrefab[] ep = GameObject.FindObjectsOfType<ExportPrefab> ();
//		foreach (var e in ep) {
//			GameObject obj = (GameObject)GameObject.Instantiate (e.gameObject);
//			obj.transform.parent = e.gameObject.transform.parent;
//			GameObject.DestroyImmediate (obj.GetComponent<ExportPrefab>(), true);
//
//			string finalpath = Path.Combine ("Assets", e.path);
//			finalpath = Path.Combine (finalpath, e.gameObject.name + ".prefab").Replace ("\\", "/");
//
//			GameObject prefab = CreateNew (obj, finalpath);
//			EditorUtility.SetDirty (prefab);
//			//ExportPrefab re = prefab.GetComponent<ExportPrefab>();
//			//GameObject.DestroyImmediate(re, true);            
//			GameObject.DestroyImmediate (obj, true);
//			Debug.Log("Export prefabs"); 
//		}
//		AssetDatabase.SaveAssets ();
//	}

//	public static GameObject CreateNew (GameObject obj, string localPath)
//	{
//		string path = Path.GetDirectoryName (localPath);
//		if (!Directory.Exists (path)) {
//			Directory.CreateDirectory (path);
//		}
//		Object prefab = AssetDatabase.LoadAssetAtPath (localPath, typeof(GameObject));
//		if (prefab == null) {
//			prefab = PrefabUtility.CreateEmptyPrefab (localPath);
//		}
//		GameObject res = PrefabUtility.ReplacePrefab (obj, prefab, ReplacePrefabOptions.ReplaceNameBased);
//		return res;
//		//GameObject res = PrefabUtility.CreatePrefab(localPath, obj, ReplacePrefabOptions.Default);
//
//		//return res;
//	}
}
#endif
