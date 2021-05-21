using Neuro.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Neuro.Models.Interfaces;
using Neuro.Models;
using System.Threading.Tasks;
using Neuro.Views;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace Neuro.ViewModels
{
    class MainViewModel : Observable
    {
        private IPerceptron perceptron;
        private ITeacher teacher;

        private int width = 16;
        private int height = 16;
        private List<int> layerSizes;
        private string layerSizesString;
        private int eraCount = 50;
        private int batchSize = 1000;
        private bool useBatch = false;
        private string teachFolderPath = @"C:\Desktop\Университет\8 семестр\Программирование портативных и смарт устройств\Dataset\Обучающая выборка";
        private int precision = 95;
        private string testFolderPath = @"C:\Desktop\Университет\8 семестр\Программирование портативных и смарт устройств\Dataset\Тестовая выборка";
        private string imagePath;
        private string pattern = @"[^_]+$";
        private bool isWindowEnabled = true;
        private System.Windows.Visibility showStatusBar = System.Windows.Visibility.Collapsed;
        private string resultString;
        private string teachingTitle;
        private string teachingStatus;
        private int teachingPercentages;

        public MainViewModel()
        {
            LayerSizes = new List<int>() { 100, 6 };
            CreatePerceptronWithTeacher();
        }

        private void CreatePerceptronWithTeacher()
        {
            try
            {
                perceptron = new Perceptron(width * height, layerSizes);
                teacher = new Teacher(perceptron);
            }
            catch (Exception ex)
            {
                MessageBox.Show("В ходе выполнения программы произошла ошибка.\n" + ex.Message);
            }
        }

        /// <summary>
        /// Если строка value содержит число большее нуля и не превосходящее Int32.maxValue, то это число будет сконвертировано и возвращено.
        /// Если строка value содержит число большее нуля и большее Int32.maxValue, то будет возвращено Int32.MaxValue.
        /// Если строка value пуста или равна null, то будет возвращена единица.
        /// Во всех остальных случаях будет возвращён ноль.
        /// </summary>
        private int StringToPositiveInt(string value, int? maxValue = null)
        {
            if (new Regex(@"^[1-9]+[0-9]*$").IsMatch(value))
            {
                int number = int.TryParse(value, out int tmp) ? tmp : int.MaxValue;

                if (maxValue != null && maxValue < number)
                {
                    number = (int)maxValue;
                }

                return number;
            }

            if (string.IsNullOrEmpty(value))
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Если строку value можно конвертировать в целое число большее нуля,
        /// то значение полученного числа будет установлено в переменную storage.
        /// </summary>
        private void SetPositiveInt(ref int storage, string value, int? maxValue = null)
        {
            int number = StringToPositiveInt(value, maxValue);

            if (number > 0)
            {
                Set(ref storage, number);
            }
        }

        public int Width
        {
            get => width;
            set
            {
                if (!Equals(width, value))
                {
                    width = value;

                    OnPropertyChanged(nameof(Width));
                    OnPropertyChanged(nameof(WidthString));

                    CreatePerceptronWithTeacher();
                }
            }
        }

        public string WidthString
        {
            get => width.ToString();
            set
            {
                int number = StringToPositiveInt(value);

                if (number > 0 && !Equals(width, number))
                {
                    width = number;

                    OnPropertyChanged(nameof(Width));
                    OnPropertyChanged(nameof(WidthString));

                    CreatePerceptronWithTeacher();
                }
            }
        }

        public int Height
        {
            get => height;
            set
            {
                if (!Equals(height, value))
                {
                    height = value;

                    OnPropertyChanged(nameof(Height));
                    OnPropertyChanged(nameof(HeightString));

                    CreatePerceptronWithTeacher();
                }
            }
        }

        public string HeightString
        {
            get => height.ToString();
            set
            {
                int number = StringToPositiveInt(value);

                if (number > 0 && !Equals(height, number))
                {
                    height = number;

                    OnPropertyChanged(nameof(Height));
                    OnPropertyChanged(nameof(HeightString));

                    CreatePerceptronWithTeacher();
                }
            }
        }

        public List<int> LayerSizes
        {
            get => layerSizes;
            set
            {
                layerSizes = value;
                layerSizesString = "";

                for (int i = 0; i < layerSizes.Count; i++)
                {
                    layerSizesString += layerSizes[i].ToString();

                    if (i != layerSizes.Count - 1)
                    {
                        layerSizesString += ", ";
                    }
                }

                OnPropertyChanged(nameof(LayerSizes));
                OnPropertyChanged(nameof(LayerSizesString));

                CreatePerceptronWithTeacher();
            }
        }

        public string LayerSizesString
        {
            get => layerSizesString;
            set
            {
                Match match = new Regex(@"^([1-9]+\d*)+\s*(,\s*([1-9]+\d*)+\s*)*,?\s*$").Match(value);

                if (match.Success)
                {
                    layerSizesString = match.Value;
                    layerSizes = new List<int>();

                    foreach (string item in match.Value.Replace(" ", "").TrimEnd(',').Split(','))
                    {
                        int number = int.TryParse(item, out int tmp) ? tmp : int.MaxValue;
                        layerSizes.Add(number);
                    }

                    OnPropertyChanged(nameof(LayerSizes));
                    OnPropertyChanged(nameof(LayerSizesString));

                    CreatePerceptronWithTeacher();
                }
            }
        }

        public string EraCountString
        {
            get => eraCount.ToString();
            set => SetPositiveInt(ref eraCount, value);
        }

        public string BatchSizeString
        {
            get => batchSize.ToString();
            set => SetPositiveInt(ref batchSize, value);
        }

        public bool UseBatch
        {
            get => useBatch;
            set
            {
                if (!Equals(useBatch, value))
                {
                    useBatch = value;

                    OnPropertyChanged(nameof(UseBatch));
                    OnPropertyChanged(nameof(BatchCheckBoxValue));
                }
            }
        }

        public bool BatchCheckBoxValue
        {
            get => !useBatch;
            set
            {
                if (!Equals(!useBatch, value))
                {
                    useBatch = !value;

                    OnPropertyChanged(nameof(UseBatch));
                    OnPropertyChanged(nameof(BatchCheckBoxValue));
                }
            }
        }

        public string TeachFolderPath
        {
            get => teachFolderPath;
            set => Set(ref teachFolderPath, value);
        }

        public string PrecisionString
        {
            get => precision.ToString();
            set => SetPositiveInt(ref precision, value, 100);
        }

        public string TestFolderPath
        {
            get => testFolderPath;
            set => Set(ref testFolderPath, value);
        }

        public string Pattern
        {
            get => pattern;
            set => Set(ref pattern, value);
        }

        public string ImagePath
        {
            get => imagePath;
            set
            {
                Set(ref imagePath, value);
                
                if (!Equals(resultString, null))
                {
                    resultString = null;
                    OnPropertyChanged(nameof(ResultString));
                }
            }
        }

        public bool IsWindowEnabled
        {
            get => isWindowEnabled;
            set => Set(ref isWindowEnabled, value);
        }

        public System.Windows.Visibility ShowStatusBar
        {
            get => showStatusBar;
            set => Set(ref showStatusBar, value);
        }

        public string ResultString
        {
            get => resultString;
            set => Set(ref resultString, value);
        }

        public string TeachingTitle
        {
            get => teachingTitle;
            set => Set(ref teachingTitle, value);
        }

        public string TeachingStatus
        {
            get => teachingStatus;
            set => Set(ref teachingStatus, value);
        }

        public int TeachingPercentages
        {
            get => teachingPercentages;
            set => Set(ref teachingPercentages, value);
        }

        private ICommand openTeachFolderCommand;
        private ICommand openTestFolderCommand;
        private ICommand specialCommand;
        private ICommand teachCommand;
        private ICommand saveCommand;
        private ICommand loadCommand;
        private ICommand clearCommand;
        private ICommand plotCommand;
        private ICommand openImageCommand;
        private ICommand recognizeCommand;

        public ICommand OpenTeachFolderCommand => openTeachFolderCommand ??= new RelayCommand(() =>
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    TeachFolderPath = fbd.SelectedPath;
                }
            }
         });

        public ICommand OpenTestFolderCommand => openTestFolderCommand ??= new RelayCommand(() =>
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    TestFolderPath = fbd.SelectedPath;
                }
            }
        });

        /// <summary> Загрузки изображений из датасета. </summary>
        /// <param name="folderPath"> Путь к датасету. </param>
        /// <param name="pattern">
        /// Регулярное выражение для выделения индекса
        /// класса из имени файла с изображением.
        /// </param>
        private static ImageData[] LoadImages(string folderPath, string pattern, int width, int height)
        {
            string[] filenames = Directory.GetFiles(folderPath);
            List<ImageData> images = new List<ImageData>();
            Regex regex = new Regex(pattern);

            Parallel.ForEach(filenames, filename => images.Add(new ImageData
            {
                Data = new Bitmap(filename).Resize(width, height).GetBrightnessArray(),
                Class = int.Parse(regex.Match(Path.GetFileNameWithoutExtension(filename)).Value)
            }));

            return images.ToArray();
        }

        public static (ImageData[] TeachImages, ImageData[] TestImages) GetSplittedDataset(string inputPath, double fraction = 0.2)
        {
            var valuesByLine = (from line in File.ReadLines(inputPath)
                                select line.Split(',')).ToArray();

            GC.Collect();

            var teachCount = (from values in valuesByLine
                              group values by values[0] into g
                              select new { Label = g.Key, TeachCount = g.Count() - (int)(g.Count() * fraction) })
                              .ToDictionary(tc => tc.Label, tc => tc.TeachCount);

            var testCount = (from values in valuesByLine
                             group values by values[0] into g
                             select new { Label = g.Key, TestCount = (int)(g.Count() * fraction) })
                             .ToDictionary(tc => tc.Label, tc => tc.TestCount);

            GC.Collect();

            valuesByLine.Shuffle();
            Random rand = new Random();
            List<ImageData> teachDataset = new List<ImageData>();
            List<ImageData> testDataset = new List<ImageData>();

            foreach (var values in valuesByLine)
            {
                if (teachCount[values[0]] != 0)
                {
                    teachCount[values[0]]--;
                    int label = int.Parse(values[0]);
                    float[] data = new float[28 * 28];

                    for (int i = 0; i < 28 * 28; i++)
                    {
                        data[i] = float.Parse(values[i + 1]) / 255;
                    }

                    teachDataset.Add(new ImageData
                    {
                        Data = data,
                        Class = label
                    });
                }
                else if (testCount[values[0]] != 0)
                {
                    testCount[values[0]]--;
                    int label = int.Parse(values[0]);
                    float[] data = new float[28 * 28];

                    for (int i = 0; i < 28 * 28; i++)
                    {
                        data[i] = float.Parse(values[i + 1]) / 255;
                    }

                    testDataset.Add(new ImageData
                    {
                        Data = data,
                        Class = label
                    });
                }
            }

            return (teachDataset.ToArray(), testDataset.ToArray());
        }

        public ICommand SpecialCommand => specialCommand ??= new RelayCommand(() =>
        {
            IsWindowEnabled = false;
            ShowStatusBar = System.Windows.Visibility.Visible;

            Task.Run(() =>
            {
                try
                {
                    Width = 28;
                    Height = 28;
                    LayerSizes = new List<int>() { 300, 62 };
                    UseBatch = false;
                    EraCountString = $"{30}";

                    TeachingTitle = "Загрузка обучающей и тестовой выборок в память.";
                    string csvPath = @"D:\Users\Nikita\source\repos\Datasets\cyrillic\russian\HRCC balanced.csv";
                    (ImageData[] teachImages, ImageData[] testImages) = GetSplittedDataset(csvPath);
                    uint? batch = null;
                    if (useBatch) batch = (uint)batchSize;
                    int teachLength = useBatch ? batchSize : teachImages.Length;
                    TeachingStatus = $"0/{teachLength}    |    0/{eraCount}";
                    TeachingTitle = "Обучение методом обратного распространения ошибки.";

                    foreach (var tuple in teacher.Teach(teachImages, testImages, eraCount, batch))
                    {
                        TeachingPercentages = (int)(tuple.Item2 * 100.0 / eraCount);
                        TeachingStatus = $"{tuple.Item1}/{teachLength}    |    {tuple.Item2}/{eraCount}";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("В ходе выполнения программы произошла ошибка.\n" + ex.Message);
                }

                IsWindowEnabled = true;
                ShowStatusBar = System.Windows.Visibility.Collapsed;
                TeachingStatus = "";
            });
        });

        public ICommand TeachCommand => teachCommand ??= new RelayCommand(() =>
        {
            IsWindowEnabled = false;
            ShowStatusBar = System.Windows.Visibility.Visible;

            Task.Run(() =>
            {
                try
                {
                    TeachingTitle = "Загрузка обучающей выборки в память.";
                    ImageData[] teachImages = LoadImages(teachFolderPath, pattern, width, height);
                    uint? batch = null;
                    if (useBatch) batch = (uint)batchSize;
                    int teachLength = useBatch ? batchSize : teachImages.Length;

                    if (testFolderPath == "")
                    {
                        TeachingStatus = $"0/{teachLength}    |    0/{eraCount}";
                        TeachingTitle = "Обучение методом обратного распространения ошибки.";

                        foreach (var tuple in teacher.Teach(teachImages, eraCount, batch))
                        {
                            TeachingPercentages = (int)(tuple.Item2 * 100.0 / eraCount);
                            TeachingStatus = $"{tuple.Item1}/{teachLength}    |    {tuple.Item2}/{eraCount}";
                        }
                    }
                    else
                    {
                        TeachingTitle = "Загрузка тестовой выборки в память.";
                        ImageData[] testImages = LoadImages(testFolderPath, pattern, width, height);
                        TeachingStatus = $"0/{teachLength}    |    0/{eraCount}";
                        TeachingTitle = "Обучение методом обратного распространения ошибки.";

                        foreach (var tuple in teacher.Teach(teachImages, testImages, eraCount, batch))
                        {
                            TeachingPercentages = (int)(tuple.Item2 * 100.0 / eraCount);
                            TeachingStatus = $"{tuple.Item1}/{teachLength}    |    {tuple.Item2}/{eraCount}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("В ходе выполнения программы произошла ошибка.\n" + ex.Message);
                }

                IsWindowEnabled = true;
                ShowStatusBar = System.Windows.Visibility.Collapsed;
                TeachingStatus = "";
            });
        });

        public ICommand SaveCommand => saveCommand ??= new RelayCommand(() =>
        {
            try
            {
                using (var sfd = new SaveFileDialog { Filter = "База данных|*.IIDB" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var fs = File.Create(sfd.FileName))
                        {
                            var bf = new BinaryFormatter();
                            bf.Serialize(fs, new SaveStructure
                            {
                                Width = width,
                                Height = height,
                                Perceptron = perceptron
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("В ходе выполнения программы произошла ошибка.\n" + ex.Message);
            }
        });

        public ICommand LoadCommand => loadCommand ??= new RelayCommand(() =>
        {
            try
            {
                using (var ofd = new OpenFileDialog { Filter = "База данных|*.IIDB" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (var fs = File.OpenRead(ofd.FileName))
                        {
                            var bf = new BinaryFormatter();
                            var tmp = (SaveStructure)bf.Deserialize(fs);
                            Width = tmp.Width;
                            Height = tmp.Height;
                            List<int> sizes = new List<int>();
                            foreach(ILayer layer in tmp.Perceptron.Layers)
                            {
                                sizes.Add(layer.Size);
                            }
                            LayerSizes = sizes;
                            perceptron = tmp.Perceptron;
                            teacher = new Teacher(perceptron);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("В ходе выполнения программы произошла ошибка.\n" + ex.Message);
            }
        });

        public ICommand ClearCommand => clearCommand ??= new RelayCommand(() =>
        {
            CreatePerceptronWithTeacher();
        });

        public ICommand PlotCommand => plotCommand ??= new RelayCommand(() =>
        {
            try
            {
                var plot = new PlotView();
                plot.DataContext = new PlotViewModel();
                ((PlotViewModel)plot.DataContext).Precision = teacher.PrecisionLog;
                plot.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("В ходе выполнения программы произошла ошибка.\n" + ex.Message);
            }
        });

        public ICommand OpenImageCommand => openImageCommand ??= new RelayCommand(() =>
        {
            using (var fd = new OpenFileDialog { Filter = "PNG|*.png|JPG|*.jpg|BMP|*.bmp|GIF|*.gif|Все файлы|*.*" })
            {
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    ImagePath = fd.FileName;
                }
            }
        });

        public ICommand RecognizeCommand => recognizeCommand ??= new RelayCommand(() =>
        {
            IsWindowEnabled = false;

            Task.Run(() => {
                try
                {
                    IList<float> output = perceptron.Recognize(new Bitmap(imagePath).Resize(width, height).GetBrightnessArray());
                    string result = "Класс ";
                    bool isItFirstClass = true;
                    float eps = 0.1f;

                    for (int i = 0; i < output.Count; i++)
                    {
                        if (output[i] + eps > 1)
                        {
                            if (isItFirstClass)
                            {
                                isItFirstClass = false;
                                result = result + i.ToString();
                            }
                            else
                            {
                                result = result + ", " + i.ToString();
                            }
                        }
                    }

                    if (result == "Класс ")
                    {
                        result = result + "не распознан";
                    }

                    ResultString = result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("В ходе выполнения программы произошла ошибка.\n" + ex.Message);
                }

                IsWindowEnabled = true;
            });
        });
    }
}
