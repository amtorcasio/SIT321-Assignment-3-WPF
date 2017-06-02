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
using System.Windows.Shapes;
using SARMS;
using SARMS.Users;
using SARMS.Content;

namespace SIT321_Assignment_3_WPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for FeedbackComment.xaml
    /// </summary>
    public partial class FeedbackComment : Window
    {
        private Administrator Admin;

        public FeedbackComment(Administrator admin, Account student, List<Unit> units)
        {
            InitializeComponent();

            Admin = admin;
        }
    }
}
