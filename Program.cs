using Entities;

//The path is determined from where the program run. May be any folder.
string path = Directory.GetCurrentDirectory();
Console.WriteLine("Acessando pasta " + path);
Console.ReadLine();
Console.WriteLine();

//The Renomeados folder is always deleted beforehand
if (Directory.Exists(path + @"\Renomeados"))
{
    Directory.Delete(path + @"\Renomeados", true);
}

try
{
    IEnumerable<string> files = Directory.EnumerateFiles(path, "*.ret", SearchOption.TopDirectoryOnly);

    //Renomeados is always created, even if there's no .ret file.
    Directory.CreateDirectory(path + @"\Renomeados");

    int nLinhas; // number of lines
    int nFiles = 0; // number of files
    int vazios = 0; // number of empty files, aka files with 2 lines or less
    int naoVazios = 0; // number of no empty files, aka files with more than 2

    foreach (string file in files)
    {
        nFiles++;
        nLinhas = 0;
        string linha = File.ReadLines(file).First();
        // the number of the account and the date are present on the first line, on these positions.
        //if there's future variation of these locations I will probably rely in other things than exact position.
        int conta = int.Parse(linha.Substring(33, 6));
        
        DateOnly data = DateOnly.Parse(linha.Substring(94, 2) + "/" + linha.Substring(96, 2) + "/"
            + linha.Substring(98, 2));
        
        IEnumerable<string> lines = File.ReadLines(file);
        foreach (string line in lines)
        {
            nLinhas++;
        }
        // there's even a class for files (Arquivos). Check it out!
        Arquivos arq = new Arquivos (conta, data, file, nLinhas);

        // Here the files are renamed and copied
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