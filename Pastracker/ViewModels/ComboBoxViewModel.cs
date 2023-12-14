using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastracker.ViewModels
{
    public sealed class ComboBoxViewModel
    {
        public ComboBoxViewModel(int Id, String Name)
        {
            SelectedValue = Id;
            DisplayMember = Name;

        }

        public int SelectedValue { get; }
        public string DisplayMember { get; }
    }
}
