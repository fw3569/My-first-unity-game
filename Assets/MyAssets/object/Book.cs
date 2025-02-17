using System.Collections.Generic;
using UnityEngine;

public class Book : InteractivityObj
{
  public List<string>context = new();
  private int page = 0;
  public override string Interactive(GameObject from){
    if(page>=context.Count){
      page-=context.Count;
      return "";
    }
    GetComponent<AudioSource>().Play();
    return context[page++];
  }
  public override void ResetInteractive(GameObject from){
    page = 0;
  }
}
