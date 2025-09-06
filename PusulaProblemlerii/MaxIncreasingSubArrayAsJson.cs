using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

public static class MaxIncreasingSubarraySolution
{
  
    public static string MaxIncreasingSubArrayAsJson(List<int> numbers)
    {
        if (numbers == null || numbers.Count == 0)
            return JsonSerializer.Serialize(System.Array.Empty<int>());

        int n = numbers.Count;

        int bestStart = 0, bestLen = 1;
        long bestSum = numbers[0];

        int currStart = 0, currLen = 1;
        long currSum = numbers[0];

        for (int i = 1; i < n; i++)
        {
            if (numbers[i] > numbers[i - 1])
            {
                currLen++;
                currSum += numbers[i];
            }
            else
            {
                if (currSum > bestSum || (currSum == bestSum && currLen > bestLen))
                {
                    bestSum = currSum; bestStart = currStart; bestLen = currLen;
                }
                currStart = i; currLen = 1; currSum = numbers[i];
            }
        }

        if (currSum > bestSum || (currSum == bestSum && currLen > bestLen))
        {
            bestSum = currSum; bestStart = currStart; bestLen = currLen;
        }

        var result = numbers.GetRange(bestStart, bestLen);
        return JsonSerializer.Serialize(result);
    }
}