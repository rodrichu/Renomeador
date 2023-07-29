namespace Entities
{
    internal class Arquivos
    {
        public int Conta { get; set; }
        public DateOnly Data { get; set; }
        public string Origem { get; set;}
        public int Linhas { get; set;}

        public Arquivos(int conta, DateOnly data, string origem, int linhas)
        {
            Conta = conta;
            Data = data.AddDays(1);
            Origem = origem;
            Linhas = linhas;

            string diaDaSemana = Data.DayOfWeek.ToString();

            while (diaDaSemana == "Saturday" || diaDaSemana == "Sunday" || Feriados.Feriado(Data) == true)
            {
                Data = Data.AddDays(1);
                diaDaSemana = Data.DayOfWeek.ToString();
            }
        }
    public string Renomear()
        {
            return "CBR" + Data.ToString("ddMMyyyy") + "_" + Conta;
        }
    }
}
