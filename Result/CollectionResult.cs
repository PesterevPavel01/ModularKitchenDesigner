namespace Result
{
    public class CollectionResult<TItem> : BaseResult<IEnumerable<TItem>>, ISuccessubleResult<TItem>
    {
        public int Count { get; set; }
    }
}
