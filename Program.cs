﻿using Entities;

//The path is determined from where the program run. May be any folder.
string path = Directory.GetCurrentDirectory();
string? targetPath;
bool targetPathExist;
if (File.Exists(path + @"\targetPath.txt"))
{
    targetPath = File.ReadLines(path + @"\targetPath.txt").First();
}
else
{
    targetPath = null;
}
if (Directory.Exists(targetPath))
{
    targetPathExist = true;
}
else
{
    targetPathExist = false;
}

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

    //Renomeados is created if any .ret file was found
    if (files.Any())
    {
        Directory.CreateDirectory(path + @"\Renomeados");
    }
    else
    {
        Console.WriteLine("Não foi encontrado nenhum arquivo de retorno. A pasta Renomeados não foi criada.");
    }

    int nLinhas; // number of lines
    int nFiles = 0; // number of files
    int vazios = 0; // number of empty files, aka files with 2 lines or less
    int naoVazios = 0; // number of no empty files, aka files with more than 2

    foreach (string file in files)
    {
        string linha = File.ReadLines(file).First();
        if (linha.Substring(0, 19) == "02RETORNO01COBRANCA") // to get sure it's the right .ret file
        {
            nFiles++;
            nLinhas = 0;

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
            Arquivos arq = new Arquivos(conta, data, file, nLinhas);

            // Here the files are renamed and copied
            if (nLinhas > 2)
            {
                Console.WriteLine("Renomeando " + Path.GetFileName(file) + " para " + arq.Renomear() + "...");
                File.Copy(file, path + @"\Renomeados\" + arq.Renomear() + ".ret");
                naoVazios++;
                if (targetPathExist == true && !File.Exists(targetPath + "\\" + arq.Renomear() + ".ret"))
                {
                    File.Copy(file, targetPath + "\\" + arq.Renomear() + ".ret");
                }
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

    }
    Console.ReadLine();

    if (nFiles > 0)
    {
        Console.WriteLine("Foram encontrados " + nFiles + " arquivos de retorno ao todo.");
        Console.WriteLine("Destes, " + vazios + " estavam vazios e foram colocados na pasta \\Renomeados\\Vazios");
        Console.WriteLine("Os outros " + naoVazios + " restantes estão em \\Renomeados");
        if (targetPathExist == true)
        {
            Console.WriteLine("Os arquivos também foram copiados para a pasta " + targetPath);
        }
        else
        {
            Console.WriteLine("O arquivo targetPath.txt informando o caminho para salvar os arquivos não foi disponibilizado" +
                " ou o caminho " + targetPath + " não existe. Favor verificar.");
        }
    }
}
catch (IOException e)
{
    Console.WriteLine("An error occurred");
    Console.WriteLine(e.Message);
}
Console.ReadLine();