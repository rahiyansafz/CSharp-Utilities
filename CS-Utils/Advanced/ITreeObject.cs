namespace CS_Utils.Advanced;
public interface ITreeObject<T>
{
    public IList<ITreeObject<T>> Children { get; set; }
    public T Id { get; set; }
    public string? Name { get; set; }
    public T? ParentId { get; set; }
}