using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class NPCManager : MonoBehaviour
{
	List<NPC> npcs = new List<NPC>(); 

	public void AddNPC(NPC npc) // 不支持扩展公用NPC RoleID
	{
		if (npc == null)
		{
			return; 
		}
//		Debug.LogError("AddNPC!!!" + npc._roleIdent); 
		var index = npcs.FindIndex((NPC obj) => obj._roleIdent == npc._roleIdent); 
		if (index > 0)
		{
			npcs.RemoveAt(index); 
		}
		npcs.Add(npc); 
	}

	public void RemoveNPC(NPC npc) // 此处可能有BUG，销毁的时候没有移除
	{
//		Debug.LogError("RemoveNPC!!!" + npc._roleIdent); 
		npcs.Remove(npc); 
	}

	public void RemoveNPC(ERole roleIdent)
	{
//		Debug.LogError("Remove NPC0: " + roleIdent); 
		var index = npcs.FindIndex((NPC obj) => obj._roleIdent == roleIdent); 
		if (index >= 0)
		{
//			Debug.LogError("Remove NPC1: " + roleIdent); 
			npcs.RemoveAt(index); 
		}
	}

	public T GetNPC<T>(ERole roleIdent) where T : class
	{
		for (int i = 0, max = npcs.Count; i < max; i++)
		{
			var n = npcs[i];
			Debug.LogError("n: roleIdent: " + n._roleIdent + ", n == null: " + (n == null)); 
		}

		for (int i = 0; i < npcs.Count; i++)
		{
			var n = npcs[i]; 
			if (n == null)
			{
				npcs.RemoveAt(i); 
			}
		}
		return npcs.Find((NPC obj) => obj._roleIdent == roleIdent) as T; 
	}

	public bool Contains(ERole roleIdent)
	{
		return npcs.Find((NPC obj) => obj._roleIdent == roleIdent) != null; 
	}
}
