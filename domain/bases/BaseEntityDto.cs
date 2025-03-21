public class EntityDto<TPkType> : Dto, IEntityDto<TPkType>
{
    public TPkType Id { get; set; }
}
