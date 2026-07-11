using System.Reflection;

namespace WpfChat.Domain.Settings;

public class ChatSettings
{
    private Type _apiServiceType = default!;
    public string ApiServiceTypeName { get; set; } = string.Empty;
    public Type ApiServiceType
    {
        get
        {
            if (_apiServiceType == null)
            {
                var typeItems = ApiServiceTypeName.Split(',', StringSplitOptions.RemoveEmptyEntries);
                switch (typeItems.Length)
                {
                    case 1:
                        _apiServiceType = Type.GetType(typeItems[0])
                            ?? throw new ApplicationException("Error creating api service");
                        break;
                    case 2:
                        var assembly = Assembly.Load(new AssemblyName(typeItems[1]));
                        if (assembly == null)
                            throw new ApplicationException("Error creating api service assembly");
                        _apiServiceType = assembly.GetType(typeItems[0])
                            ?? throw new ApplicationException("Error creating api service");
                        break;
                    default:
                        throw new ApplicationException($"Could not load type '{ApiServiceTypeName}'.");
                }
            }
            return _apiServiceType;
        }
    }
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Path { get; set; } = string.Empty;
    public int RefreshIntervalSec { get; set; }
}
