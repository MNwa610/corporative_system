using System;

class Program
{
    static void Main()
    {
        // Ввод данных от пользователя
        Console.Write("Введите количество модулей (n): ");
        int n = int.Parse(Console.ReadLine());
        
        // Ввод размеров модуля (ширина и длина)
        Console.Write("Введите размеры модуля (a b): ");
        string[] input = Console.ReadLine().Split();
        int a = int.Parse(input[0]);  // Ширина модуля
        int b = int.Parse(input[1]);  // Длина модуля
        
        // Ввод размеров поля для размещения модулей
        Console.Write("Введите размеры поля (w h): ");
        input = Console.ReadLine().Split();
        int w = int.Parse(input[0]);  // Ширина поля
        int h = int.Parse(input[1]);  // Высота поля

        // Вычисление максимальной толщины защиты с помощью основного алгоритма
        int maxD = CalculateMaxProtection(n, a, b, w, h);
        
        // Вывод форматированных результатов расчета
        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine("РЕЗУЛЬТАТЫ РАСЧЕТА");
        Console.WriteLine(new string('=', 40));
        
        // Проверка и вывод результата
        if (maxD >= 0)
        {
            // Если найдена допустимая толщина защиты
            Console.WriteLine($"Максимальная толщина защиты: {maxD} м");
            
            // Показываем оба варианта ориентации модуля для найденной толщины
            CheckAndPrintOrientation(n, a, b, w, h, maxD, "Ориентация A×B");  // Оригинальная ориентация
            CheckAndPrintOrientation(n, b, a, w, h, maxD, "Ориентация B×A");  // Повернутая ориентация
        }
        else
        {
            // Если размещение невозможно ни с какой толщиной защиты
            Console.WriteLine("Невозможно разместить модули на поле");
        }
    }

    // Основной метод для расчета максимальной толщины защиты
    static int CalculateMaxProtection(int n, int a, int b, int w, int h)
    {
        int maxD = -1;  // Начальное значение - защита невозможна
        
        // Перебираем возможные толщины защиты от 0 до минимального размера поля
        for (int d = 0; d <= Math.Min(w, h); d++)
        {
            // Проверяем обе возможные ориентации модуля:
            // ориентация a×b (модуль расположен как есть)
            bool orientation1 = CanPlaceModules(n, a, b, w, h, d);
            // ориентация b×a (модуль повернут на 90 градусов)
            bool orientation2 = CanPlaceModules(n, b, a, w, h, d);
            
            // Если хотя бы одна ориентация позволяет разместить модули
            if (orientation1 || orientation2)
            {
                maxD = d;  // Запоминаем текущую толщину как допустимую
            }
            else
            {
                // Если для текущей толщины размещение невозможно, 
                // то и для больших толщин тоже невозможно (выходим из цикла)
                break;
            }
        }
        
        return maxD;  // Возвращаем найденную максимальную толщину
    }

    // Метод проверки возможности размещения модулей с заданной толщиной защиты
    static bool CanPlaceModules(int n, int moduleWidth, int moduleHeight, int fieldWidth, int fieldHeight, int d)
    {
        // Вычисляем размеры модуля с учетом защиты:
        // с каждой стороны добавляется толщина защиты d
        int protectedWidth = moduleWidth + 2 * d;
        int protectedHeight = moduleHeight + 2 * d;

        // Проверяем, что модуль с защитой вообще помещается на поле
        if (protectedWidth > fieldWidth || protectedHeight > fieldHeight)
            return false;  // Модуль слишком большой для поля

        // Вычисляем максимальное количество модулей, которое можно разместить:
        // Количество модулей по ширине поля (целочисленное деление)
        int modulesInWidth = fieldWidth / protectedWidth;
        // Количество модулей по высоте поля (целочисленное деление)
        int modulesInHeight = fieldHeight / protectedHeight;
        // Общее количество модулей
        int totalModules = modulesInWidth * modulesInHeight;

        // Проверяем, что помещается не менее требуемого количества модулей
        return totalModules >= n;
    }

    // Вспомогательный метод для вывода информации о конкретной ориентации
    static void CheckAndPrintOrientation(int n, int a, int b, int w, int h, int d, string orientationName)
    {
        // Вычисляем размеры модуля с защитой для данной ориентации
        int protectedWidth = a + 2 * d;
        int protectedHeight = b + 2 * d;
        
        // Проверяем, что модуль помещается на поле при данной ориентации
        if (protectedWidth <= w && protectedHeight <= h)
        {
            // Вычисляем количество модулей для данной ориентации
            int modulesInWidth = w / protectedWidth;
            int modulesInHeight = h / protectedHeight;
            int totalModules = modulesInWidth * modulesInHeight;
            
            // Если количество модулей удовлетворяет требованию, выводим информацию
            if (totalModules >= n)
            {
                Console.WriteLine($"\n{orientationName}:");
                Console.WriteLine($"  Размер модуля с защитой: {protectedWidth} × {protectedHeight} м");
                Console.WriteLine($"  Компоновка: {modulesInWidth} × {modulesInHeight} = {totalModules} модулей");
            }
        }
    }
}