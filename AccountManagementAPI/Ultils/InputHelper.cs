using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagementAPI.Ultils
{
    public static class InputHelper
    {
        //dùng để nhập chuỗi, nếu để trống thì nhập lại
        public static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(s)) 
                    return s.Trim();
                Console.WriteLine("Giá trị không được để trống. Thử lại.");
            }
        }

        //dùng để nhập số dương, nếu không phải số hoặc số <= 0 thì nhập lại
        public static double ReadPositiveDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out var v) && v > 0) 
                    return v;
                Console.WriteLine("Vui lòng nhập số > 0.");
            }
        }

        //nhập lựa chọn trong menu
        public static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out var v) && v >= min && v <= max) 
                    return v;
                Console.WriteLine($"Vui lòng nhập số nguyên trong khoảng [{min}..{max}].");
            }
        }
    }
}
