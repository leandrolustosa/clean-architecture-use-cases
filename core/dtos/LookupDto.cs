public class LookupDto : Dto, ILookupDto
{
	public long Key { get; set; }

	public string Value { get; set; }
}