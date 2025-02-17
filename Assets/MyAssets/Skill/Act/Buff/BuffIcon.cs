using UnityEngine;

public class BuffIcon : MonoBehaviour
{
  public float duration = 1.0f;
  public float start_time = 0.0f;
  public GameObject black_mask;
  public GameObject cold_mask;
  public void FixedUpdate(){
    Vector3 local_scale = new(1,Mathf.Max(0,Mathf.Min(1,(Time.fixedTime-start_time)/duration)));
    black_mask.GetComponent<RectTransform>().localScale = local_scale;
    cold_mask.GetComponent<RectTransform>().localScale = local_scale;
  }
}
