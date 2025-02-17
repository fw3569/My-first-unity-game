using UnityEngine;

public class BuffGauge : MonoBehaviour
{
  public float x_distance = 0.0f;
  public void UpdateGauge()
  {
    Vector2 pos = new(0,0);
    foreach(Transform child in transform){
      if(child.gameObject.activeSelf==false){
        continue;
      }
      child.GetComponent<RectTransform>().localPosition = pos;
      pos.x+=x_distance;
    }
  }
}
