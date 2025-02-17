using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : InteractivityObj
{
  public List<Toward4> open_direction = new();
  public List<GameObject>related_gameobjs = new();
  public GameObject open_door_anime;
  public override string Interactive(GameObject from){
    bool valid = false;
    Tilemap tilemap = gameObject.GetComponentInParent<Tilemap>();
    Vector3 pos = new();
    int count = 0;
    for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++){
      for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++){
        Vector3Int localPlace = new(n, p, 0);
        if (tilemap.HasTile(localPlace)){
          pos += tilemap.GetCellCenterWorld(localPlace);
          ++count;
        }
      }
    }
    pos/=count;
    Vector2 dir = from.transform.position - pos;
    if(Mathf.Abs(dir.x)>Mathf.Abs(dir.y)){
      if(dir.x<0){
        if(open_direction.Contains(Toward4.LEFT)){
        valid = true;
        }
      } else {
        if(open_direction.Contains(Toward4.RIGHT)){
          valid = true;
        }
      }
    } else {
      if(dir.y<0){
        if(open_direction.Contains(Toward4.DOWN)){
          valid = true;
        }
      } else {
        if(open_direction.Contains(Toward4.UP)){
          valid = true;
        }
      }
    }
    if(valid){
      interactivity = false;
      foreach(GameObject obj in related_gameobjs){
        if(obj.TryGetComponent<Door>(out var door)){
          door.GetComponent<TilemapRenderer>().enabled = false;
        }
      }
      GetComponent<TilemapRenderer>().enabled = false;
      Instantiate(open_door_anime,pos+new Vector3(0.5f,0),transform.rotation,transform);
      return "";
    } else {
      return "Can not open from this side";
    }
  }
  public override void ResetInteractive(GameObject from){
  }
  public void DestroyDoor(){
    foreach(GameObject obj in related_gameobjs){
      if(obj!=null){
        GameManager.Instance().AddDeletedObj(obj.transform.GetPath());
        Destroy(obj);
      }
    }
    GameManager.Instance().AddDeletedObj(transform.GetPath());
    Destroy(gameObject);
  }
}
