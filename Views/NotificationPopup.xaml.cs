using SemesterSync.ViewModel;
using CommunityToolkit.Maui.Views;

namespace SemesterSync.Views;

public partial class NotificationPopup : Popup
{
    public NotificationPopup(NotificationPopupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }


    private NotificationPopupViewModel vm;
    public void Cancel_Clicked(object? sender, EventArgs e)
    {
        Close();
    }

    public void Save_Clicked(object? sender, EventArgs e)
    {
        vm.Save();
        Close();
    }
}