public interface ILookupDto<TPkType>
{
	TPkType Key { get; set; }
	string Value { get; set; }
}