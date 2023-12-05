using Microsoft.Identity.Client;
using Pastracker.Models;
using Pastracker.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;


namespace Pastracker.ViewModels
{
	public class DashboardViewModel : BindableBase
	{
        private readonly IRegionManager _regionManager;
        private ObservableCollection<MoveContent> _moveContents;
        private ObservableCollection<Company> _companies;

        private int[] _years = new int[7];

        public int[] Years
        {
            get { return _years; }
            set { SetProperty(ref _years, value); }
        }
        public ObservableCollection<MoveContent> MoveContents
        {
            get { return _moveContents; }
            set { SetProperty(ref _moveContents, value); }
        }
        public ObservableCollection<Company> Companies
        {
            get { return _companies; }
            set { SetProperty(ref _companies, value); }
        }
        public DashboardViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            EditorCommand = new DelegateCommand(EditorCommandExecute);


            using (var context = new AppDbContext())
            {
                Companies = new ObservableCollection<Company>(context.Companies.ToList());
            }

            for (int i = 0; i < 7; i++)
            {
                this.Years[i] = (DateTime.Now.Year) - i;
            }

        }
        public DelegateCommand EditorCommand { get; }

        private void EditorCommandExecute()
        {
            // Menu表示
            var p = new NavigationParameters();
            _regionManager.RequestNavigate("ContentRegion", nameof(Editor), p);

        }

        private void YearSelectionChangedExecute(object[] selectedItems)
        {
            try
            {
                var selectedItem = selectedItems[0];
                var year = selectedItem;
                ShowAccountJournalsTable((int)year);
            }
            catch
            {

            }
        }
        private void ShowAccountJournalsTable(int year)
        {
            using var context = new AppDbContext();
            this.MoveContents = new ObservableCollection<MoveContent>(context.MoveContents
                                                                                   .Where(j => j.PickupDate.Year == year)
                                                                                   .ToList());

        }


    }
}
