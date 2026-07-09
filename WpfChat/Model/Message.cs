using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WpfChat.Model;

public class Message
{
    [Key]
    public int MessageId { get; set; }
    [Required]
    public string From { get; set; } = default!;
    [Required]
    public string To { get; set; } = default!;
    [Required]
    public string Body { get; set; } = default!;
    public DateTime Time { get; set; } = DateTime.Now;
    [IgnoreDataMember]
    public byte Me { get; set; } = 0;
    public string FromAndTime => $"{From}\n{Time.ToString("H:m:s")}";
}
