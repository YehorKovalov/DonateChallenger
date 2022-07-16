namespace Infrastructure.Services.Abstractions;

public interface IJsonSerializerWrapper
{
    T Deserialize<T>(string value);

    string Serialize<T>(T data);
}