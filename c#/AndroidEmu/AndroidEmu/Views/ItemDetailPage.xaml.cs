using AndroidEmu.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AndroidEmu.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}