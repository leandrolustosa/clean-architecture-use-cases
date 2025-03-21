public interface IEntityDto<TPkType> : IDto
{
	TPkType Id { get; set; }
}