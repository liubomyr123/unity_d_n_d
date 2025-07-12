using System.Collections.Generic;
using UnityEngine;

public class ArduinoUnoR3Pins : MonoBehaviour
{
    public static List<HeaderPinIdentifier> GetHeaderPins()
    {
        return new List<HeaderPinIdentifier>
        {
            new HeaderPinIdentifier("mother_PC5_left", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PC4_left", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PC3", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PC2", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PC1", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PC0", HeaderPinType.FemaleHeaderPin),

            new HeaderPinIdentifier("mother_VIN_IN", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_GND_left_1", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_GND_left_2", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_5V_OUT", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_3V3_OUT", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PC6", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_IOREF", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_NC", HeaderPinType.FemaleHeaderPin),

            new HeaderPinIdentifier("mother_PD0", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PD1", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PD2", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PD3", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PD4", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PD5", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PD6", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PD7", HeaderPinType.FemaleHeaderPin),

            new HeaderPinIdentifier("mother_PB0", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PB1", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PB2", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PB3", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PB4", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PB5", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_GND_rigth_1", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_AREF", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PC4_rigth", HeaderPinType.FemaleHeaderPin),
            new HeaderPinIdentifier("mother_PC5_rigth", HeaderPinType.FemaleHeaderPin),

            new HeaderPinIdentifier("father_bottom_1", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_bottom_2", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_bottom_3", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_bottom_4", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_bottom_5", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_bottom_6", HeaderPinType.MaleHeaderPin),

            new HeaderPinIdentifier("father_top_1", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_top_2", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_top_3", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_top_4", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_top_5", HeaderPinType.MaleHeaderPin),
            new HeaderPinIdentifier("father_top_6", HeaderPinType.MaleHeaderPin),
        };
    }
}
