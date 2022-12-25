using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
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
using CalCul;
using CommunityToolkit.Mvvm.Input;
using static CalCul.Calculator;

namespace CalCulWPF
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainViewModel();
		}
	}

	public class MainViewModel : INotifyPropertyChanged
	{
		public string Screen
		{
			get => _screen;
			set
			{
				if (_screen == value) return;
				_screen = value;
				OnPropertyChanged();
			}
		}

		public string LastAction
		{
			get => _lastAction;
			set
			{
				if (_lastAction == value) return;
				_lastAction = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand<string> ButtonPressedCommand { get; }

		private string _screen = string.Empty;

		private string _lastAction = string.Empty;

		private Stack<decimal> mem;

		private const string memFileName = "mem.json";

		public MainViewModel()
		{
			try
			{
				Load();
			}
			catch
			{
				mem = new Stack<decimal>();
			}

			ButtonPressedCommand = new RelayCommand<string>((arg) =>
			{
				LastAction = arg;
				if (string.IsNullOrEmpty(Screen) ? false : char.IsLetter(Screen[0]))
					Screen = string.Empty;
				switch (arg)
				{
					case "=":
						Calculate();
						break;
					case "AC":
						Screen = string.Empty;
						break;
					case "MC":
						mem.Clear();
						Save();
						break;
					case "MS":
						try
						{
							mem.Push(decimal.Parse(Screen));
							Save();
							Screen = string.Empty;
						}
						catch
						{
							Screen = "Error in memory";
						}
						break;
					case "MR":
						if (mem.Count != 0)
							Screen = mem.Pop().ToString();
						break;
					case "DEL":
						if (Screen.Length> 0)
							Screen = Screen.Remove(Screen.Length - 1, 1);
						break;
					default:
						Screen += arg;
						break;
				}
			});
		}

		private void Calculate()
		{
			try
			{
				Screen = Compute(Screen).ToString();
			}
			catch
			{
				
				Screen = "Error in calculation";
			}
		}

		private void Load()
		{
			var json = File.ReadAllText(memFileName);
			mem = new Stack<decimal>(JsonSerializer.Deserialize<Stack<decimal>>(json)!);
		}

		private void Save()
		{
			var json = JsonSerializer.Serialize(mem);
			File.WriteAllText(memFileName, json);
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
