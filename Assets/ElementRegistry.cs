using System.Collections.Generic;

public static class ElementRegistry
{
    public static Dictionary<string, string> UIElementsDic = new Dictionary<string, string>
    {
        { "FATHER", "Arduino_Father" },
        { "MOTHER", "Arduino_Mother" },
        { "UNO", "Arduino_Uno_With_Pins" },

        { "Arduino_Father", "FATHER" },
        { "Arduino_Mother", "MOTHER" },
        { "Arduino_Uno_With_Pins", "UNO" },
    };
}
