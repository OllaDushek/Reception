﻿#pragma checksum "..\..\..\Pages\WorkerMainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "306AE84B357BF1CA211E3F0FD69043839CD2A8CE02B9C15E60FEAF11137830A2"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Reception.Pages;
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


namespace Reception.Pages {
    
    
    /// <summary>
    /// WorkerMainPage
    /// </summary>
    public partial class WorkerMainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\Pages\WorkerMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CheckInButton;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Pages\WorkerMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ServiceButton;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Pages\WorkerMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CheckOutButton;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Pages\WorkerMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DocumentButton;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Pages\WorkerMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BackButton;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Pages\WorkerMainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame frameWork;
        
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
            System.Uri resourceLocater = new System.Uri("/Reception;component/pages/workermainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\WorkerMainPage.xaml"
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
            this.CheckInButton = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\Pages\WorkerMainPage.xaml"
            this.CheckInButton.Click += new System.Windows.RoutedEventHandler(this.CheckInButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ServiceButton = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\Pages\WorkerMainPage.xaml"
            this.ServiceButton.Click += new System.Windows.RoutedEventHandler(this.ServiceButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CheckOutButton = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\Pages\WorkerMainPage.xaml"
            this.CheckOutButton.Click += new System.Windows.RoutedEventHandler(this.CheckOutButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DocumentButton = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\Pages\WorkerMainPage.xaml"
            this.DocumentButton.Click += new System.Windows.RoutedEventHandler(this.DocumentButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BackButton = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\Pages\WorkerMainPage.xaml"
            this.BackButton.Click += new System.Windows.RoutedEventHandler(this.BackButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.frameWork = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
