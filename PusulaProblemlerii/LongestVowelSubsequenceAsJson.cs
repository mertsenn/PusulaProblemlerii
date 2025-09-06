using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

public static class VowelSubsequenceSolver
{

    public static string LongestVowelSubsequenceAsJson(List<string> words)
    {
        if (words == null || words.Count == 0)
            return JsonSerializer.Serialize(System.Array.Empty<object>());

        var results = new List<object>(words.Count);

        foreach (var original in words)
        {
            var word = original ?? string.Empty;

            int bestStart = 0, bestLen = 0;
            int curStart = 0, curLen = 0;

            for (int i = 0; i < word.Length; i++)
            {
                char c = char.ToLowerInvariant(word[i]);
                bool isVowel = c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';

                if (isVowel)
                {
                    if (curLen == 0) curStart = i;
                    curLen++;
                    if (curLen > bestLen) { bestLen = curLen; bestStart = curStart; }
                }
                else curLen = 0;
            }

            string seq = bestLen > 0 ? word.Substring(bestStart, bestLen) : string.Empty;
            results.Add(new { word = word, sequence = seq, length = bestLen });
        }

        return JsonSerializer.Serialize(results);
    }
}