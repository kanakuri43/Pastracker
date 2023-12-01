using Prism.Mvvm;
using Pastracker.Views;
using Prism.Regions;

namespace Pastracker.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private string _title = "Pastracker";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Dashboard));

        }
    }
}
