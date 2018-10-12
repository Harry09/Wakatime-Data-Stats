using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using ReactiveUI;


namespace WTStats.GUI.ViewModels
{
	public class ViewModelBase : ReactiveObject
    {
		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.RaisePropertyChanged(propertyName);
		}
    }
}
