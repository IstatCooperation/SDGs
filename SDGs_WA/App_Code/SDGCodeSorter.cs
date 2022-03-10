using System.Collections.Generic;

public class SDGCodeSorter : IComparer<string>
{
    public int Compare(string x, string y)
    {
        string[] a = x.Split('.');
        string[] b = y.Split('.');
        for (int i = 0; i < a.Length&&i<b.Length; i++)
        {
            int i1, i2;
            bool num1 = int.TryParse(a[i], out i1);
            bool num2 = int.TryParse(b[i], out i2);
            if (num1 && num2)
            {
                if (i1 != i2)
                {
                    return i1 - i2;
                }
            }
            else if (num1)
            {
                return -1;
            }
            else if (num2)
            {
                return 1;
            }
            else
            {
                if (!a[i].Equals(b[i]))
                {
                    return a[i].CompareTo(b[i]);
                }
            }
        }
        return 0;
    }
}