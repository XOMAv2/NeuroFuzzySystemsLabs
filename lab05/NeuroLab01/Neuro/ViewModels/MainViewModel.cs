using Neuro.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Neuro.Models.Interaces;
using Neuro.Models;
using System.Threading.Tasks;
using Neuro.Views;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Neuro.ViewModels
{
    class MainViewModel : Observable
    {
        private IPerceptron perceptron;
        private ITeacher teacher;

        private int width = 128;
        private int height = 128;
        private int aCount = 1000;
        private int rCount = 6;
        private int eraCount = 35;
        private string teachFolderPath = @"C:\Desktop\Университет\8 семестр\Программирование портативных и смарт устройств\Dataset\Обучающая выборка";
        private int precision = 95;
        private string testFolderPath = @"C:\Desktop\Университет\8 семестр\Программирование портативных и смарт устройств\Dataset\Тестовая выборка";
        private string imagePath;
        private string pattern = @"[^_]+$";
        private bool isWindowEnabled = true;
        private string resultString;

        public MainViewModel()
        {
            CreatePerceptronWithTeacher();
        }

        private void CreatePerceptronWithTeacher()
        {
            try
            {
                perceptron = new Perceptron(width * height, aCount, rCount);
                teacher = new Teacher(perceptron);
            }
            catch (Exception ex)
            {
                MessageBox.Show("В ходе выполнения программы роизошла ошибка.\n" + ex.Message);
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

        public string ACountString
        {
            get => aCount.ToString();
            set
            {
                int number = StringToPositiveInt(value);

                if (number > 0 && !Equals(aCount, number))
                {
                    aCount = number;

                    OnPropertyChanged(nameof(ACountString));

                    CreatePerceptronWithTeacher();
                }
            }
        }

        public string RCountString
        {
            get => rCount.ToString();
            set
            {
                int number = StringToPositiveInt(value);

                if (number > 0 && !Equals(rCount, number))
                {
                    rCount = number;

                    OnPropertyChanged(nameof(RCountString));

                    CreatePerceptronWithTeacher();
                }
            }
        }

        public string EraCountString
        {
            get => eraCount.ToString();
            set => SetPositiveInt(ref eraCount, value);
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

        public string ResultString
        {
            get => resultString;
            set => Set(ref resultString, value);
        }

        private ICommand openTeachFolderCommand;
        private ICommand openTestFolderCommand;
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

        public ICommand TeachCommand => teachCommand ??= new RelayCommand(async () =>
        {
            IsWindowEnabled = false;

            await Task.Run(() => {
                try
                {
                    if (testFolderPath == "")
                    {
                        teacher.Teach
                        (
                            ImageProcessing.LoadImages(teachFolderPath, pattern),
                            eraCount
                        );
                    }
                    else
                    {
                        teacher.Teach
                        (
                            ImageProcessing.LoadImages(teachFolderPath, pattern),
                            ImageProcessing.LoadImages(testFolderPath, pattern),
                            eraCount
                        );
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("В ходе выполнения программы роизошла ошибка.\n" + ex.Message);
                }

                IsWindowEnabled = true;
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
                                ACount = aCount,
                                RCount = rCount,
                                Perceptron = perceptron
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("В ходе выполнения программы роизошла ошибка.\n" + ex.Message);
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
                            ACountString = tmp.ACount.ToString();
                            RCountString = tmp.RCount.ToString();
                            perceptron = tmp.Perceptron;
                            teacher = new Teacher(perceptron);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("В ходе выполнения программы роизошла ошибка.\n" + ex.Message);
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
                MessageBox.Show("В ходе выполнения программы роизошла ошибка.\n" + ex.Message);
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

        public ICommand RecognizeCommand => recognizeCommand ??= new RelayCommand(async () =>
        {
            IsWindowEnabled = false;

            await Task.Run(() => {
                try
                {
                    IList<byte> output = perceptron.Recognize(ImageProcessing.ImageToSignalVector(new Bitmap(imagePath)));
                    string result = "Класс ";
                    bool isItFirstClass = true;

                    for (int i = 0; i < output.Count; i++)
                    {
                        if (output[i] != 0)
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
                    MessageBox.Show("В ходе выполнения программы роизошла ошибка.\n" + ex.Message);
                }

                IsWindowEnabled = true;
            });
        });
    }
}
