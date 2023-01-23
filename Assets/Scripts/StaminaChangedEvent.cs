using System;
using UnityEngine.Events;

public class StaminaChangedEvent : UnityEvent<StaminaChangedEventArgs>
{

}

public class StaminaChangedEventArgs : EventArgs
{
    public float Value;
    public float MaxValue;
}