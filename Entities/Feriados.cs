namespace Entities
{
    class Feriados
    {
        public static DateOnly Easter(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = (b - (b / 4) - ((8 * b + 13) / 25) + (19 * a) + 15) % 30;
            int d = c - (c / 28) * (1 - (c / 28) * (29 / (c + 1)) * ((21 - a) / 11));
            int e = d - ((year + (year / 4) + d + 2 - b + (b / 4)) % 7);
            int month = 3 + ((e + 40) / 44);
            int day = e + 28 - (31 * (month / 4));
            return new DateOnly(year, month, day);
        }
        public static DateOnly PaixaoDeCristo(int year)
        {
            return Easter(year).AddDays(-2);
        }
        public static DateOnly Carnaval(int year)
        {
            return Easter(year).AddDays(-47);
        }
        public static DateOnly CorpusChristi(int year)
        {
            return Easter(year).AddDays(60);
        }

        private static string[] feriados = new string[] { "0101", "2104", "0105", "0709", "1210", "0211", "1511", "2512" };
        public static bool Feriado (DateOnly data)
        {
            string paixaoDeCristo = PaixaoDeCristo(data.Year).ToString("ddMM");
            string carnaval = Carnaval(data.Year).ToString("ddMM");
            string corpusChristi = CorpusChristi(data.Year).ToString("ddMM");

            List<string> list = feriados.ToList<string>();
            list.Add(paixaoDeCristo);
            list.Add(carnaval);
            list.Add(corpusChristi);

            return list.Contains(data.ToString("ddMM"));
        }
    }
}
