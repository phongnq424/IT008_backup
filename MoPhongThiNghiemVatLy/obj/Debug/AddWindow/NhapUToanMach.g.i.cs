﻿#pragma checksum "..\..\..\AddWindow\NhapUToanMach.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "AD9B310A95E4671FAFC074D919F6C7C3E40A2C488663533F7A609134537530B3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using MoPhongThiNghiemVatLy.AddWindow;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MoPhongThiNghiemVatLy.AddWindow {
    
    
    /// <summary>
    /// NhapUToanMach
    /// </summary>
    public partial class NhapUToanMach : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 79 "..\..\..\AddWindow\NhapUToanMach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox QuantityTextBox;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\AddWindow\NhapUToanMach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Snackbar ErrorSnackbar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MoPhongThiNghiemVatLy;component/addwindow/nhaputoanmach.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AddWindow\NhapUToanMach.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.QuantityTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 87 "..\..\..\AddWindow\NhapUToanMach.xaml"
            this.QuantityTextBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.OnTextBoxKeyDown);
            
            #line default
            #line hidden
            
            #line 89 "..\..\..\AddWindow\NhapUToanMach.xaml"
            this.QuantityTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.OnTextBoxTextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 102 "..\..\..\AddWindow\NhapUToanMach.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnSaveButtonClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ErrorSnackbar = ((MaterialDesignThemes.Wpf.Snackbar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

