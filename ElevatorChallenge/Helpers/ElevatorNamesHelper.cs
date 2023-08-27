namespace ElevatorChallenge.Helpers
{
    public static class ElevatorNamesHelper
    {
        private static readonly char[] AlphabetArray = GenerateAlphabetArray();

        public static string GetElevatorName(int number)
        {
            if (number >= 0 && number < AlphabetArray.Length)
            {
                return AlphabetArray[number].ToString();
            }
            else
            {
                return "Unknown";
            }
        }

        private static char[] GenerateAlphabetArray()
        {
            char startChar = 'A';
            char endChar = 'Z';
            int arraySize = endChar - startChar + 1;

            char[] alphabet = new char[arraySize];

            for (int i = 0; i < arraySize; i++)
            {
                alphabet[i] = (char)(startChar + i);
            }

            return alphabet;
        }

    }
}
