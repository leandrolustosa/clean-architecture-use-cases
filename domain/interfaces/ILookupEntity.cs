public interface ILookupEntity<TPkType>
{
	TPkType Key { get; }
	string Value { get; }
	long ParentKey { get; }
}