﻿using SemesterSync.Views;

namespace SemesterSync
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TermDetails), typeof(TermDetails));
            Routing.RegisterRoute(nameof(UpdateClass), typeof(UpdateClass));
            Routing.RegisterRoute(nameof(DetailedClass), typeof(DetailedClass));
            Routing.RegisterRoute(nameof(Progress), typeof(Progress));
        }
    }
}
