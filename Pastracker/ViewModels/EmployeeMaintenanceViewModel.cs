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
        private int _companyId;
        private int _branchId;
        private string _name;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        public int CompanyId
        {
            get { return _companyId; }
            set { SetProperty(ref _companyId, value); }
        }
        public int BranchId
        {
            get { return _branchId; }
            set { SetProperty(ref _branchId, value); }
        }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public DelegateCommand RegisterCommand { get; }

        public EmployeeMaintenanceViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            RegisterCommand = new DelegateCommand(RegisterCommandExecute);


        }

        private void RegisterCommandExecute()
        {


            using (var context = new AppDbContext())
            {
                if (this.Id == 0)
                {
                    // Create
                    var e = new Employee
                    {
                        CompanyId = this.CompanyId,
                        BranchId = this.BranchId,
                        Name = this.Name,
                    };
                    context.Employees.Add(e);
                    context.SaveChanges();


                }
                else
                {
                    // Update
                    var e = context.Employees.FirstOrDefault(p => p.Id == this.Id);
                    if (e != null)
                    {

                        e.Id = this.Id;
                        e.CompanyId = this.CompanyId;
                        e.BranchId = this.BranchId;
                        e.Name = this.Name;

                        context.SaveChanges();
                    }
                }
            }
            //InitializeScreen();
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
