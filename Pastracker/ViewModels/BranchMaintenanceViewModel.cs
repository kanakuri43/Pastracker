using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pastracker.ViewModels
{
	public class BranchMaintenanceViewModel : BindableBase
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

        public BranchMaintenanceViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

        }
	}
}
