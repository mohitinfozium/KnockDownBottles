using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ScrollScript : MonoBehaviour
{

    public Scrollbar Target;
    public Button TheOtherButton;
    public float Step =1f;

    public void Increment()
    {
        if (Target == null || TheOtherButton == null) throw new System.Exception("Setup ScrollbarIncrementer first!");
        Target.value = 1;// Mathf.Clamp(Target.value + Step, 0, 1);
        GetComponent<Button>().interactable = Target.value != 1;
        TheOtherButton.interactable = true;
    }

    public void Decrement()
    {
        if (Target == null || TheOtherButton == null) throw new System.Exception("Setup ScrollbarIncrementer first!");
        Target.value = 0;// Mathf.Clamp(Target.value - Step, 0, 1);
        GetComponent<Button>().interactable = Target.value != 0; ;
        TheOtherButton.interactable = true;
    }

}
