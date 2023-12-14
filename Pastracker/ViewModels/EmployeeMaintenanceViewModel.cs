using Microsoft.Data.SqlClient;
using Pastracker.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pastracker.ViewModels
{
	public class EmployeeMaintenanceViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private int _id;
        private string _name;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        public EmployeeMaintenanceViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;


        }

        private void ShowEmployeeContents()
        {
            using (var context = new AppDbContext())
            {
                var emploeyy = context.Employees.FirstOrDefault(e => e.Id == this.Id);
                if(emploeyy != null) 
                { 
                    this.Name = emploeyy.Name;
                }
            }



        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Id = navigationContext.Parameters.GetValue<int>(nameof(Id));
            ShowEmployeeContents();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
