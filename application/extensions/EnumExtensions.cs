public static class EnumExtensions
{
    /// <summary>
    /// Retorna o nome do Enum, 
    ///     caso este esteja com anotação [DescriptionAttribute] irá retornar seu valor.
    ///     caso contrario retornará o nome da propriedade.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string Descricao(this Enum value)
    {
        return GetCustomDescription(value);
    }

    private static string GetCustomDescription(object objEnum)
    {
        var fi = objEnum.GetType().GetField(objEnum.ToString());
        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return (attributes.Length > 0) ? attributes[0].Description : objEnum.ToString();
    }

    
}