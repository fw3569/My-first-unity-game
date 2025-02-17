using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
  protected float value_max = 1.0f;
  protected float value = 1.0f;
  public void UpdateGauge(float value, float value_max){
    this.value_max = value_max;
    this.value = value;
  }

  protected void Update()
  {
    GetComponent<Slider>().value = value*1.0f/value_max;
  }
}
