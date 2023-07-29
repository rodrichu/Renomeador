using Entities;

string path = Directory.GetCurrentDirectory();
Console.WriteLine("Acessando pasta " + path);
Console.ReadLine();
Console.WriteLine();

if (Directory.Exists(path + @"\Renomeados"))
{
    Directory.Delete(path + @"\Renomeados", true);
}

try
{
    IEnumerable<string> files = Directory.EnumerateFiles(path, "*.ret", SearchOption.TopDirectoryOnly);

    Directory.CreateDirectory(path + @"\Renomeados");

    int nLinhas;
    int nFiles = 0;
    int vazios = 0;
    int naoVazios = 0;
    
    foreach (string file in files)
    {
        nFiles++;
        nLinhas = 0;
        string linha = File.ReadLines(file).First();
        
        int conta = int.Parse(linha.Substring(33, 6));
        
        DateOnly data = DateOnly.Parse(linha.Substring(94, 2) + "/" + linha.Substring(96, 2) + "/"
            + linha.Substring(98, 2));
        
        IEnumerable<string> lines = File.ReadLines(file);
        foreach (string line in lines)
        {
            nLinhas++;
        }

        Arquivos arq = new Arquivos (conta, data, file, nLinhas);

        if (nLinhas > 2)
        {
            Console.WriteLine("Renomeando " + Path.GetFileName(file) + " para " + arq.Renomear() +"...");
            File.Copy(file, path + @"\Renomeados\" + arq.Renomear() + ".ret");
            naoVazios++;
        }
        else
        {
            Console.Write("Renomeando " + Path.GetFileName(file) + " para " + arq.Renomear() + "... ");
            Console.WriteLine("Este arquivo está vazio.");
            Directory.CreateDirectory(path + @"\Renomeados\Vazios");
            File.Copy(file, path + @"\Renomeados\Vazios\" + arq.Renomear() + ".ret");
            vazios++;
        }
        Console.WriteLine();
    }
    Console.ReadLine();
    Console.WriteLine("Foram encontrados " + nFiles + " arquivos de retorno ao todo.");
    Console.WriteLine("Destes, " + vazios + " estavam vazios e foram colocados na pasta \\Renomeados\\Vazios");
    Console.WriteLine("Os outros " + naoVazios + " restantes estão em \\Renomeados");
}
catch (IOException e)
{
    Console.WriteLine("An error occurred");
    Console.WriteLine(e.Message);
}
Console.ReadLine();