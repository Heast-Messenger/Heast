namespace Core.Attributes;

public class StringValueAttribute : Attribute
{
    public StringValueAttribute(string value)
    {
        Value = value;
    }

    public string Value { get; protected set; }
}