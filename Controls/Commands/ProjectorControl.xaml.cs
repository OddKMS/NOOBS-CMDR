﻿using NOOBS_CMDR.Commands;
using NOOBS_CMDR.Extensions;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace NOOBS_CMDR.Controls.Commands
{
    /// <summary>
    /// Interaction logic for ActionControl.xaml
    /// </summary>
    public partial class ProjectorControl : UserControl
    {
        public static readonly DependencyProperty commandProperty = DependencyProperty.Register("Command", typeof(ProjectorCommand), typeof(ProjectorControl));

        public ProjectorCommand Command
        {
            get { return (ProjectorCommand)GetValue(commandProperty); }
            set { SetValue(commandProperty, value); }
        }

        public ProjectorControl()
        {
            InitializeComponent();

            this.Loaded += delegate
            {
                ProjectorTypeComboSetup();
                IsWindowedComboSetup();
                SceneComboSetup();
                SourceComboSetup();
            };
        }

        private void ProjectorTypeComboSetup()
        {
            this.ProjectorTypeCombo.SelectedValuePath = "Value";
            this.ProjectorTypeCombo.DisplayMemberPath = "Key";

            ProjectorTypeCombo.Items.Add(new KeyValuePair<string, ProjectorCommand.Type>("Preview", ProjectorCommand.Type.Preview));
            ProjectorTypeCombo.Items.Add(new KeyValuePair<string, ProjectorCommand.Type>("Program", ProjectorCommand.Type.StudioProgram));
            ProjectorTypeCombo.Items.Add(new KeyValuePair<string, ProjectorCommand.Type>("Scene", ProjectorCommand.Type.Scene));
            ProjectorTypeCombo.Items.Add(new KeyValuePair<string, ProjectorCommand.Type>("Source", ProjectorCommand.Type.Source));
            ProjectorTypeCombo.Items.Add(new KeyValuePair<string, ProjectorCommand.Type>("Multi View", ProjectorCommand.Type.MultiView));
        }

        private void IsWindowedComboSetup()
        {
            this.IsWindowedCombo.SelectedValuePath = "Value";
            this.IsWindowedCombo.DisplayMemberPath = "Key";

            IsWindowedCombo.Items.Add(new KeyValuePair<string, bool>("Windowed", false));
            IsWindowedCombo.Items.Add(new KeyValuePair<string, bool>("Fullscreen", true));
        }

        private void RefreshScenes()
        {
            SceneCombo.ItemsSource = Command.obs.GetScenes();
        }

        private void RefreshSources()
        {
            SourceCombo.ItemsSource = Command.obs.GetSources(null, true);
        }

        private void SceneComboSetup()
        {
            RefreshScenes();
            SceneCombo.HideSuggestionListBox();
        }

        private void SourceComboSetup()
        {
            RefreshSources();
            SourceCombo.HideSuggestionListBox();
        }

        private void MonitorField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SceneCombo_TextBox_Clicked(object sender, EventArgs e)
        {
            RefreshScenes();
        }

        private void SourceCombo_TextBox_Clicked(object sender, EventArgs e)
        {
            RefreshSources();
        }
    }

    public class Projector_SceneVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((ProjectorCommand.Type)value)
            {
                case ProjectorCommand.Type.Scene:
                    return Visibility.Visible;
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Projector_SourceVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((ProjectorCommand.Type)value)
            {
                case ProjectorCommand.Type.Source:
                    return Visibility.Visible;
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Projector_IsFullscreenVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
