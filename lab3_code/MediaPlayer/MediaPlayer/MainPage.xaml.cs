using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Media.Core;
using Windows.Media;
using Windows.Media.Playback;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MediaPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaTimelineController mediaTimeline;
        Windows.Media.Playback.MediaPlayer mediaPlayer;
        MediaSource mediaSource;
        TimeSpan timeSpan;
        public MainPage()
        {
            
            this.InitializeComponent();
            this.InitializePage();
        }

        private void InitializePage() {
            var view = ApplicationView.GetForCurrentView();
            view.ExitFullScreenMode();
            mediaPlayerElement.IsFullWindow = false;
        }

     

        //选择文件
        private async void OpenFileClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            fileOpenPicker.FileTypeFilter.Add(".mp4");
            fileOpenPicker.FileTypeFilter.Add(".wmv");
            fileOpenPicker.FileTypeFilter.Add(".wma");
            fileOpenPicker.FileTypeFilter.Add(".mp3");

            StorageFile file = await fileOpenPicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                mediaPlayer = new Windows.Media.Playback.MediaPlayer();
                mediaSource = MediaSource.CreateFromStream(stream, file.ContentType);
                title.Text = file.Name;
            }
            else {
                mediaPlayer = new Windows.Media.Playback.MediaPlayer();
                mediaSource = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/week4.mp4"));
                title.Text = "week4";
            }
            mediaSource.OpenOperationCompleted += MediaSourceOpenOperationCompleted;
            mediaPlayer.Source = mediaSource;
            mediaPlayerElement.SetMediaPlayer(mediaPlayer);
            mediaTimeline = new MediaTimelineController();
            mediaPlayer.CommandManager.IsEnabled = false;
            mediaPlayer.TimelineController = mediaTimeline;
            mediaTimeline.IsLoopingEnabled = true;
            mediaTimeline.PositionChanged += MediaTimelinePositionChanged;
            if (file!= null && file.ContentType == "audio/mpeg")
            {
               cover.Visibility = Visibility.Visible;
                fullScreenButton.IsEnabled = false;
            }
            else {
                cover.Visibility = Visibility.Collapsed;
                fullScreenButton.IsEnabled = true;
            }
            RestartClick(sender,e);
        }

        //播放与暂停按钮
        private void PlayPauseClick(object sender, RoutedEventArgs e)
        {
            
            if (mediaTimeline != null)
            {
                //暂停之后点击继续或开始
                if (mediaTimeline.State == MediaTimelineControllerState.Paused)
                {
                    coverStoryboard.Begin();
                    mediaTimeline.Resume();
                    playPauseButton.Icon = new SymbolIcon(Symbol.Pause);

                }//播放时暂停
                else if (mediaTimeline.State == MediaTimelineControllerState.Running)
                {
                    mediaTimeline.Pause();
                    coverStoryboard.Pause();
                    playPauseButton.Icon = new SymbolIcon(Symbol.Play);
                }
                else if (mediaTimeline.State == MediaTimelineControllerState.Stalled) {
                    mediaTimeline.Start();
                    coverStoryboard.Begin();
                }
            }
            else
            {
                var messages = new MessageDialog("no file").ShowAsync();
            }

        }
        //重新开始
        private void RestartClick(object sender, RoutedEventArgs e)
        {
           
            if (mediaTimeline != null)
            {
                mediaTimeline.Start();
                mediaSlider.Value = 0;
                mediaSlider1.Value = 0;
                coverStoryboard.Begin();
                playPauseButton.Icon = new SymbolIcon(Symbol.Pause);
            }
            else
            {
                var messages = new MessageDialog("no file").ShowAsync();
            }
        }
        //进度条的拖动
        private void mediaSliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           
            Slider slider = sender as Slider;
            if (slider != null) {
                timeSpan = TimeSpan.FromSeconds(slider.Value);
                mediaTimeline.Position = timeSpan;
            }
        }
        //全屏显示
        private void FullScreenClick(object sender, RoutedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                view.ExitFullScreenMode();
                mediaPlayerElement.IsFullWindow = !mediaPlayerElement.IsFullWindow;
                fullScreenButton.Icon = new SymbolIcon(Symbol.FullScreen);
                mediaSlider.Visibility = Visibility.Visible;
            }
            else {
                if (view.TryEnterFullScreenMode()) {
                    ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
                    fullScreenButton.Icon = new  SymbolIcon (Symbol.BackToWindow);
                    mediaPlayerElement.IsFullWindow = !mediaPlayerElement.IsFullWindow;
                    mediaSlider.Visibility = Visibility.Collapsed;
                }
            }
        }

        //得到媒体的时长，决定slider的最大值
        private async void MediaSourceOpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            timeSpan = sender.Duration.GetValueOrDefault();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                mediaSlider.Minimum = 0;
                mediaSlider.Maximum = timeSpan.TotalSeconds;
                mediaSlider.StepFrequency = 1;

                mediaSlider1.Minimum = 0;
                mediaSlider1.Maximum = timeSpan.TotalSeconds;
                mediaSlider1.StepFrequency = 1;
            });
        }
        //让slider的进度与播放进度一致
        private async void MediaTimelinePositionChanged(MediaTimelineController sender, object args)
        {
            if (timeSpan != TimeSpan.Zero)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (mediaSlider.Maximum < sender.Position.TotalSeconds)
                    {
                        mediaTimeline.Pause();
                        int i = (int)sender.Position.TotalSeconds;
                        recordSeconds.Text = i.ToString() + "s";
                    }
                    else {
                        mediaSlider.Value = sender.Position.TotalSeconds;
                        mediaSlider1.Value = sender.Position.TotalSeconds;
                        int i = (int)sender.Position.TotalSeconds;
                        recordSeconds.Text = i.ToString() + "s";
                    }
                    
                });
            }


        }
        //appbar中的slider显示
        private void SliderFatherClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element != null)
            {
                FlyoutBase.ShowAttachedFlyout(element);
            }
        }
        //改变声音
        private void volumeSliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           
            Slider slider = sender as Slider;
            if (slider != null)
            {
                double value = slider.Value / 100;
                mediaPlayer.Volume = value;

            }
        }

        //快进
        private void quickClick(object sender, RoutedEventArgs e)
        {
            mediaTimeline.ClockRate = 1.5;
        }
        //慢速
        private void slowClick(object sender, RoutedEventArgs e)
        {
            mediaTimeline.ClockRate = 0.5;
        }
    }
}
