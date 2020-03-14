namespace AspCoreGraphQL.Entities
{
    public interface IHasId<TKey>
    {
        public TKey Id { get; }
    }
}