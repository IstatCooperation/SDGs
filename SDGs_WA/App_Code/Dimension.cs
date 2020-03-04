public class Dimension
{
    private readonly string name;
    private readonly string label;
    private readonly string key;
    private readonly string table;
    private readonly string desc;
    private readonly bool time;
    private readonly bool integerKey;
    private readonly bool integerValue;
    private readonly string descEn;

    public Dimension(string name, string label, string key, string table, string desc, bool time, bool integerKey, bool integerValue, string descEn)
    {
        this.name = name;
        this.label = label;
        this.key = key;
        this.table = table;
        this.desc = desc;
        this.time = time;
        this.integerKey = integerKey;
        this.integerValue = integerValue;
        this.descEn = descEn;
    }

    public string getName()
    {
        return name;
    }

    public string getVariableLabel()
    {
        return label;
    }

    public string getKey()
    {
        return key;
    }

    public string getTable()
    {
        return table;
    }

    public string getDesc()
    {
        return desc;
    }

    public bool IsTime()
    {
        return time;
    }

    public bool IsIntegerKey()
    {
        return integerKey;
    }

    public bool IsIntegerValue()
    {
        return integerValue;
    }

    public string getDescEn()
    {
        return descEn;
    }

    public string GetVariableName()
    {
        return name;
    }

    public string GetValueSetName()
    {
        return "VS" + name;
    }

    public string GetValuePoolName()
    {
        return "VP" + name;
    }

}

