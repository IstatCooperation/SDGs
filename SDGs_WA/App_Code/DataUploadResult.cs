public class DataUploadResult
{

    private int code;
    private string message;
    private int insertedData;
    private int updatedData;
    private int wrongData;

    public DataUploadResult(int code, string message)
    {
        this.code = code;
        this.message = message;
    }

    public DataUploadResult(int insertedData, int updatedData, int wrongData)
    {
        this.code = 0;
        this.insertedData = insertedData;
        this.updatedData = updatedData;
        this.wrongData = wrongData;
    }

    public int getCode()
    {
        return this.code;
    }

    public string getMessage()
    {
        return this.message;
    }

    public int getInsertedData()
    {
        return this.insertedData;
    }

    public int getUpdatedData()
    {
        return this.updatedData;
    }

    public int getWrongData()
    {
        return this.wrongData;
    }

}

