using Cake.Core;
using Cake.Core.Annotations;
using System;

namespace Cake.DotNetConf.Addin
{
	public static class Hello
	{
		[CakeMethodAlias]
		public static void MandarUmAlo(this ICakeContext context)
		{

			Console.WriteLine("Olá dotnet conf, estou rodando por um addin!!!");

		}

		[CakeMethodAlias]
		public static int Somar(this ICakeContext context, int a, int b)
		{
			return a + b;
		}

	}
}
