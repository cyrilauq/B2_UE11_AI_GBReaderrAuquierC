using System.Text.RegularExpressions;

namespace GBReaderAuquierC.Domains;

public class Isbn
{
    public static bool IsValid(string isbn)
    {
        if(isbn is null || !new Regex(@"^\d{10,11}$").IsMatch(isbn)) { return false; }
        string isbnSys = ConvertForSys(isbn);
        if (isbnSys.Length == 11 && !Regex.IsMatch(isbnSys[9..11], @"(10|11)$")) { return false; }

        int count = 10, calc = 0, nControle = Int32.Parse(isbnSys[9..isbn.Length]);
        var temp = isbnSys[..9];
        foreach (var c in temp)
        {
            calc += int.Parse(c.ToString()) * count;
            count--;
        }
        return nControle == (11 - (calc % 11));
    }

    public static string ConvertForUser(string isbn)
    {
        if(isbn is not null && isbn.Length == 11) {
            return isbn.EndsWith("10") ? isbn[..9] + "X": isbn[..9] + "0";
        }
        return isbn;
    }

    public static string ConvertForSys(string isbn)
    {
        if(isbn is not null && ((isbn.EndsWith("0") && !isbn.EndsWith("10")) || isbn.EndsWith("X"))) {
            return isbn.EndsWith("0") ? isbn[..9] + "11": isbn[..9] + "10";
        }
        return isbn;
    }
}