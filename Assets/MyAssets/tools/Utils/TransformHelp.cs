using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ObjectList{
  public List<GameObject> obj_list;
}

public static class TransformHelp{
  public static List<string> GetPath(this Transform transform){
    List<string> ret = new();
    if(transform.parent!=null){
      ret = transform.parent.GetPath();
    }
    ret.Add(transform.name);
    return ret;
  }
  public static string ToStringOverride(this List<string> path){
    string ret = "";
    foreach(string str in path){
      ret += "/" + str;
    }
    return ret;
  }
  public static GameObject FindGameObject(this Transform transform,List<string> path, int i){
    GameObject ret;
    if(i>=path.Count){
      return null;
    }
    if(path[i]==transform.name){
      if(i==path.Count-1){
        return transform.gameObject;
      }
      foreach(Transform child in transform){
        ret = child.FindGameObject(path,i+1);
        if(ret !=null){
          return ret;
        }
      }
    }
    return null;
  }
}
