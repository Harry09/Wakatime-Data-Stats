using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

using Avalonia.Controls;

using WTStats.Core;
using WTStats.Core.Generators;

namespace WTStats.GUI.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		private string _dataFilePath;
		private string _outputDirectory = Directory.GetCurrentDirectory();

		public bool GenerateHTML { get; set; } = true;

		public bool GenerateTxt { get; set; }

		public string DataFilePath
		{
			get => _dataFilePath;
			set { _dataFilePath = value; OnPropertyChanged(); }
		}

		public string OutputDirectory
		{
			get => _outputDirectory;
			set { _outputDirectory = value; OnPropertyChanged(); }
		}

		public async void OnBrowseDataFile()
		{
			var dlg = new OpenFileDialog() { Title = "Open" };
			dlg.Filters.Add(new FileDialogFilter() { Name = "Data file", Extensions = { "json" } });

			var result = await dlg.ShowAsync();

			if (result is null)
				return;

			DataFilePath = result.FirstOrDefault();
		}

		public async void OnBrowseOutputDirectory()
		{
			var dlg = new OpenFolderDialog()
			{
				Title = "Save",
				DefaultDirectory = Directory.GetCurrentDirectory()
			};

			var result = await dlg.ShowAsync();

			if (result is null)
				return;

			OutputDirectory = result;
		}

		public void OnGenerate()
		{
			if (!File.Exists(DataFilePath))
				return;

			var mainGenerator = new MainGenerator(DataFilePath);

			if (GenerateTxt)
				mainGenerator.AddGenerator<TxtGenerator>();

			if (GenerateHTML)
				mainGenerator.AddGenerator<HTMLGenerator>();

			try
			{
				var datas = mainGenerator.Generate();

				if (datas is null)
					return;

				int i = 1;

				foreach (var data in datas)
				{
					string filePath = string.Empty;

					if (!string.IsNullOrWhiteSpace(OutputDirectory))
					{
						filePath = OutputDirectory + "/";
					}

					filePath += $"{data.DataName}.{data.FileExtension}";

					File.WriteAllText(filePath, data.Data);

					i++;
				}

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					Process.Start("explorer.exe", OutputDirectory.Replace(" / ", "\\"));
				}
			}
			catch (Exception ex)
			{
				Debug.Fail(ex.StackTrace);
			}
		}
	}
}
