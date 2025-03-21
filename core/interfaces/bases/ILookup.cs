public interface ILookup
{
	long Key { get; }
	string Value { get; }
	long ParentKey { get; }
}