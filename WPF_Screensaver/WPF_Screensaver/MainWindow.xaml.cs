using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WPF_Screensaver
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        private MediaPlayer player = new MediaPlayer();
        double inc = 0.1;
        double p = 0;
        Random rnd = new Random(); // отвечает за генерацию случайных чисел.
        Label somtex; // объект класса Label
        double wsizex, wsizey = 0;
        List<Image> balls = new List<Image>(); // list for cubes

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new System.Windows.Threading.DispatcherTimer(); //создаем метод движения
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 015);
            timer.Start();

            player.Open(new Uri(@"../../Resources/song.mp3", UriKind.RelativeOrAbsolute)); // добавляем музыку в программу
            player.Volume = 70;
            player.Play();

            somtex = new Label(); // Создаем новое текстовое поле
            somtex.Content = "";
            somtex.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 90)); // Устанавливается цвет
            somtex.FontSize = 48; // Размер шрифта
            cns.Children.Add(somtex); // добавляем на холст
            wsizex = ActualWidth;
            wsizey = ActualHeight;
            Canvas.SetRight(somtex, wsizex*0.42); // координаты надписи на холсте
            Canvas.SetTop(somtex, wsizey*0.05);

            

            for (int c = 0; c < 35; c++) // Создаем 2D обьекты
            {
                Image image = new Image(); // конструктор класса
                image.Source = new BitmapImage(new Uri(@"../../Resources/imcube.png", UriKind.RelativeOrAbsolute));
                image.Height = 10; // высота картинки

                balls.Add(image); // положили в список
                cns.Children.Add(image); // добавляем на холст
                Canvas.SetTop(image, (double)rnd.Next(0, (int)wsizey)); // Устанавливаем положение картинки
                Canvas.SetLeft(image, (double)rnd.Next(0, (int)wsizex*2) - wsizex);
            }


        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            double z = 7;
            double x = z * Math.Sin(p); //Параметрическое уравнение окружности
            double y = z * Math.Cos(p);

            rotate1.Angle += 1;
            Translate.OffsetX = x; // Имя преобразования
            Translate.OffsetY = y; // Установка его свойства
            double xf, yf;
            p = p + 0.01;

            foreach (Image im in balls) // пробегаем все изображения в списке 
            {
                xf = Canvas.GetLeft(im); // Получаем координаты кубика
                yf = Canvas.GetTop(im);
                if (yf>wsizey) // Если кубик за экраном то перезадаем его координаты
                {
                    yf = 0;
                    xf = rnd.Next(0, (int)wsizex * 2) - wsizex;
                }

                Canvas.SetLeft(im, xf + 1); // Устанавливаются координаты кубика на 1 больше
                Canvas.SetTop(im, yf + 1);
            }


            int r = (int)p;
            if (r%2==0) // Меняем текст надписи в зависимотси от времени
            {
                somtex.Content = "You are in";
            }else
            {
                somtex.Content = "Screensaver mode";
            }

            xf = Canvas.GetLeft(somtex); // Получаем координаты надписи
            yf = Canvas.GetTop(somtex);

            Canvas.SetLeft(somtex, xf + 0); // Устанавливаем координаты обратно
            Canvas.SetTop(somtex, yf + 0);

            Light.Color = Color.FromArgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), 255); // меняем цвет подцветки случайным образом.
        }
    }
}
