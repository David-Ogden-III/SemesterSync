﻿using C971_Ogden.ViewModel;

namespace C971_Ogden.Pages;

public partial class ActiveTerm : ContentPage
{
    public ActiveTerm(ActiveTermViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    
}
