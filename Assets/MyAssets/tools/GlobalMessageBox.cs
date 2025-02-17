using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GlobalMessageBox : MonoBehaviour
{
  static public GlobalMessageBox instance;
  public float display_time = 3.0f;
  [System.Serializable]
  public struct MsgInfo{
    public float release_time;
    public string msg;
  }
  public Queue<MsgInfo>messages = new();
  private bool change = false;
  public void Awake(){
    instance = this;
  }
  public void FixedUpdate(){
    while(messages.Count!=0&&messages.First().release_time<=Time.fixedTime){
      messages.Dequeue();
      change = true;
    }
    if(change){
      string text = "";
      foreach(MsgInfo message in messages){
        if(text.Length!=0){
          text+="\n";
        }
        text+=message.msg;
      }
      GetComponent<Text>().text = text;
      change = false;
    }
  }
  static public GlobalMessageBox Instance(){
    return instance;
  }
  public void AddMessage(string str){
    messages.Enqueue(new()
    {
      release_time = Time.fixedTime+display_time,
      msg = str
    });
    change = true;
  }
}
