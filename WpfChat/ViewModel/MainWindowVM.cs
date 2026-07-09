using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Configuration;

using WpfChat.Model;

namespace WpfChat.ViewModel;

public interface IMainViewModel : IBaseViewModel
{ }
public class MainWindowVM : BaseViewModel, IMainViewModel
{
    private readonly IConfigurationRoot _config;

    public MainWindowVM()
    {
        _config = App.GetRequiredService<IConfigurationRoot>();
        UserName = "Ivan";
        FriendName = "Anna";
        Messages = new ObservableCollection<Message>(
            [
                new Message()
                {
                    MessageId = 1,
                    From = UserName,
                    To = FriendName,
                    Body = "Majstrovstvá sveta vo futbale 2026 prebiehajú v USA, Kanade a Mexiku a sú historicky najväčším turnajom – prvýkrát sa ho zúčastňuje 48 tímov, ktoré odohrajú 104 zápasov. Šampionát začal 11. júna a finále je naplánované na 19. júla."
                },
                new Message()
                {
                    MessageId = 2,
                    From = FriendName,
                    To = UserName,
                    Body = "Do play-off postupujú prví dvaja z každej skupiny + 8 najlepších tretích tímov, spolu 32 mužstiev."
                },
                new Message()
                {
                    MessageId = 3,
                    From = UserName,
                    To = FriendName,
                    Body = "Majstrovstvá sveta vo futbale 2026 prebiehajú v USA, Kanade a Mexiku a sú historicky najväčším turnajom – prvýkrát sa ho zúčastňuje 48 tímov, ktoré odohrajú 104 zápasov. Šampionát začal 11. júna a finále je naplánované na 19. júla."
                },
                new Message()
                {
                    MessageId = 4,
                    From = FriendName,
                    To = UserName,
                    Body = "Do play-off postupujú prví dvaja z každej skupiny + 8 najlepších tretích tímov, spolu 32 mužstiev."
                },
                new Message()
                {
                    MessageId = 5,
                    From = UserName,
                    To = FriendName,
                    Body = "Majstrovstvá sveta vo futbale 2026 prebiehajú v USA, Kanade a Mexiku a sú historicky najväčším turnajom – prvýkrát sa ho zúčastňuje 48 tímov, ktoré odohrajú 104 zápasov. Šampionát začal 11. júna a finále je naplánované na 19. júla."
                },
                new Message()
                {
                    MessageId = 6,
                    From = FriendName,
                    To = UserName,
                    Body = "Do play-off postupujú prví dvaja z každej skupiny + 8 najlepších tretích tímov, spolu 32 mužstiev."
                },
                new Message()
                {
                    MessageId = 7,
                    From = UserName,
                    To = FriendName,
                    Body = "Majstrovstvá sveta vo futbale 2026 prebiehajú v USA, Kanade a Mexiku a sú historicky najväčším turnajom – prvýkrát sa ho zúčastňuje 48 tímov, ktoré odohrajú 104 zápasov. Šampionát začal 11. júna a finále je naplánované na 19. júla."
                },
                new Message()
                {
                    MessageId = 8,
                    From = FriendName,
                    To = UserName,
                    Body = "Do play-off postupujú prví dvaja z každej skupiny + 8 najlepších tretích tímov, spolu 32 mužstiev."
                },
                new Message()
                {
                    MessageId = 1,
                    From = UserName,
                    To = FriendName,
                    Body = "Majstrovstvá sveta vo futbale 2026 prebiehajú v USA, Kanade a Mexiku a sú historicky najväčším turnajom – prvýkrát sa ho zúčastňuje 48 tímov, ktoré odohrajú 104 zápasov. Šampionát začal 11. júna a finále je naplánované na 19. júla."
                },
                new Message()
                {
                    MessageId = 2,
                    From = FriendName,
                    To = UserName,
                    Body = "Do play-off postupujú prví dvaja z každej skupiny + 8 najlepších tretích tímov, spolu 32 mužstiev."
                },
                new Message()
                {
                    MessageId = 1,
                    From = UserName,
                    To = FriendName,
                    Body = "Majstrovstvá sveta vo futbale 2026 prebiehajú v USA, Kanade a Mexiku a sú historicky najväčším turnajom – prvýkrát sa ho zúčastňuje 48 tímov, ktoré odohrajú 104 zápasov. Šampionát začal 11. júna a finále je naplánované na 19. júla."
                },
                new Message()
                {
                    MessageId = 2,
                    From = FriendName,
                    To = UserName,
                    Body = "Do play-off postupujú prví dvaja z každej skupiny + 8 najlepších tretích tímov, spolu 32 mužstiev."
                },
                new Message()
                {
                    MessageId = 1,
                    From = UserName,
                    To = FriendName,
                    Body = "Majstrovstvá sveta vo futbale 2026 prebiehajú v USA, Kanade a Mexiku a sú historicky najväčším turnajom – prvýkrát sa ho zúčastňuje 48 tímov, ktoré odohrajú 104 zápasov. Šampionát začal 11. júna a finále je naplánované na 19. júla."
                },
                new Message()
                {
                    MessageId = 2,
                    From = FriendName,
                    To = UserName,
                    Body = "Do play-off postupujú prví dvaja z každej skupiny + 8 najlepších tretích tímov, spolu 32 mužstiev."
                },
                new Message()
                {
                    MessageId = 1,
                    From = UserName,
                    To = FriendName,
                    Body = "Majstrovstvá sveta vo futbale 2026 prebiehajú v USA, Kanade a Mexiku a sú historicky najväčším turnajom – prvýkrát sa ho zúčastňuje 48 tímov, ktoré odohrajú 104 zápasov. Šampionát začal 11. júna a finále je naplánované na 19. júla."
                },
                new Message()
                {
                    MessageId = 2,
                    From = FriendName,
                    To = UserName,
                    Body = "Do play-off postupujú prví dvaja z každej skupiny + 8 najlepších tretích tímov, spolu 32 mužstiev."
                },
            ]);
        foreach (var msg in Messages.Where(m => m.From == UserName).ToList())
            msg.Me = 1;
    }

    public string Title { get; set; } = "Chat";
    private string _username = string.Empty;
    public string UserName 
    { 
        get
        {
            return _username;
        }
        set
        {
            _username = value;
            OnPropertyChanged(nameof(UserName));
            SetTitle();
        }
    }
    private string _friendName = string.Empty;
    public string FriendName 
    { 
        get
        {
            return _friendName;
        }
        set
        {
            _friendName = value;
            OnPropertyChanged(nameof(FriendName));
            SetTitle();
        }
    }
    public ICollection<Message> Messages { get; set; }

    private void SetTitle()
    {
        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(FriendName))
            Title = "Chat";
        else
        {
            Title = $"Chat ({UserName} <=> {FriendName})";
            OnPropertyChanged(nameof(Title));
        }
    }

    internal void Connect()
    {
        SetTitle();
    }
}
