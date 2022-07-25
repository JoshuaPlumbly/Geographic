using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipInput : ShipInput
{
    void Update()
    {
        Yaw = Input.GetAxis("Yaw");
        Pitch = Input.GetAxis("Pitch");
        Roll = Input.GetAxis("Roll");

        Forward = Input.GetAxis("Vertical");
        Right = Input.GetAxis("Horizontal");
        Up = Input.GetAxis("Elevation");

        CruiseControl = Input.GetKeyDown(KeyCode.C);
        FlightAssist = Input.GetKeyDown(KeyCode.Z);
    }
}

public abstract class ShipInput : MonoBehaviour
{
    public float Yaw { get; private protected set; }
    public float Pitch { get; private protected set; }
    public float Roll { get; private protected set; }
    public float Forward { get; private protected set; }
    public float Right { get; private protected set; }
    public float Up { get; private protected set; }
    public bool CruiseControl { get; private protected set; }
    public bool FlightAssist { get; private protected set; }
}
