public static class IPChecker
{
    public static bool IsIPCorrect(string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;
        
        const int min = 0;
        const int max = 255;
        const char stopSign = '.';
        int j = 0;
        for (int i = 0; i < str.Length; i += j)
        {
            string currentByte = string.Empty;
            
            if (str[i] == stopSign)
                continue;
            
            for (j = 0; i + j < str.Length &&  str[i + j] != stopSign; j++)
            {
                currentByte += str[i + j];

                if (j == 3)
                {
                    return false;
                }
            }

            int num = int.Parse(currentByte);
            if (num is < min or > max)
            {
                return false;
            }
        }

        return true;
    }
}
