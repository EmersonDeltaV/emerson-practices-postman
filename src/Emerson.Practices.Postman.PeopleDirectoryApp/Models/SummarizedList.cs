public class SummarizedList<T>
{
    public SummarizedList(List<T> data)
    {
        Data = data;
    }

    public int Count => Data?.Count ?? 0;

    public List<T> Data { get; set; }
}
