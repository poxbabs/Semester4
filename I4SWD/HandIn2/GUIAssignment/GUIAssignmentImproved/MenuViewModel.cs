﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using MvvmFoundation.Wpf;

// ========== Authors ==========  //
// Gruppe 25 SWD: GUI Assignment  //
// Nickolai Lundby Ernst, NE84070 //
// Rune Keenaa Aagaard, 20105809  //
// =============================  //

namespace GUIAssignmentImproved
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        private IPageViewModel _currentPageViewModel;
        private ICommand _changePageCommand;
        private List<IPageViewModel> _pageViewModels;
        public MenuViewModel()
        {
            PageViewModels.Add(new HomeViewModel());
            PageViewModels.Add(new PlayerViewModel());
			PageViewModels.Add(new SoundsViewModel());
            PageViewModels.Add(new GraphicsViewModel());
        }


        public ICommand ChangePageCommand
        {
            get
            {
	                return _changePageCommand ?? (_changePageCommand = new RelayCommand<IPageViewModel>(model => ChangeViewModel((IPageViewModel) model), model => model != null));
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }


        #region INotifyPropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
