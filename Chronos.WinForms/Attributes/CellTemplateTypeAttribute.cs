namespace Chronos.WinForms.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class CellTemplateTypeAttribute : Attribute
    {
        public Type CellTemplateType { get; }

        public CellTemplateTypeAttribute(Type cellTemplateType)
        {
            CellTemplateType = cellTemplateType;
        }
    }
}
