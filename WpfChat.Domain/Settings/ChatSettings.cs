using System;
using System.Collections.Generic;
using System.Text;

namespace WpfChat.Domain.Settings;

public class ChatSettings
{
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Path { get; set; } = string.Empty;
    public int RefreshIntervalSec { get; set; }
}
