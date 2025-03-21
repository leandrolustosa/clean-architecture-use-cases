public class LookupEntity<TPkType> : ILookupEntity<TPkType>
{
    public TPkType Key { get; set; }
    public string Value { get; set; }
    public long ParentKey { get; set; }
}
