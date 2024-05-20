using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimatorHandonInput : MonoBehaviour
{
    public InputActionProperty inputTriggerAction;
    public InputActionProperty inputGripAction;
    public Animator handAnimator;
    // Start is called before the first frame update
    void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = inputTriggerAction.action.ReadValue<float>();
        float gripValue = inputGripAction.action.ReadValue<float>();

        handAnimator.SetFloat("Trigger", triggerValue);
        handAnimator.SetFloat("Grip", gripValue);

    }
}
