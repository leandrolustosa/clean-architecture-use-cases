public class LookupDataSourceLoadOptionsBinder : IModelBinder
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		var loadOptions = new LookupDataSourceLoadOptions();
		DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());

		bindingContext.Result = ModelBindingResult.Success(loadOptions);
		return Task.CompletedTask;
	}
}
