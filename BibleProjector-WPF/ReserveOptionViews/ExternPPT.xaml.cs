﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BibleProjector_WPF.ReserveOptionViews
{
    /// <summary>
    /// ExternPPT.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExternPPT : UserControl
    {
        public ExternPPT()
        {
            InitializeComponent();
        }

        // ==================================== 항목 파일 열기/새로고침 =================================

        void PPTOpen_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.ReserveOptionViewModels.ExternPPT)this.DataContext)
                .RunModifyOpenPPT();
        }

        void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.ReserveOptionViewModels.ExternPPT)this.DataContext)
                .RunRefreshPPT();
        }

        // ==================================== 실행 =================================

        void PPTRun_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel.ReserveOptionViewModels.ExternPPT)this.DataContext)
                .ShowContent();
        }
    }
}
