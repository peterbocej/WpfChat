using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace WpfChat.ViewModel;
public interface IMainViewModel : IBaseViewModel
{ }
public class MainViewModel : BaseViewModel, IMainViewModel
{
    private readonly IConfigurationRoot _config;
    public MainViewModel()
    {
        _config = App.GetRequiredService<IConfigurationRoot>();
    }

    public string Title { get; set; } = "Chat";
}
