#tool nuget:?package=NUnit.ConsoleRunner

// acessando parâmetros que foram passados para o script
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

string solution = "../../src/" + "Cake.DotNetConf.Addin/Cake.DotNetConf.Addin.sln";

/* 
	exemplo de um método utilizando c# no script 
*/ 
int Somar(int a, int b)
{
	return a + b;
}

/* 
	task simples estrutura básica, a descrição não é obrigatória,
	mas permite a listagem de tasks e suas descrições fornecendo
	um rápido esclarecimento das funcionalidades do script
	
	PS> .\build.ps1 --showdescription

	PS> .\build.ps1
*/
Task("Default")
    .Description("Exibe a mensagem de hello world")
    .Does(() => {

        Information("Hello World");

    }); 


/* 
	Dependeência entre tasks, no caso a chamada para 'Bye', executará 'Hello'

	PS> .\build.ps1 -Target hello
*/
Task("Hello")
    .Does(() => {

        Information("Hello dotnet conf");
        
    });

/* 
	Dependeência entre tasks, no caso a chamada para 'Bye', executará 'Hello'

	PS> .\build.ps1 -Target bye
*/
Task("Bye")
    .IsDependentOn("Hello")
    .Does(() => {

        Information("Bye dotnet conf");}
    ); 

/* 
	Tratamento de erros

	PS> .\build.ps1 -Target error
*/
Task("Error")
    .Does(() => {

        int valor = 100;
        int divisor = 0;

        int resultado = valor / divisor;
        
    })
    .OnError(exception =>
    {
        Error("Menssagem de exceção: " + exception.Message);
    });

/* 
	Tratamento de erros, com bloco finally onde o código será sempre executado

	PS> .\build.ps1 -Target finally
*/
Task("Finally")
    .Does(() => {

        int valor = 100;
        int divisor = 0;

        int resultado = valor / divisor;
        
    })
    .OnError(exception =>
    {
        Error("Menssagem de exceção: " + exception.Message);
    })
    .Finally(() =>
    {  
        Warning("Aviso: Exectando bloco finally");
    });

/* 
	excemplo restaurando pacotes nuge da solução

	PS> .\build.ps1 -Target nuget
*/
Task("Nuget")
    .Does(() => {

        NuGetRestore(solution, new NuGetRestoreSettings(){
            Source = new List<string> { 
                "https://api.nuget.org/v3/index.json", 
            }
        });
    });

/* 
	compilando a solução
*/
Task("Build")
    .IsDependentOn("Nuget")
    .Does(() => {

        MSBuild(solution, new MSBuildSettings()
            .WithProperty("Configuration", configuration));

    });

/* 
	rodando os testes da solução com NUnit
	* precisamos o tool do NUnit seja incluindo no pipeline de execução especificando ou não a versão
		#tool nuget:?package=NUnit.ConsoleRunner
        #tool nuget:?package=NUnit.ConsoleRunner&version=3.7.0

	PS> .\build.ps1 -Target tests
*/
Task("Tests")
    .IsDependentOn("Build")
    .Does(() => {

        var src = Directory("../../src");

        NUnit3(GetFiles(src.Path.FullPath + "/**/bin/" + configuration + "/*Tests.dll"));

    });

/* 
	Utilizandos Aliases criados para nosso script
	* rode um build, e adicione a ddl do addin, Cake.DotNetConf.Addin.dll, na pasta tools/cake/addin
	* inclua a referencia para o addin no inicio do script 
		#reference addins/Cake.DotNetConf.Addin.dll
	* descomente e rode a task addin

	PS> .\build.ps1 -Target addin
*/
Task("Addin")
    .Does(() => {

        //MandarUmAlo();

		//var resultado = Somar(1,1);

		//Information("O resultado da soma foi: " + resultado);
    });

/* 
	Executa uma determinada task
*/
RunTarget(target);