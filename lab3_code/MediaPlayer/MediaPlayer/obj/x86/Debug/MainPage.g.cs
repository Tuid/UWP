﻿#pragma checksum "D:\vs2017_work\MediaPlayer\MediaPlayer\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BC7C26F4CE4812A7CCE4F3EAC664307A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MediaPlayer
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // MainPage.xaml line 33
                {
                    this.mediaPlayerElement = (global::Windows.UI.Xaml.Controls.MediaPlayerElement)(target);
                }
                break;
            case 2: // MainPage.xaml line 34
                {
                    this.cover = (global::Windows.UI.Xaml.Shapes.Ellipse)(target);
                }
                break;
            case 3: // MainPage.xaml line 53
                {
                    this.mediaSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    ((global::Windows.UI.Xaml.Controls.Slider)this.mediaSlider).ValueChanged += this.mediaSliderValueChanged;
                }
                break;
            case 4: // MainPage.xaml line 54
                {
                    this.recordSeconds = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5: // MainPage.xaml line 39
                {
                    this.coverStoryboard = (global::Windows.UI.Xaml.Media.Animation.Storyboard)(target);
                }
                break;
            case 6: // MainPage.xaml line 45
                {
                    this.picture = (global::Windows.UI.Xaml.Media.ImageBrush)(target);
                }
                break;
            case 7: // MainPage.xaml line 31
                {
                    this.title = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8: // MainPage.xaml line 61
                {
                    this.slowButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.slowButton).Click += this.slowClick;
                }
                break;
            case 9: // MainPage.xaml line 62
                {
                    this.playPauseButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.playPauseButton).Click += this.PlayPauseClick;
                }
                break;
            case 10: // MainPage.xaml line 63
                {
                    this.quickButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.quickButton).Click += this.quickClick;
                }
                break;
            case 11: // MainPage.xaml line 64
                {
                    this.restartButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.restartButton).Click += this.RestartClick;
                }
                break;
            case 12: // MainPage.xaml line 65
                {
                    this.mediaButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.mediaButton).Click += this.SliderFatherClick;
                }
                break;
            case 13: // MainPage.xaml line 72
                {
                    this.Volume = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.Volume).Click += this.SliderFatherClick;
                }
                break;
            case 14: // MainPage.xaml line 79
                {
                    this.openFileButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.openFileButton).Click += this.OpenFileClick;
                }
                break;
            case 15: // MainPage.xaml line 80
                {
                    this.fullScreenButton = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.fullScreenButton).Click += this.FullScreenClick;
                }
                break;
            case 16: // MainPage.xaml line 75
                {
                    this.volumeSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    ((global::Windows.UI.Xaml.Controls.Slider)this.volumeSlider).ValueChanged += this.volumeSliderValueChanged;
                }
                break;
            case 17: // MainPage.xaml line 68
                {
                    this.mediaSlider1 = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    ((global::Windows.UI.Xaml.Controls.Slider)this.mediaSlider1).ValueChanged += this.mediaSliderValueChanged;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

