namespace CS_Utils.Helpers;
public static class VerhoeffUtility
{
    private static readonly int[] _inverse = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };

    private static readonly int[,] _multiplication =
    {
        {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
        {1, 2, 3, 4, 0, 6, 7, 8, 9, 5},
        {2, 3, 4, 0, 1, 7, 8, 9, 5, 6},
        {3, 4, 0, 1, 2, 8, 9, 5, 6, 7},
        {4, 0, 1, 2, 3, 9, 5, 6, 7, 8},
        {5, 9, 8, 7, 6, 0, 4, 3, 2, 1},
        {6, 5, 9, 8, 7, 1, 0, 4, 3, 2},
        {7, 6, 5, 9, 8, 2, 1, 0, 4, 3},
        {8, 7, 6, 5, 9, 3, 2, 1, 0, 4},
        {9, 8, 7, 6, 5, 4, 3, 2, 1, 0}
    };

    private static readonly int[,] _permutation =
    {
        {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
        {1, 5, 7, 6, 2, 8, 3, 0, 9, 4},
        {5, 8, 0, 3, 7, 9, 6, 1, 4, 2},
        {8, 9, 1, 6, 0, 4, 3, 5, 2, 7},
        {9, 4, 5, 3, 1, 2, 6, 8, 7, 0},
        {4, 2, 8, 6, 5, 7, 3, 9, 0, 1},
        {2, 7, 9, 3, 8, 0, 6, 4, 1, 5},
        {7, 0, 4, 6, 9, 1, 3, 2, 5, 8}
    };

    public static string Generate(string text)
    {
        var c = 0;
        var myArray = StringToReversedIntArray(text);

        for (var i = 0; i < myArray.Length; i++) c = _multiplication[c, _permutation[(i + 1) % 8, myArray[i]]];

        return _inverse[c].ToString();
    }

    public static bool Validate(string number)
    {
        var c = 0;
        var myArray = StringToReversedIntArray(number);

        for (var i = 0; i < myArray.Length; i++) c = _multiplication[c, _permutation[i % 8, myArray[i]]];

        return c == 0;
    }

    private static int[] StringToReversedIntArray(string number)
    {
        var myArray = new int[number.Length];

        for (var i = 0; i < number.Length; i++) myArray[i] = int.Parse(number.Substring(i, 1));

        Array.Reverse(myArray);

        return myArray;
    }
}