using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;

public class InteractiveControl : MonoBehaviour
{
  public Text text;
  public GameObject text_canvas;
  private HashSet<InteractivityObj> interactivity_objs = new();
  public InteractivityObj interactivity_obj;
  InputAction interactive_action;
  private bool interactive_key = false;
  private float interactive_key_release_time;
  void Start()
  {
    interactive_action = InputSystem.actions.FindAction("Interactive");
  }

  void Update()
  {
    float now = Time.time;
    if (interactive_action.WasPressedThisFrame()){
      interactive_key = true;
      interactive_key_release_time = now + Time.fixedDeltaTime;
    } else if (interactive_key_release_time < now){
      interactive_key = false;
    }
  }
  void FixedUpdate()
  {
    if(interactive_key){
      Interactive();
      interactive_key = false;
    }
  }
  public void UpdateInteractivityObj(){
    interactivity_obj = null;
    foreach(InteractivityObj obj in interactivity_objs){
      if(obj!=null&&obj.interactivity== true&&obj.text!=null&&obj.text.Length!=0){
        interactivity_obj= obj;
        break;
      }
    }
    if(interactivity_obj == null){
      text_canvas.SetActive(false);
    } else {
      text.text = "["+interactive_action.actionMap["Interactive"].GetBindingDisplayString(InputBinding.MaskByGroup("Keyboard&Mouse"))+"] "+interactivity_obj.text;
      text_canvas.SetActive(true);
    }
  }
  public void Interactive(){
    if(interactivity_obj!=null){
      if(interactivity_obj.interactivity == true){
        string ret_text = interactivity_obj.Interactive(gameObject);
        if(ret_text.Length == 0){
          text_canvas.SetActive(false);
        } else {
          text.text = ret_text;
        }
      }
    }
  }
  void OnTriggerEnter2D(Collider2D col){
    if(col.TryGetComponent<InteractivityObj>(out var obj)){
      interactivity_objs.Add(obj);
      UpdateInteractivityObj();
    }
  }
  void OnTriggerExit2D(Collider2D col){
    if(col.TryGetComponent<InteractivityObj>(out var obj)){
      if(obj.interactivity== true){
        obj.ResetInteractive(gameObject);
      }
      interactivity_objs.Remove(obj);
      UpdateInteractivityObj();
    }
  }
}
